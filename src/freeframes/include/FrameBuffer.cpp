// FrameBuffer.cpp - circular frame buffer

#include "FrameBuffer.h"

#include <stdlib.h>
#include <stdio.h>

#define NOT_IMPLEMENTED()	(NULL)

const UINT32 VIDPIX24_SIZE = sizeof( VideoPixel24bit );

FrameBuffer::FrameBuffer( UINT32 fPixels, UINT32 len )
	: framePixels(fPixels),
	  frameBytes(fPixels * VIDPIX24_SIZE),
	  start(0),
	  length(0),
	  buffer(NULL)
{
	resize( len );
}

FrameBuffer::~FrameBuffer( )
{
	resize( 0 );
}

bool FrameBuffer::resize( UINT32 len )
{
	start = 0;
	length = len;
	if( buffer != NULL )
	{
		free( buffer );
		buffer = NULL;
	}
	if( len > 0 )
	{
		buffer = (VideoPixel24bit*)calloc( len, frameBytes );
		if( buffer == NULL )
		{
			fprintf( stderr, "Unable to allocate frame buffer");
			return false;
		}
	}
	return true;
}

inline UINT32 FrameBuffer::getPixelOffset( UINT32 frameIndex )
{
	if( length == 0 || frameIndex >= length )
		return 0;
	UINT32 frameOffset = (start + frameIndex) % length;
	return frameOffset * frameBytes;
}

bool FrameBuffer::pushFrame( VideoPixel24bit* frameIn )
{
	if( buffer == NULL || length == 0 || frameIn == NULL )
		return false;
	VideoPixel24bit *front = getFrame( 0 );
	memcpy( front, frameIn, frameBytes );
	if( start >= length+1 )
		start = 0;
	else
		start++;
	return true;
}

VideoPixel24bit* FrameBuffer::getFrame( UINT32 frameIndex )
{
	if( buffer == NULL || length == 0 || frameIndex >= length )
		return NULL;
	UINT32 offset = getPixelOffset( frameIndex );
	return buffer + offset;
}

bool FrameBuffer::getFrameCopy( UINT32 frameIndex, VideoPixel24bit *frameOut )
{
	VideoPixel24bit* bufferedFrame = getFrame( frameIndex );
	if( bufferedFrame == NULL )
		return false;
	memcpy( frameOut, bufferedFrame, frameBytes );
	return true;
}


