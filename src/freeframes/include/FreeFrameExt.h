// FreeFrameExt.h - FreeFrame (VVVV) Extended 1.0
#ifndef __FREEFRAME_EXT_H__
#define __FREEFRAME_EXT_H__

#include <Windows.h>
#include "FreeFrame.h"



// implementation specific definitions

typedef struct ParamConstsStructTag {
	unsigned int Type;
	float Default;
	char Name[16];
} ParamConstsStruct;

typedef struct ParamStructTag {
	float Value;
	char DisplayValue[16];
} ParamStruct;

typedef struct OutputConstsStructTag {
	unsigned int Type;
	char Name[16];
} OutputConstsStruct;

typedef struct OutputStructTag {
	DWORD SliceCount;
	float* Spread;
} OutputStruct;

typedef struct InputStructTag {
	DWORD Index;
	DWORD SliceCount;
	double* Spread;
} InputStruct;

typedef struct VideoPixel24bitTag {
	BYTE red;
	BYTE green;
	BYTE blue;
} VideoPixel24bit;

#endif
