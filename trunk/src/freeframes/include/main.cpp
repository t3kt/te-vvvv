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
	plugMainUnion retval;

	//// declare pPlugObj - pointer to this instance
	//plugClass *pPlugObj;

	//// typecast LPVOID into pointer to a plugClass
	//pPlugObj = (plugClass*) instanceID;

	switch(functionCode) {

	case FF_GETINFO:
		retval.PISvalue = getInfo();
		break;
	case FF_INITIALISE:
		retval.ivalue = initialise();
		break;
	case FF_DEINITIALISE:
		retval.ivalue = deInitialise();			// todo: pass on instance IDs etc
		break;
	case FF_GETNUMPARAMETERS:
		retval.ivalue = getNumParameters();
		break;
	case FF_GETPARAMETERNAME:
		retval.svalue =  getParameterName( (DWORD) pParam );
		break;
	case FF_GETPARAMETERDEFAULT:
		retval.fvalue =  getParameterDefault( (DWORD) pParam );
		break;
	case FF_GETPARAMETERDISPLAY:
		retval.svalue = getParameterDisplay( instanceID, (DWORD) pParam );
		break;
	// parameters are passed in here as a packed struct of two DWORDS:
	// index and value
	case FF_SETPARAMETER:
		retval.ivalue = setParameter( instanceID, (SetParameterStruct*) pParam );
		break;
	case FF_PROCESSFRAME:
		retval.ivalue = processFrame( instanceID, pParam );
		break;
	case FF_GETPARAMETER:
		retval.fvalue = getParameter( instanceID, (DWORD) pParam );
		break;
	case FF_GETPLUGINCAPS:
		retval.ivalue = getPluginCaps( (DWORD) pParam);
		break;

// Russell - FF 1.0 upgrade in progress ...

	case FF_INSTANTIATE:
		retval.ivalue = (DWORD) instantiate( (VideoInfoStruct*) pParam);
		break;
	case FF_DEINSTANTIATE:
		retval.ivalue = deInstantiate(instanceID);
		break;
	case FF_GETEXTENDEDINFO:
		retval.ivalue = (DWORD) getExtendedInfo();
		break;
	case FF_PROCESSFRAMECOPY:
		retval.ivalue = processFrameCopy( instanceID, (ProcessFrameCopyStruct*)pParam);
		break;
	case FF_GETPARAMETERTYPE:
		retval.ivalue = getParameterType( (DWORD) pParam );
		break;

//freeframe 1.0 extended. see: http://vvvv.org/tiki-index.php?page=FreeFrameExtendedSpecification
//outputs
	case FF_GETNUMOUTPUTS:
		retval.ivalue = getNumOutputs();
		break;
	case FF_GETOUTPUTNAME:
		retval.svalue = getOutputName((DWORD) pParam);
		break;
	case FF_GETOUTPUTTYPE:
		retval.ivalue = getOutputType((DWORD) pParam);
		break;
	case FF_GETOUTPUTSLICECOUNT:
		retval.ivalue = getOutputSliceCount(instanceID, (DWORD) pParam);
		break;
	case FF_GETOUTPUT:
		//retval.svalue = (char*)pPlugObj->getOutput((DWORD) pParam);
		retval.svalue = (char*)getOutput( instanceID, (DWORD) pParam);
		break;
	case FF_SETTHREADLOCK:
		 retval.ivalue = setThreadLock(instanceID, (DWORD) pParam);
		 break;

//spreaded inputs
	case FF_SETINPUT:
		retval.ivalue =  setInput(instanceID,  (InputStruct*) pParam );
		break;

// ....................................

	default:
		retval.ivalue = FF_FAIL;
		break;
	}
	return retval;
}
#ifdef linux

} /* extern "C" */
#endif

// }
