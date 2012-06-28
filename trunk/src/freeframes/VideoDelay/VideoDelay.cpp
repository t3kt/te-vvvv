// VideoDelay.cpp : Defines the exported functions for the DLL application.
//

#include "VideoDelay.h"
#include <stdlib.h>
#include <stdio.h>

VideoDelay::VideoDelay( VideoInfoStruct* pVideoInfo )
	: Buffer( pVideoInfo->frameWidth * pVideoInfo->frameHeight, DEFAULT_BUFFER_LENGTH ),
	  BufferLength( DEFAULT_BUFFER_LENGTH )
{
	VideoInfo.frameWidth		= pVideoInfo->frameWidth;
	VideoInfo.frameHeight	= pVideoInfo->frameHeight;
	VideoInfo.bitDepth		= pVideoInfo->bitDepth;

	// this shouldn't happen if the host is checking the capabilities properly
	/*if (VideoInfo.bitDepth > FF_CAP_32BITVIDEO ||
		VideoInfo.bitDepth < FF_CAP_16BITVIDEO)
	{
		return FALSE;
	}*/

	InitializeCriticalSection(&CriticalSection);

	//setBufferLength( DEFAULT_BUFFER_LENGTH );
}

VideoDelay::~VideoDelay( )
{
	DeleteCriticalSection(&CriticalSection);
}

plugMainUnion VideoDelay::execFunction( DWORD functionCode, LPVOID pParam )
{
	plugMainUnion retval;
	SetParameterStruct *pSetParam;

	switch( functionCode )
	{
	case FF_GETPARAMETERDISPLAY:
		return createRetVal( getParameterDisplay( (DWORD) pParam ) );
	case FF_SETPARAMETER:
		pSetParam = (SetParameterStruct*)pParam;
		return createRetVal( setParameter( pSetParam->index, pSetParam->value ) );
	case FF_PROCESSFRAME:
		return createRetVal( processFrame( (VideoPixel24bit*)pParam ) );
	case FF_GETPARAMETER:
		return createRetVal( getParameter( (DWORD) pParam ) );
		
	// NOT IMPLEMENTED
	default:
		return createRetVal( FF_FAIL );
	}
}

inline DWORD VideoDelay::setBufferLength( UINT32 length )
{
	if( length < 1 )
		length = 1;
	if( Buffer.resize( length ) )
		return FF_SUCCESS;
	return FF_FAIL;
}

DWORD VideoDelay::setFrameOffset( UINT32 offset )
{
	if( offset > BufferLength )
		offset = BufferLength - 1;
	return FF_SUCCESS;
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
	char *displayValue = getParamDisplayBuffer( param );
	if( displayValue == NULL )
		return NULL;
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

DWORD VideoDelay::processFrame( IN OUT VideoPixel24bit* pFrame )
{
	DWORD result;

	enterLock();

	if( !Buffer.pushFrame( pFrame ) )
		result = FF_FAIL;
	else if( !Buffer.getFrameCopy( FrameOffset, pFrame ) )
		result = FF_FAIL;
	else
		result = FF_SUCCESS;

	leaveLock();

	return result;
}


