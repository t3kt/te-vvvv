#ifndef __VIDEO_DELAY_H__
#define __VIDEO_DELAY_H__

//struct VideoDelayInstanceTag;
//#define PLUGIN_INSTANCE_POINTER (VideoDelayInstanceTag*)
//typedef VideoDelayInstanceTag* PLUGIN_INSTANCE_POINTER;

#include <FreeFrame.h>
#include <FreeFrameExt.h>
#include <FreeFrameImpl.h>

#define VIDEODELAY_API __declspec(dllexport)

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

typedef struct VideoDelayInstanceTag {
	//unsigned int bufferLength;
	//unsigned int frameOffset;

	ParamStruct Params[NUM_PARAMS];
	OutputStruct Outputs[NUM_OUTPUTS];

	VideoInfoStruct VideoInfo;

	CRITICAL_SECTION CriticalSection;
} VideoDelayInstance;

#endif
