#ifndef __VIDEO_DELAY_H__
#define __VIDEO_DELAY_H__

//struct VideoDelayInstanceTag;
//#define PLUGIN_INSTANCE_POINTER (VideoDelayInstanceTag*)
//typedef VideoDelayInstanceTag* PLUGIN_INSTANCE_POINTER;

#include <FreeFrame.h>
#include <FreeFrameExt.h>
#include <FreeFrameImpl.h>
#include <FrameBuffer.h>

#define VIDEODELAY_API __declspec(dllexport)

#define NUM_INPUTS 1  //number of video inputs

const UINT32 DEFAULT_BUFFER_LENGTH = 100;
const UINT32 DEFAULT_FRAME_OFFSET  = 0;

enum
{
	PARAM_BUFFER_LENGTH,
	PARAM_FRAME_OFFSET,

	NUM_PARAMS
};

enum
{
	OUTPUT_FOO,

	NUM_OUTPUTS
};

class VideoDelay
{
public:

	VideoDelay( VideoInfoStruct* pVideoInfo );
	~VideoDelay( );


	//extern "C"  __declspec(dllexport) /*__stdcall*/ plugMainUnion plugMain(DWORD functionCode, LPVOID pParam, LPVOID instanceID)

	plugMainUnion execFunction( DWORD functionCode, LPVOID pParam );

	DWORD setBufferLength( UINT32 length );
	DWORD setFrameOffset( UINT32 offset );

	DWORD setParameter( DWORD param, double value );

	float getParameter( DWORD param );

	char* getParameterDisplay( DWORD param );

	inline void enterLock( )
	{
		EnterCriticalSection( &CriticalSection );
	}

	inline void leaveLock( )
	{
		LeaveCriticalSection( &CriticalSection );
	}

	DWORD processFrame( IN OUT VideoPixel24bit *pFrame );

private:

	inline char* getParamDisplayBuffer( DWORD param )
	{
		switch( param )
		{
		case PARAM_BUFFER_LENGTH:
			return BufferLengthDisplay;
		case PARAM_FRAME_OFFSET:
			return FrameOffsetDisplay;
		default:
			return NULL;
		}
	}

	VideoInfoStruct VideoInfo;

	UINT32 BufferLength;
	UINT32 FrameOffset;

	char BufferLengthDisplay[ FF_PARAM_DISPLAY_LENGTH ];
	char FrameOffsetDisplay[ FF_PARAM_DISPLAY_LENGTH ];

	FrameBuffer Buffer;

	CRITICAL_SECTION CriticalSection;

};

#endif
