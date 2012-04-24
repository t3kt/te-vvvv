// VideoDelay.cpp : Defines the exported functions for the DLL application.
//

#include "VideoDelay.h"
#include <stdlib.h>

// Plugin Globals
PlugInfoStruct GPlugInfo;
PlugExtendedInfoStruct GPlugExtInfo;
ParamConstsStruct GParamConstants[NUM_PARAMS];
OutputConstsStruct GOutputConstants[NUM_OUTPUTS];

PlugInfoStruct *getInfo()
{
	//TODO: implement this
	GPlugInfo.APIMajorVersion = 1;		// number before decimal point in version nums
	GPlugInfo.APIMinorVersion = 1;		// this is the number after the decimal point
										// so version 0.511 has major num 0, minor num 501
	char ID[FF_INFO_ID_LENGTH+1] = "zDEL";		 // this *must* be unique to your plugin
								 // see www.freeframe.org for a list of ID's already taken
	char name[FF_INFO_NAME_LENGTH+1] = "VideoDelay";

	memcpy(GPlugInfo.uniqueID, ID, FF_INFO_ID_LENGTH);
	memcpy(GPlugInfo.pluginName, name, FF_INFO_NAME_LENGTH);
	GPlugInfo.pluginType = FF_EFFECT;

	return &GPlugInfo;
}

LPVOID getExtendedInfo()
{
	//TODO: implement this

	GPlugExtInfo.PluginMajorVersion = 1;
	GPlugExtInfo.PluginMinorVersion = 1;

	// I'm just passing null for description etc for now
	// todo: send through description and about
	GPlugExtInfo.Description = NULL;
	GPlugExtInfo.About = NULL;

	// FF extended data block is not in use by the API yet
	// we will define this later if we want to
	GPlugExtInfo.FreeFrameExtendedDataSize = 0;
	GPlugExtInfo.FreeFrameExtendedDataBlock = NULL;

	return (LPVOID) &GPlugExtInfo;
}

DWORD initialise()
{
	GParamConstants[PARAM_BUFFER_LENGTH].Type = FF_TYPE_STANDARD;
	GParamConstants[PARAM_BUFFER_LENGTH].Default = 100;
	char bufLenName[FF_PARAM_NAME_LENGTH + 1] = "Buffer Length";
	memcpy(GParamConstants[PARAM_BUFFER_LENGTH].Name, bufLenName, FF_PARAM_NAME_LENGTH);

	GParamConstants[PARAM_FRAME_OFFSET].Type = FF_TYPE_STANDARD;
	GParamConstants[PARAM_FRAME_OFFSET].Default = 10;
	char offsetName[FF_PARAM_NAME_LENGTH + 1] = "Frame Offset";
	memcpy(GParamConstants[PARAM_FRAME_OFFSET].Name, offsetName, FF_PARAM_NAME_LENGTH);
	
	return FF_SUCCESS;
}

DWORD deInitialise()
{
	//TODO: implement this
	return FF_SUCCESS;
}

DWORD getNumParameters() { return NUM_PARAMS; }

ParamConstsStruct *getParameterInfo( DWORD index )
{
	if( index > 0 && index < NUM_PARAMS )
		return &GParamConstants[index];
	return NULL;
}

DWORD	getNumOutputs() { return NUM_OUTPUTS; }

OutputConstsStruct* getOutputInfo( DWORD index )
{
	if( index > 0 && index < NUM_OUTPUTS )
		return &GOutputConstants[index];
	return NULL;
}

DWORD	getPluginCaps(DWORD index)
{
	//TODO: implement this
	return NULL;
}

LPVOID instantiate( VideoInfoStruct* pVideoInfo )
{
	VideoDelayInstance *pInstance;

	pInstance = (VideoDelayInstance*)malloc( sizeof(VideoDelayInstance) );

	for(int i = 0; i < NUM_PARAMS; i++)
	{
		pInstance->Params[i].Value = GParamConstants[i].Default;
	}

	InitializeCriticalSection(&pInstance->CriticalSection);

	//TODO: implement this

	return NULL;
}

DWORD deInstantiate(LPVOID instanceID)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;

	//TODO: implement this

	DeleteCriticalSection(&pInstance->CriticalSection);

	free( pInstance );

	return FF_SUCCESS;
}

char* getParameterDisplay(LPVOID instanceID, DWORD param)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

DWORD setParameter(LPVOID instanceID, SetParameterStruct* setParam)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

float getParameter(LPVOID instanceID, DWORD param)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

DWORD getOutputSliceCount(LPVOID instanceID, DWORD index)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

float* getOutput(LPVOID instanceID, DWORD index)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

DWORD setInput(LPVOID instanceID, InputStruct* pInput)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return NULL;
}

DWORD processFrame(LPVOID instanceID, LPVOID pFrame)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;

	EnterCriticalSection(&pInstance->CriticalSection);

	//TODO: implement this

	LeaveCriticalSection(&pInstance->CriticalSection);

	return FF_SUCCESS;
}

DWORD processFrameCopy(LPVOID instanceID, ProcessFrameCopyStruct* pFrameData)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;
	//TODO: implement this
	return FF_FAIL;
}

DWORD setThreadLock(LPVOID instanceID, DWORD enter)
{
	VideoDelayInstance* pInstance = (VideoDelayInstance*)instanceID;

	if(*(bool*)enter)
		EnterCriticalSection(&pInstance->CriticalSection);
	else
		LeaveCriticalSection(&pInstance->CriticalSection);

	return FF_SUCCESS;
}


