// FrameBuffer.h - circular frame buffer
#ifndef __FRAME_BUFFER_H__
#define __FRAME_BUFFER_H__

#include <FreeFrame.h>
#include <FreeFrameExt.h>

class FrameBuffer
{
public:
	FrameBuffer( UINT32 fPixels, UINT32 len );
	~FrameBuffer( );

	bool resize( UINT32 len );
	bool pushFrame( VideoPixel24bit *frameIn );
	VideoPixel24bit* getFrame( UINT32 index );
	bool getFrameCopy( UINT32 index, VideoPixel24bit *frameOut );

private:
	UINT32 getPixelOffset( UINT32 frameIndex );

	UINT32 framePixels;
	UINT32 frameBytes;
	UINT32 start;
	UINT32 length;
	VideoPixel24bit* buffer;
};

#endif
