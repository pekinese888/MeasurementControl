// EnergyMeasurement.cpp : Defines the entry point for the console application.
//
#define _CRT_NONSTDC_NO_WARNINGS


#include "stdafx.h"

#include <iostream>
#include <fstream>
#include <cstdlib>
#include <cstdio>
#include <string>
#include <time.h>
#include <signal.h>
#include <Windows.h>
#include <chrono>
#include <ctime>
#include <algorithm>
using namespace std;

#include "visa.h"

static ViSession defaultRM;
static ViSession instr;
static ViUInt32 numInstrs;
static ViFindList findList;
static ViUInt32 retCount;
static ViUInt32 writeCount;
static ViStatus status;
static char instrResourceString[VI_FIND_BUFLEN];

static char unsigned buffer[100];
static char *sbuffer = (char*)buffer;
static char stringinput[512];

static chrono::high_resolution_clock::time_point ModeStart = chrono::high_resolution_clock::now();
static chrono::high_resolution_clock::duration ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);

/*	Array for Off Mode Measurements
Measurement Number, Time from 0 (start of measurement), Voltage, Current, Power
*/
static double OffMode[50000][5] = { { 0 },{ 0 } };

/*	Array for current Measurements
Measurement Number, Time from 0 (start of measurement), Voltage, Current, Power
*/
static double ShortIdleMode[50000][5] = { { 0 },{ 0 } };

/*	Array for current Measurements
Measurement Number, Time from 0 (start of measurement), Voltage, Current, Power
*/
static double LongIdleMode[50000][5] = { { 0 },{ 0 } };

/*	Array for current Measurements
Measurement Number, Time from 0 (start of measurement), Voltage, Current, Power
*/
static double SleepMode[50000][5] = { { 0 },{ 0 } };

/*	Function: Accumulate Measurements
*	Measures Voltage, Current and Power and writes all Values to the first Row of measurements which contains a 0 in the first Column
*	measurementStart needs to be initialized by calling clock() prior to the first call of Accumulate Measurements
*/
void AccumulateMeasurements(double measurements[50000][5]);

/*	Function: Check for Stability
*	Checks if the last 2/3 of the supplied Measurements are Stable by taking a Linear Regression over the last 2/3
*	and determining if the Slope of the Linear Regression is below a certain Value
*	Returns True if the Measurement is stable, false otherwise.
*	If bool reuseValues is true the function assumes to be called again after only one Measurement is added. So it does not need to process the whole array.
*/
bool IsStable(double measurements[50000][5], bool reuseValues);

/* Functions to calculate Max,Min and Avg Power Value of the last 5 min
*/

double MaxPower(double measurements[50000][5]);

double MinPower(double measurements[50000][5]);

double AvgPower(double measurements[50000][5]);

/*	Function: Wait for Enter
*	Waits for Enter Input to proceed
*/
void WaitForEnter();

/*	Function: Wait for xx ms
*	Waits for ammount of ms
*/
void WaitForMS(unsigned long);

/*
*	Fuction: SIGINIT Handler
*/
void siginthandler(int);

/*Entrypoint*/
int main(void)
{
	int i;

	/*
	* First we must call viOpenDefaultRM to get the manager
	* handle.  We will store this handle in defaultRM.
	*/
	status = viOpenDefaultRM(&defaultRM);
	if (status < VI_SUCCESS)
	{
		wprintf(L"Could not open a session to the VISA Resource Manager!\n");
		exit(EXIT_FAILURE);
	}

	/* Find the TCPIP TMC VISA Instrument */
	wprintf(L"Searching for Intrument... \n");
	status = viFindRsrc(defaultRM, const_cast<ViString>("TCPIP?*INSTR"), &findList, &numInstrs, instrResourceString);

	if (status < VI_SUCCESS)
	{
		wprintf(L"An error occurred while finding resources.\nHit enter to continue.");
		fflush(stdin);
		getchar();
		viClose(defaultRM);
		return status;
	}

	
	/* Connect to First TCPIP TMC VISA Instrument found*/
	status = viOpen(defaultRM, instrResourceString, VI_NULL, VI_NULL, &instr);
	if (status < VI_SUCCESS)
	{
		wprintf(L"Cannot open a session to the device.\n");
		return status;
	}

	/*
	* At this point we now have a session open to the TCPIP TMC instrument.
	* We will now use the viWrite function to send the device the string "*IDN?\n",
	* asking for the device's identification.
	*/
	strcpy_s(stringinput, "*IDN?\n");
	status = viWrite(instr, (ViBuf)stringinput, (ViUInt32)strlen(stringinput), &writeCount);
	if (status < VI_SUCCESS)
	{
		printf("Error writing *IDN?\n to the device.\n");
		status = viClose(instr);
		return status;
	}

	/*
	* Now we will attempt to read back a response from the device to
	* the identification query that was sent.  We will use the viRead
	* function to acquire the data.  We will try to read back 100 bytes.
	* This function will stop reading if it finds the termination character
	* before it reads 100 bytes.
	* After the data has been read the response is displayed.
	*/
	status = viRead(instr, buffer, 100, &retCount);
	if (status < VI_SUCCESS)
	{
		wprintf(L"Error reading a response from the device.\n");
	}
	else
	{
		printf("Connected to Device: %*s\n", retCount, buffer);
	}
	buffer[0] = '\0'; //Clear Buffer
	
	/* Let User decide for Measurement Mode
	*  1. Measurement according to Energystar 6.1
	*  *** Room for more Modes ***
	*/
	int mode = 0;
	do
	{
		wprintf(L"Please select Measurement Mode:\n");
		wprintf(L"1. Measurement according to Energystar 6.1\n");
		scanf_s("%99s", buffer, (unsigned)_countof(buffer));
		switch (atoi((char*)buffer)) {
		case 1:
			wprintf(L"Starting Measurement according to Energystar 6.1\nResults are saved in Results.csv\n\n");
			mode = 1;
			break;
		default:
			wprintf(L"Wrong Input!\n\n");
			mode = 0;
			break;
		}
		buffer[0] = '\0'; //Clear Buffer
	} while (mode == 0);

	/*Initialize Variables*/
	bool once = true;
	bool initialize = true;
	bool wasStable = false;
	chrono::high_resolution_clock::duration minTotalPeriod = 900000ms; // 15 min Minimum Period
	chrono::high_resolution_clock::duration maxTotalPeriod = 10800000ms; // 3h Maximum Period
	time_t CurrentDateAndTime = chrono::system_clock::to_time_t(chrono::system_clock::now());
	wchar_t buffer[30];
	ofstream myfile;
	wstring fileResultscsv = L"Results.csv";
	wstring fileOffModecsv = L"OffMode.csv";
	wstring fileShortIdleModecsv = L"ShortIdle.csv";
	wstring fileLongIdleModecsv = L"LongIdle.csv";
	wstring fileSleepModecsv = L"Sleep.csv";
	wstring folderName = L"\0";

	/*Create Folder for Results*/
	if (_wctime_s(buffer, 30, &CurrentDateAndTime) == NULL) { folderName = buffer; }
	else
	{
		wcout << L"Unable to create Folder. Aborting execution.\n";
		wprintf(L"Disconnection from Instrument.\n");
		status = viClose(instr);
		status = viClose(defaultRM);
		return -1;

	}
	folderName = buffer;
	folderName.erase(remove(folderName.begin(), folderName.end(), '\n'), folderName.end());
	folderName.erase(remove(folderName.begin(), folderName.end(), ':'), folderName.end());
	folderName = L".\\" + folderName + L"\\";
	wcout << folderName;
	CreateDirectory(folderName.c_str(), NULL);

	switch (mode) {
	case 1: // Measurement according to Energystar 6.1
		/*Prepare UUT*/
		wprintf(L"Resetting PWR Meter.");
		strcpy_s(stringinput, "*RST?\n");
		myfile.open((folderName + fileResultscsv));
		myfile << "Mode, MaxPower, MinPower, AvgPower\n";
		myfile.close();
		status = viWrite(instr, (ViBuf)stringinput, (ViUInt32)strlen(stringinput), &writeCount);
		if (status < VI_SUCCESS)
		{
			wprintf(L"Disconnection from Instrument.\n");
			status = viClose(instr);
			status = viClose(defaultRM);
			return -1;
		}
		wprintf(L"Please make sure to Setup the UUT correctly:\n\n"
			L"1. Connect UUT to PWR Meter\n"
			L"2. Load BIOS Defaults\n"
			L"3. Make Sure WOL and ASPM are configured how you need it\n"
			L"4. Install Clean OS with newest Drivers\n"
			L"5. Disable Sleep, LCD and HDD Timeout (Set to 0 Minutes)\n"
			L"6. Run Maintenance Tasks - If possible leave the System Idle for Multiple Hours without entering Sleep\n"
			L"7. Make sure to take out the Battery of Notebooks and Tablets\n"
			L"8. Calibrate Notebooks to 90 Nits and Tablets/Slates to 150 Nits\n\n"
			L"Shut down UUT and press Enter to start measurements...\n"
		);
		WaitForEnter();

		/*Measure Off Mode*/
		wprintf(	L"Measuring off Mode in Progress...\n"
				L"This will take 15-180 Minutes\n"
		);
		ModeStart = std::chrono::high_resolution_clock::now();
		once = true;
		initialize = true;
		wasStable = false;
		buffer[0] = '\0'; //Clear Buffer
		do {
			wprintf(L"Measure...\n");
			AccumulateMeasurements(OffMode);
			if ((ModeTimePassed > minTotalPeriod) && (once == false) && (initialize == true)) {
				initialize = false;
			}
			if ((ModeTimePassed > minTotalPeriod) && (once==true)) {
				once = false;
			}
			ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);
			WaitForMS(250 - (chrono::duration_cast<chrono::milliseconds>(ModeTimePassed).count() % 250));
			signal(SIGINT, siginthandler);
			if ((ModeTimePassed > minTotalPeriod)) {
				wprintf(L"Check for stability...\n");
				wasStable = IsStable(OffMode, !initialize);
				if (wasStable) break;
			}
		} while ((ModeTimePassed <= minTotalPeriod) || (!wasStable && (ModeTimePassed <= maxTotalPeriod)));

		myfile.open((folderName + fileOffModecsv));
		for (int i = 0; i < (unsigned)_countof(OffMode); i++) {
			if (OffMode[i][0] == 0) break;
			myfile << (unsigned long)OffMode[i][0] << ", " << (unsigned long)OffMode[i][1] << ", " << OffMode[i][2] << ", " << OffMode[i][3] << ", " << OffMode[i][4] << "\n";
		}
		myfile.close();

		if (wasStable) {
			wprintf(L"Stability was established. Measurements are saved in OffMode.csv.\nResults saved in Results.csv\n");

			myfile.open((folderName + fileResultscsv), ios::app);
			myfile << "OffMode, " << MaxPower(OffMode) << ", " << MinPower(OffMode) << ", " << AvgPower(OffMode) << "\n";
			myfile.close();
		} 
		else
		{
			wprintf(L"Stability could not be established. Please inspect OffMode.csv manually for cyclic behavior\n");
		}

		

		ModeStart = chrono::high_resolution_clock::now();
		ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);


		/*Measure Short Idle*/
		wprintf(L"Please Start UUT and wait until Windows has started, then press Enter.\n");
		WaitForEnter();

		wprintf(L"Measuring Short Idle Mode in Progress...\n"
			L"This will take 15-180 Minutes\n"
		);
		ModeStart = chrono::high_resolution_clock::now();
		once = true;
		initialize = true;
		wasStable = false;
		buffer[0] = '\0'; //Clear Buffer
		do {
			wprintf(L"Measure...\n");
			AccumulateMeasurements(ShortIdleMode);
			if ((ModeTimePassed > minTotalPeriod) && (once == false) && (initialize == true)) {
				initialize = false;
			}
			if ((ModeTimePassed > minTotalPeriod) && (once == true)) {
				once = false;
			}
			ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);
			WaitForMS(250 - (chrono::duration_cast<chrono::milliseconds>(ModeTimePassed).count() % 250));
			signal(SIGINT, siginthandler);
			if ((ModeTimePassed > minTotalPeriod)) {
				wprintf(L"Check for stability...\n");
				wasStable = IsStable(ShortIdleMode, !initialize);
				if (wasStable) break;
			}
		} while ((ModeTimePassed <= minTotalPeriod) || (!wasStable && (ModeTimePassed <= maxTotalPeriod)));

		myfile.open((folderName + fileShortIdleModecsv));
		for (int i = 0; i < (unsigned)_countof(ShortIdleMode); i++) {
			if (ShortIdleMode[i][0] == 0) break;
			myfile << (unsigned long)ShortIdleMode[i][0] << ", " << (unsigned long)ShortIdleMode[i][1] << ", " << ShortIdleMode[i][2] << ", " << ShortIdleMode[i][3] << ", " << ShortIdleMode[i][4] << "\n";
		}
		myfile.close();

		if (wasStable) {
			wprintf(L"Stability was established. Measurements are saved in ShortIdleMode.csv.\nResults saved in Results.csv\n");

			myfile.open((folderName + fileResultscsv), ios::app);
			myfile << "ShortIdleMode, " << MaxPower(ShortIdleMode) << ", " << MinPower(ShortIdleMode) << ", " << AvgPower(ShortIdleMode) << "\n";
			myfile.close();
		}
		else
		{
			wprintf(L"Stability could not be established. Please inspect ShortIdleMode.csv manually for cyclic behavior\n");
		}



		ModeStart = chrono::high_resolution_clock::now();
		ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);


		/*Measure Long Idle*/
		wprintf(L"Please set Display and HDD Timeout to standard Values (10min). If LCD has just blanked, please press Enter.\n");
		WaitForEnter();

		wprintf(L"Measuring Short Idle Mode in Progress...\n"
			L"This will take 15-180 Minutes\n"
		);
		ModeStart = chrono::high_resolution_clock::now();
		once = true;
		initialize = true;
		wasStable = false;
		buffer[0] = '\0'; //Clear Buffer
		do {
			wprintf(L"Measure...\n");
			AccumulateMeasurements(LongIdleMode);
			if ((ModeTimePassed > minTotalPeriod) && (once == false) && (initialize == true)) {
				initialize = false;
			}
			if ((ModeTimePassed > minTotalPeriod) && (once == true)) {
				once = false;
			}
			ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);
			WaitForMS(250 - (chrono::duration_cast<chrono::milliseconds>(ModeTimePassed).count() % 250));
			signal(SIGINT, siginthandler);
			if ((ModeTimePassed > minTotalPeriod)) {
				wprintf(L"Check for stability...\n");
				wasStable = IsStable(LongIdleMode, !initialize);
				if (wasStable) break;
			}
		} while ((ModeTimePassed <= minTotalPeriod) || (!wasStable && (ModeTimePassed <= maxTotalPeriod)));

		myfile.open((folderName + fileLongIdleModecsv));
		for (int i = 0; i < (unsigned)_countof(LongIdleMode); i++) {
			if (LongIdleMode[i][0] == 0) break;
			myfile << (unsigned long)LongIdleMode[i][0] << ", " << (unsigned long)LongIdleMode[i][1] << ", " << LongIdleMode[i][2] << ", " << LongIdleMode[i][3] << ", " << LongIdleMode[i][4] << "\n";
		}
		myfile.close();

		if (wasStable) {
			wprintf(L"Stability was established. Measurements are saved in LongIdleMode.csv.\nResults saved in Results.csv\n");

			myfile.open((folderName + fileResultscsv), ios::app);
			myfile << "LongIdleMode, " << MaxPower(LongIdleMode) << ", " << MinPower(LongIdleMode) << ", " << AvgPower(LongIdleMode) << "\n";
			myfile.close();
		}
		else
		{
			wprintf(L"Stability could not be established. Please inspect LongIdleMode.csv manually for cyclic behavior\n");
		}



		ModeStart = chrono::high_resolution_clock::now();
		ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);

		/*Measure Sleep Mode*/
		wprintf(L"Please manually enter Sleep. Then press Enter.\n");
		WaitForEnter();

		wprintf(L"Measuring Short Idle Mode in Progress...\n"
			L"This will take 15-180 Minutes\n"
		);
		ModeStart = chrono::high_resolution_clock::now();
		once = true;
		initialize = true;
		wasStable = false;
		buffer[0] = '\0'; //Clear Buffer
		do {
			wprintf(L"Measure...\n");
			AccumulateMeasurements(SleepMode);
			if ((ModeTimePassed > minTotalPeriod) && (once == false) && (initialize == true)) {
				initialize = false;
			}
			if ((ModeTimePassed > minTotalPeriod) && (once == true)) {
				once = false;
			}
			ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);
			WaitForMS(250 - (chrono::duration_cast<chrono::milliseconds>(ModeTimePassed).count() % 250));
			signal(SIGINT, siginthandler);
			if ((ModeTimePassed > minTotalPeriod)) {
				wprintf(L"Check for stability...\n");
				wasStable = IsStable(SleepMode, !initialize);
				if (wasStable) break;
			}
		} while ((ModeTimePassed <= minTotalPeriod) || (!wasStable && (ModeTimePassed <= maxTotalPeriod)));

		myfile.open((folderName + fileSleepModecsv));
		for (int i = 0; i < (unsigned)_countof(SleepMode); i++) {
			if (SleepMode[i][0] == 0) break;
			myfile << (unsigned long)SleepMode[i][0] << ", " << (unsigned long)SleepMode[i][1] << ", " << SleepMode[i][2] << ", " << SleepMode[i][3] << ", " << SleepMode[i][4] << "\n";
		}
		myfile.close();

		if (wasStable) {
			wprintf(L"Stability was established. Measurements are saved in SleepMode.csv.\nResults saved in Results.csv\n");

			myfile.open((folderName + fileResultscsv), ios::app);
			myfile << "SleepMode, " << MaxPower(SleepMode) << ", " << MinPower(SleepMode) << ", " << AvgPower(SleepMode) << "\n";
			myfile.close();
		}
		else
		{
			wprintf(L"Stability could not be established. Please inspect SleepMode.csv manually for cyclic behavior\n");
		}



		ModeStart = chrono::high_resolution_clock::now();
		ModeTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);

		/*Export Measured Data to CSV File*/
		wprintf(L"Measurements finished. Results saved in Results.csv");
		/**/
		break;
	default:
		wprintf(L"Unknown Mode Selected!\n");
		break;
	}


	/*Only here for Reference
	for (int i = 0; i < 4; i++) {
		clock_t before = clock();
		strcpy_s(stringinput, "MEAS?V,I,W\n");
		status = viWrite(instr, (ViBuf)stringinput, (ViUInt32)strlen(stringinput), &writeCount);
		if (status < VI_SUCCESS)
		{
			printf("Error writing to the device %d.\n", i + 1);
			status = viClose(instr);
			continue;
		}
		status = viRead(instr, buffer, 100, &retCount);
		if (status < VI_SUCCESS)
		{
			printf("Error reading a response from the device %d.\n", i + 1);
		}
		else
		{
			printf("\nVolt: %*s\n", retCount, buffer);
			printf("Time taken: %d \n", ((clock() - before) * 1000) / CLOCKS_PER_SEC);
			buffer[0] = '\0'; //Clear Buffer
		}
		
	}
	*/



	/*
	* Now we will close the session to the instrument using
	* viClose. This operation frees all system resources.
	*/
	wprintf(L"Disconnection from Instrument.\n");
	status = viClose(instr);
	status = viClose(defaultRM);
	return 0;
}

void AccumulateMeasurements(double measurements[50000][5]) {
	static char unsigned buffer[100];
	static long measureCount = 0;
	static chrono::high_resolution_clock::time_point measurementStart = chrono::high_resolution_clock::now();
	static chrono::high_resolution_clock::duration measurementTimePassed = 0ms;
	double volt = 0;
	double ampere = 0;
	double watt = 0;
	if (measurements[0][0] == 0) {
		measureCount = 0;
		measurementStart = chrono::high_resolution_clock::now();
		measurementTimePassed = 0ms;
	}
	measureCount++;
	measurementTimePassed = chrono::duration_cast<chrono::milliseconds>(chrono::high_resolution_clock::now() - ModeStart);
	/*Measure Values*/
	strcpy_s(stringinput, "FETC?V,I,W\n");
	status = viWrite(instr, (ViBuf)stringinput, (ViUInt32)strlen(stringinput), &writeCount);
	if (status < VI_SUCCESS)
	{
		printf("Error writing to the device.\n");
		status = viClose(instr);
	}
	status = viRead(instr, buffer, 100, &retCount);
	if (status < VI_SUCCESS)
	{
		printf("Error reading a response from the device.\n");
	}
	else
	{
		printf("%*s\n", retCount, buffer);
	}
	/*Convert Values*/
	sscanf_s((char*)buffer, "%lf,%lf,%lf", &volt, &ampere, &watt);
	/*Save Values*/
	measurements[measureCount - 1][0] = measureCount;
	measurements[measureCount - 1][1] = chrono::duration_cast<chrono::milliseconds>(measurementTimePassed).count();
	measurements[measureCount - 1][2] = volt;
	measurements[measureCount - 1][3] = ampere;
	measurements[measureCount - 1][4] = watt;

	measurements[measureCount][0] = 0;
	for (int i = 0; i < 100; i++)buffer[i] = '\0'; //Clear Buffer
}

bool IsStable(double measurements[50000][5], bool reuseValues) {
	static unsigned long totalMeasurements = 0;
	static unsigned long beginOfSecondTwoThird = 0;
	static double avgPower = 0; // max Power in W
	static double sum_xy = 0; // ms*W
	static double sum_x = 0; // ms
	static double sum_y = 0; // W
	static double sum_xsqr = 0; // (ms)²
	static double slope = 0; //Slope in W/ms

	if (reuseValues == false) {
		totalMeasurements = 0;
		beginOfSecondTwoThird = 0;
		avgPower = 0;
		sum_xy = 0;
		sum_x = 0;
		sum_y = 0;
		sum_xsqr = 0;

		// Check Number of Total Measurements in array
		for (int i = 0; i < 50000; i++) {
			if (measurements[i][0] == 0) {
				totalMeasurements = unsigned long(measurements[i - 1][0]);
				break;
			}
		}

		// Set begin of Regression Interval
		beginOfSecondTwoThird = (totalMeasurements / 3);

		// Determine Average Power to check for Stability Criterium
		for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
			avgPower = avgPower + measurements[i][4];
		}
		avgPower = avgPower / (totalMeasurements - beginOfSecondTwoThird);

		// Calculate sum x*y over all Power Measurements in Regression Interval
		for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
			sum_xy = sum_xy + measurements[i][1] * measurements[i][4];
		}

		// Calculate sum x over all Power Measurements in Regression Interval
		for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
			sum_x = sum_x + measurements[i][1];
		}

		// Calculate sum y over all Power Measurements in Regression Interval
		for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
			sum_y = sum_y + measurements[i][4];
		}

		// Calculate sum x² over all Power Measurements in Regression Interval
		for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
			sum_xsqr = sum_xsqr + measurements[i][1] * measurements[i][1];
		}
	}
	else {

		// Determine Average Power to check for Stability Criterium
		avgPower = avgPower * (totalMeasurements - beginOfSecondTwoThird);

		//Increase Number of Total Measurements and assign new begin of second two thirds
		totalMeasurements++;
		if (beginOfSecondTwoThird < (totalMeasurements / 3)) {
			//DEBUG printf("Gets removed: {%lf,%lf}\n", measurements[beginOfSecondTwoThird][1], measurements[beginOfSecondTwoThird][4]);

			//Remove Value from Sum x*y which got dropped out
			sum_xy = sum_xy - measurements[beginOfSecondTwoThird][1] * measurements[beginOfSecondTwoThird][4];

			//Remove Value from Sum x which got dropped out
			sum_x = sum_x - measurements[beginOfSecondTwoThird][1];

			//Remove Value from Sum y which got dropped out
			sum_y = sum_y - measurements[beginOfSecondTwoThird][4];

			//Remove Value from Sum x² which got dropped out
			sum_xsqr = sum_xsqr - measurements[beginOfSecondTwoThird][1] * measurements[beginOfSecondTwoThird][1];

			//Remove Value from avgPower which got dropped out
			avgPower = avgPower - measurements[beginOfSecondTwoThird][4];

			//Set new Begin of Second Two Thirds
			beginOfSecondTwoThird = (totalMeasurements / 3);
		}

		// Determine Average Power to check for Stability Criterium
		avgPower = (avgPower + measurements[totalMeasurements - 1][4]);
		avgPower = avgPower / (totalMeasurements - beginOfSecondTwoThird);

		//DEBUG printf("Is added: {%lf,%lf}\n", measurements[totalMeasurements - 1][1], measurements[totalMeasurements - 1][4]);
		// Calculate new sum x*y over all Power Measurements in Regression Interval
		sum_xy = sum_xy + measurements[totalMeasurements - 1][1] * measurements[totalMeasurements - 1][4];

		// Calculate new sum x over all Power Measurements in Regression Interval
		sum_x = sum_x + measurements[totalMeasurements - 1][1];

		// Calculate new sum y over all Power Measurements in Regression Interval
		sum_y = sum_y + measurements[totalMeasurements - 1][4];

		// Calculate new sum x² over all Power Measurements in Regression Interval
		sum_xsqr = sum_xsqr + measurements[totalMeasurements - 1][1] * measurements[totalMeasurements - 1][1];
	}

	//DEBUG
	/*
	printf("\n\n For checking: \n");
	printf("Begin Of Regression Interval: %d\n", beginOfSecondTwoThird);
	printf("Total Measurements: %d\n", totalMeasurements);
	printf("{");

	for (int i = beginOfSecondTwoThird; i < totalMeasurements; i++) {
	if (measurements[i][0] == 0) break;
	printf("{%d,%lf},", unsigned long(measurements[i][1]), measurements[i][4]);
	}

	printf("}\n\n");
	*/


	slope = 3600000000 * (((totalMeasurements - beginOfSecondTwoThird)*sum_xy - ((sum_x)*(sum_y))) / ((totalMeasurements - beginOfSecondTwoThird)*sum_xsqr - pow(sum_x, 2)));
	printf("Current Slope in mW/h: %lf\n", slope);

	if (avgPower < 1) {
		if (abs(slope) < 10) return true;
	}
	else
	{
		if (abs(slope) < (avgPower * 1000 / 100)) return true;
	}


	return false;
}

double MaxPower(double measurements[50000][5]) {
	double MaxPower = 0;
	double TotalRuntime = 0;

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) { 
			TotalRuntime = measurements[i - 1][1];
			break; 
		}

	}

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) {
			break;
		}
		if ((TotalRuntime - measurements[i][1]) <= 300250) {
			if(MaxPower < measurements[i][4])MaxPower = measurements[i][4];
		}
	}
	return MaxPower;
}

double MinPower(double measurements[50000][5]) {
	double MinPower = 0;
	double TotalRuntime = 0;

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) {
			TotalRuntime = measurements[i - 1][1];
			break;
		}

	}

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) {
			break;
		}
		if ((TotalRuntime - measurements[i][1]) <= 300250) {
			if (MinPower == 0)MinPower = measurements[i][4];
			if (MinPower > measurements[i][4])MinPower = measurements[i][4];
		}
	}
	return MinPower;
}

double AvgPower(double measurements[50000][5]) {
	double AvgPower = 0;
	double BeginOfTotalRuntime = 0;
	double EndOfTotalRuntime = 0;
	double TotalRuntime = 0;

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) {
			TotalRuntime = measurements[i - 1][1];
			EndOfTotalRuntime = measurements[i - 1][0];
			break;
		}

	}

	for (int i = 0; i < 50000; i++) {
		if (measurements[i][0] == 0) {
			break;
		}
		if ((TotalRuntime - measurements[i][1]) <= 300250) {
			if (BeginOfTotalRuntime == 0) BeginOfTotalRuntime = measurements[i][0];
			if (AvgPower != 0)AvgPower = ((AvgPower * ((i + 1) - BeginOfTotalRuntime)) + measurements[i][4]) / ((i + 2) - BeginOfTotalRuntime);
			if (AvgPower == 0)AvgPower = measurements[i][4];
		}
	}
	return AvgPower;
}


void WaitForEnter() {
	static char ch = '\0';
	while (ch != '\n')
	{
		ch = getchar();
	}
	ch = getchar();
	while (ch != '\n')
	{
		ch = getchar();
	}
}

void WaitForMS(unsigned long delay) {
	clock_t start = ((clock() * 1000) / CLOCKS_PER_SEC);
	while ((((clock() * 1000) / CLOCKS_PER_SEC) - start) <= delay);
}

void siginthandler(int param)
{
	status = viClose(instr);
	status = viClose(defaultRM);
	exit(1);
}