// VideoDelay.cpp : Defines the exported functions for the DLL application.
//

#include "VideoDelay.h"
#include <stdlib.h>
#include <stdio.h>

DWORD VideoDelay::init( VideoInfoStruct* pVideoInfo )
{
	VideoInfo.frameWidth		= pVideoInfo->frameWidth;
	VideoInfo.frameHeight	= pVideoInfo->frameHeight;
	VideoInfo.bitDepth		= pVideoInfo->bitDepth;
	FrameSize = getBitsForDepth( pVideoInfo->bitDepth );

	// this shouldn't happen if the host is checking the capabilities properly
	if (VideoInfo.bitDepth > FF_CAP_32BITVIDEO ||
		VideoInfo.bitDepth < FF_CAP_16BITVIDEO)
	{
		return FALSE;
	}

	InitializeCriticalSection(&CriticalSection);

	FrameOffset = 0;
	FrameBuffer = NULL;
	Cursor = 0;

	setBufferLength( DEFAULT_BUFFER_LENGTH );
	setFrameOffset( DEFAULT_FRAME_OFFSET );

	return TRUE;
}

void VideoDelay::dispose( )
{
	resizeFrameBuffer( 0 );
	DeleteCriticalSection(&CriticalSection);
}

inline DWORD VideoDelay::setBufferLength( UINT32 length )
{
	if( length < 1 )
		length = 1;
	return resizeFrameBuffer( length );
}

DWORD VideoDelay::setFrameOffset( UINT32 offset )
{
	if( offset > BufferLength )
		offset = BufferLength - 1;
	//TODO: implement this
	return NULL;
}

DWORD VideoDelay::setParameter( DWORD param, double value )
{
	switch( param )
	{
	case PARAM_BUFFER_LENGTH:
		return setBufferLength( (UINT32)value );
	case PARAM_FRAME_OFFSET:
		return setFrameOffset( (UINT32)value );
	}
	return FF_FAIL;
}

float VideoDelay::getParameter( DWORD param )
{

	switch( param )
	{
	case PARAM_BUFFER_LENGTH:	return (float)BufferLength;
	case PARAM_FRAME_OFFSET:	return (float)FrameOffset;
	default:					return NULL;
	}
}

char* VideoDelay::getParameterDisplay( DWORD param )
{
	char displayValue[FF_PARAM_DISPLAY_LENGTH];
	memset( displayValue, ' ', FF_PARAM_DISPLAY_LENGTH );
	switch( param )
	{
	case PARAM_BUFFER_LENGTH:
		sprintf_s( displayValue, FF_PARAM_DISPLAY_LENGTH, "%i", BufferLength );
		break;
	case PARAM_FRAME_OFFSET:
		sprintf_s( displayValue, FF_PARAM_DISPLAY_LENGTH, "%i", FrameOffset );
		break;
	}
	return displayValue;
}

DWORD VideoDelay::resizeFrameBuffer( UINT32 bufferLength )
{
	//UINT32 oldLength = getBufferLength( pInstance );
	//UINT32 oldOffset = getFrameOffset( pInstance );

	////....
	//pInstance->FrameBuffer = (VideoPixel24bit*)realloc(
	//	pInstance->FrameBuffer, pInstance->FrameSize * bufferLength );

	//TODO: implement this
	return FF_FAIL;
}

bool VideoDelay::pushFrame( IN VideoPixel24bit* pFrame )
{
	//TODO: implement this
	return false;
}

bool VideoDelay::getNextFrame( OUT VideoPixel24bit* pFrameOutput )
{
	//TODO: implement this
	return false;
}

DWORD VideoDelay::processFrame( IN OUT VideoPixel24bit* pFrame )
{
	DWORD result;

	enterLock();

	if(!pushFrame( pFrame ))
		result = FF_FAIL;
	else if(!getNextFrame( pFrame ))
		result = FF_FAIL;
	else
		result = FF_SUCCESS;

	leaveLock();

	return result;
}


