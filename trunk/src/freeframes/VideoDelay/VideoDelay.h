#ifndef __VIDEO_DELAY_H__
#define __VIDEO_DELAY_H__

//struct VideoDelayInstanceTag;
//#define PLUGIN_INSTANCE_POINTER (VideoDelayInstanceTag*)
//typedef VideoDelayInstanceTag* PLUGIN_INSTANCE_POINTER;

#include <FreeFrame.h>
#include <FreeFrameExt.h>
#include <FreeFrameImpl.h>

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

	DWORD init( VideoInfoStruct* pVideoInfo );
	void dispose( );

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

	DWORD resizeFrameBuffer( UINT32 bufferLength );

	bool pushFrame( IN VideoPixel24bit *pFrame );

	bool getNextFrame( OUT VideoPixel24bit *pFrameOutput );

	VideoInfoStruct VideoInfo;
	DWORD FrameSize;

	UINT32 BufferLength;
	UINT32 FrameOffset;

	UINT32 Cursor;

	VideoPixel24bit *FrameBuffer;

	CRITICAL_SECTION CriticalSection;

};

#endif
