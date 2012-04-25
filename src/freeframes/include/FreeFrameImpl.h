// FreeFrameImpl.h - implementation-related stuff that isn't specific to a single plugin but isn't part of the (extended) standard
#ifndef __FREEFRAMEIMPL_H__
#define __FREEFRAMEIMPL_H__

#include "FreeFrame.h"
#include "FreeFrameExt.h"

#define FF_PARAM_NAME_LENGTH	16
#define FF_INFO_ID_LENGTH		4
#define FF_INFO_NAME_LENGTH		16
#define FF_PARAM_DISPLAY_LENGTH	16

//#ifndef PLUGIN_INSTANCE_POINTER
//#define PLUGIN_INSTANCE_POINTER LPVOID
//#endif

PlugInfoStruct*	getInfo();

DWORD	initialise();

DWORD	deInitialise();

DWORD	getNumParameters();

ParamConstsStruct *getParameterInfo( DWORD index );

inline char* getParameterName(DWORD index)
{
	ParamConstsStruct* pInfo = getParameterInfo( index );
	if( pInfo )
		return pInfo->Name;
	return NULL;
};

inline float getParameterDefault(DWORD index)
{
	ParamConstsStruct* pInfo = getParameterInfo( index );
	if( pInfo )
		return pInfo->Default;
	return 0;
}

inline unsigned int getParameterType(DWORD index)
{
	ParamConstsStruct* pInfo = getParameterInfo( index );
	if( pInfo )
		return pInfo->Type;
	return NULL;
}

DWORD	getPluginCaps(DWORD index);

LPVOID instantiate(VideoInfoStruct* pVideoInfo);

DWORD deInstantiate(LPVOID pInstance);

LPVOID getExtendedInfo();

//freeframe 1.0 extended. see: http://vvvv.org/tiki-index.php?page=FreeFrameExtendedSpecification
DWORD	getNumOutputs();

OutputConstsStruct* getOutputInfo( DWORD index );

inline unsigned int getOutputType(DWORD index)
{
	OutputConstsStruct* pInfo = getOutputInfo( index );
	if( pInfo )
		return pInfo->Type;
	return 0;
}

inline char*	getOutputName(DWORD index)
{
	
	OutputConstsStruct* pInfo = getOutputInfo( index );
	if( pInfo )
		return pInfo->Name;
	return NULL;
}

char* getParameterDisplay(LPVOID pInstance, DWORD param);

DWORD setParameter(LPVOID pInstance, SetParameterStruct* setParam);

DWORD processFrame(LPVOID pInstance, LPVOID pFrame);

float getParameter(LPVOID pInstance, DWORD param);

DWORD processFrameCopy(LPVOID pInstance, ProcessFrameCopyStruct* pFrameData);

DWORD getOutputSliceCount(LPVOID pInstance, DWORD index);

float* getOutput(LPVOID pInstance, DWORD index);

DWORD setThreadLock(LPVOID pInstance, DWORD index);

DWORD setInput(LPVOID pInstance, InputStruct* pInput);

inline DWORD getBitsForDepth( DWORD bitDepth )
{
	switch( bitDepth )
	{
	case FF_CAP_16BITVIDEO: return 16;
	case FF_CAP_24BITVIDEO: return 24;
	case FF_CAP_32BITVIDEO: return 32;
	default: return 0;
	}
}


#endif
