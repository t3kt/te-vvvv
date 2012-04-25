// statics.cpp - static functions and instance wrapper functions

#include "VideoDelay.h"
#include <stdlib.h>

typedef VideoDelay* LPINST;

// Plugin Globals
PlugInfoStruct GPlugInfo;
PlugExtendedInfoStruct GPlugExtInfo;
ParamConstsStruct GParamConstants[NUM_PARAMS];
OutputConstsStruct GOutputConstants[NUM_OUTPUTS];

PlugInfoStruct *getInfo()
{
	GPlugInfo.APIMajorVersion = 1;					// number before decimal point in version nums
	GPlugInfo.APIMinorVersion = 1;					// this is the number after the decimal point
													// so version 0.511 has major num 0, minor num 501
	char ID[FF_INFO_ID_LENGTH+1]		= "vdl@";	// this *must* be unique to your plugin
													// see www.freeframe.org for a list of ID's already taken
	char name[FF_INFO_NAME_LENGTH+1]	= "VideoDelay";

	memcpy(GPlugInfo.uniqueID, ID, FF_INFO_ID_LENGTH);
	memcpy(GPlugInfo.pluginName, name, FF_INFO_NAME_LENGTH);
	GPlugInfo.pluginType = FF_EFFECT;

	return &GPlugInfo;
}

LPVOID getExtendedInfo()
{
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
	GParamConstants[PARAM_BUFFER_LENGTH].Default = DEFAULT_BUFFER_LENGTH;
	char bufLenName[FF_PARAM_NAME_LENGTH + 1] = "Buffer Length";
	memcpy(GParamConstants[PARAM_BUFFER_LENGTH].Name, bufLenName, FF_PARAM_NAME_LENGTH);

	GParamConstants[PARAM_FRAME_OFFSET].Type = FF_TYPE_STANDARD;
	GParamConstants[PARAM_FRAME_OFFSET].Default = DEFAULT_FRAME_OFFSET;
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
	switch( index )
	{
	case FF_CAP_16BITVIDEO:			return FF_FALSE;
	case FF_CAP_24BITVIDEO:			return FF_TRUE;
	case FF_CAP_32BITVIDEO:			return FF_FALSE;
	case FF_CAP_PROCESSFRAMECOPY:	return FF_FALSE;
	case FF_CAP_MINIMUMINPUTFRAMES:	return NUM_INPUTS;
	case FF_CAP_MAXIMUMINPUTFRAMES:	return NUM_INPUTS;
	case FF_CAP_COPYORINPLACE:		return FF_FALSE;
	default:						return FF_FALSE;
	}
}

LPVOID instantiate( VideoInfoStruct* pVideoInfo )
{
	LPINST pInstance;

	pInstance = (LPINST)malloc( sizeof(VideoDelay) );
	if( pInstance->init( pVideoInfo ) )
		return pInstance;
	return NULL;
}

DWORD deInstantiate(LPVOID instanceID)
{
	LPINST pInstance = (LPINST)instanceID;

	pInstance->dispose();

	free( pInstance );

	return FF_SUCCESS;
}

DWORD setInput(LPVOID instanceID, InputStruct* pInput)
{
	LPINST pInstance = (LPINST)instanceID;
	
	if( pInput->SliceCount == 0 )
		return FF_FAIL;

	return pInstance->setParameter( pInput->Index, pInput->Spread[0] );
}

char* getParameterDisplay(LPVOID instanceID, DWORD param)
{
	LPINST pInstance = (LPINST)instanceID;
	return pInstance->getParameterDisplay( param );
}

DWORD setParameter(LPVOID instanceID, SetParameterStruct* setParam)
{
	LPINST pInstance = (LPINST)instanceID;
	return pInstance->setParameter( setParam->index, setParam->value );
}

float getParameter(LPVOID instanceID, DWORD param)
{
	LPINST pInstance = (LPINST)instanceID;
	return pInstance->getParameter( param );
}

DWORD getOutputSliceCount(LPVOID instanceID, DWORD index)
{
	LPINST pInstance = (LPINST)instanceID;
	//TODO: implement this
	return NULL;
}

float* getOutput(LPVOID instanceID, DWORD index)
{
	LPINST pInstance = (LPINST)instanceID;
	//TODO: implement this
	return NULL;
}

DWORD processFrame(LPVOID instanceID, LPVOID pFrame)
{
	LPINST pInstance = (LPINST)instanceID;

	return pInstance->processFrame( (VideoPixel24bit*)pFrame );
}

DWORD processFrameCopy(LPVOID instanceID, ProcessFrameCopyStruct* pFrameData)
{
	//LPINST pInstance = (LPINST)instanceID;
	return FF_FAIL;
}

DWORD setThreadLock(LPVOID instanceID, DWORD enter)
{
	LPINST pInstance = (LPINST)instanceID;

	if(*(bool*)enter)
		pInstance->enterLock();
	else
		pInstance->leaveLock();

	return FF_SUCCESS;
}
