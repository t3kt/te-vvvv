// main.cpp : Defines the entry point for the DLL application.

#include "FreeFrameImpl.h"

//////////////////////////////////////////////////////////////////
// Plugin dll entry point
//////////////////////////////////////////////////////////////////

BOOL APIENTRY DllMain(HANDLE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved)
{
	return TRUE;
}

///////////////////////////////////////////////////////////////////////////////////////
// plugMain - The one and only exposed function
// parameters:
//	functionCode - tells the plugin which function is being called
//  pParam - 32-bit parameter or 32-bit pointer to parameter structure
//
// PLUGIN DEVELOPERS:  you shouldn't need to change this function
//
// All parameters are cast as 32-bit untyped pointers and cast to appropriate
// types here
//
// All return values are cast to 32-bit untyped pointers here before return to
// the host
//

#ifdef WIN32
extern "C"  __declspec(dllexport) /*__stdcall*/ plugMainUnion plugMain(DWORD functionCode, LPVOID pParam, LPVOID instanceID)
#elif LINUX
extern "C" {
   plugMainUnion plugMain( DWORD functionCode, LPVOID pParam, LPVOID instanceID)
#endif
{
	//// declare pPlugObj - pointer to this instance
	//plugClass *pPlugObj;

	//// typecast LPVOID into pointer to a plugClass
	//pPlugObj = (plugClass*) instanceID;

	switch(functionCode) {

	case FF_GETINFO:
		return createRetVal( getInfo() );
	case FF_INITIALISE:
		return createRetVal( initialise() );
	case FF_DEINITIALISE:
		return createRetVal( deInitialise() );			// todo: pass on instance IDs etc
	case FF_GETNUMPARAMETERS:
		return createRetVal( getNumParameters() );
	case FF_GETPARAMETERNAME:
		return createRetVal( getParameterName( (DWORD) pParam ) );
	case FF_GETPARAMETERDEFAULT:
		return createRetVal( getParameterDefault( (DWORD) pParam ) );
	case FF_GETPARAMETERDISPLAY:
		return createRetVal( getParameterDisplay( instanceID, (DWORD) pParam ) );
	// parameters are passed in here as a packed struct of two DWORDS:
	// index and value
	case FF_SETPARAMETER:
		return createRetVal( setParameter( instanceID, (SetParameterStruct*) pParam ) );
	case FF_PROCESSFRAME:
		return createRetVal( processFrame( instanceID, pParam ) );
	case FF_GETPARAMETER:
		return createRetVal( getParameter( instanceID, (DWORD) pParam ) );
	case FF_GETPLUGINCAPS:
		return createRetVal( getPluginCaps( (DWORD) pParam) );

// Russell - FF 1.0 upgrade in progress ...

	case FF_INSTANTIATE:
		return createRetVal( instantiate( (VideoInfoStruct*) pParam ));
	case FF_DEINSTANTIATE:
		return createRetVal(deInstantiate(instanceID));
	case FF_GETEXTENDEDINFO:
		return createRetVal(getExtendedInfo());
	case FF_PROCESSFRAMECOPY:
		return createRetVal( processFrameCopy( instanceID, (ProcessFrameCopyStruct*)pParam) );
	case FF_GETPARAMETERTYPE:
		return createRetVal( getParameterType( (DWORD) pParam ) );

//freeframe 1.0 extended. see: http://vvvv.org/tiki-index.php?page=FreeFrameExtendedSpecification
//outputs
	case FF_GETNUMOUTPUTS:
		return createRetVal( getNumOutputs() );
	case FF_GETOUTPUTNAME:
		return createRetVal( getOutputName((DWORD) pParam) );
	case FF_GETOUTPUTTYPE:
		return createRetVal( getOutputType((DWORD) pParam) );
	case FF_GETOUTPUTSLICECOUNT:
		return createRetVal( getOutputSliceCount(instanceID, (DWORD) pParam) );
	case FF_GETOUTPUT:
		//retval.svalue = (char*)pPlugObj->getOutput((DWORD) pParam);
		return createRetVal( getOutput( instanceID, (DWORD) pParam) );
	case FF_SETTHREADLOCK:
		 return createRetVal( setThreadLock(instanceID, (DWORD) pParam) );

//spreaded inputs
	case FF_SETINPUT:
		return createRetVal( setInput(instanceID,  (InputStruct*) pParam ) );

// ....................................

	default:
		return createRetVal( FF_FAIL );
	}
}
#ifdef linux

} /* extern "C" */
#endif

// }
