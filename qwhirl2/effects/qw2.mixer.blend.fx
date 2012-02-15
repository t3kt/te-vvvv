/*

% Scene blending effect for qwhirl2

Includes modified version of some code from VVVV's "Blend.fx" effect.

*/

//Normal	Add	Subtract	Screen	Multiply	Darken	Lighten	Difference	Exclusion	Overlay
//HardLight	SoftLight	Dodge	Burn	Reflect	Glow	Freeze	Heat
// .. there are 18

//float4x4 WorldViewProj : WorldViewProjection;

float2 R; // texture dimensions... of larger texture
float Opacity1 <float uimin=0.0; float uimax=1.0;> = 1.0;
float Opacity2 <float uimin=0.0; float uimax=1.0;> = 1.0;
texture tex0, tex1, tex2;

sampler s0 = sampler_state {
	Texture=(tex0);
	MipFilter=LINEAR;
	MinFilter=LINEAR;
	MagFilter=LINEAR;
};
sampler s1 = sampler_state {
	Texture=(tex1);
	MipFilter=LINEAR;
	MinFilter=LINEAR;
	MagFilter=LINEAR;
};
sampler s2 = sampler_state {
	Texture=(tex2);
	MipFilter=LINEAR;
	MinFilter=LINEAR;
	MagFilter=LINEAR;
};

#define bld(op, c0, c1) float4(lerp((c0*c0.a+c1*c1.a*(1-c0.a))/saturate(c0.a+c1.a*(1-c0.a)),saturate(op),c0.a*c1.a).rgb,saturate(c0.a+c1.a*(1-c0.a)))

/*float4 zz__bld(float4 op, float4 c0, float4 c1) {
	float3 rgbcolor;
	float alpha;
	rgbcolor = lerp((c0*c0.a+c1*c1.a*(1-c0.a))/saturate(c0.a+c1.a*(1-c0.a)),saturate(op),c0.a*c1.a);
	alpha = saturate(c0.a+c1.a*(1-c0.a));
	return float4( rgbcolor.rgb, alpha );
}*/

#define applyOpacity1(c) float4(c * float4(1,1,1,Opacity1))
#define applyOpacity2(c) float4(c * float4(1,1,1,Opacity2))

float4 calcNormal(float4 c0, float4 c1) : color		{	return bld(c1, c1, c0);	}
float4 calcAdd(float4 c0, float4 c1) : color		{		return bld(c0+c1,c0,c1);	}
float4 calcSubtract(float4 c0, float4 c1) : color	{	return bld(c0-c1,c0,c1);	}
float4 calcScreen(float4 c0, float4 c1) : color		{	return bld(c0+c1*saturate(1-c0),c0,c1);	}
float4 calcMultiply(float4 c0, float4 c1) : color	{	return bld(c0*c1,c0,c1);	}
float4 calcDarken(float4 c0, float4 c1) : color		{	return bld(min(c0,c1),c0,c1);	}
float4 calcLighten(float4 c0, float4 c1) : color	{	return bld(max(c0,c1),c0,c1);	}
float4 calcDifference(float4 c0, float4 c1) : color	{	return bld(abs(c0-c1),c0,c1);	}
float4 calcExclusion(float4 c0, float4 c1) : color	{	return bld(c0+c1-2*c0*c1,c0,c1);	}
float4 calcOverlay(float4 c0, float4 c1) : color	{	return bld((c0<.5)?(2*c0*c1):1-2*(1-c0)*(1-c1),c0,c1);	}
float4 calcHardLight(float4 c0, float4 c1) : color	{	return bld((c1<.5)?(2*c0*c1):1-2*(1-c0)*(1-c1),c0,c1);	}
float4 calcSoftLight(float4 c0, float4 c1) : color	{	return bld(2*c0*c1+c0*c0-2*c0*c0*c1,c0,c1);	}
float4 calcDodge(float4 c0, float4 c1) : color		{	return bld((c1==1)?1:c0/(1-c1),c0,c1);	}
float4 calcBurn(float4 c0, float4 c1) : color		{	return bld((c1==0)?0:1-(1-c0)/c1,c0,c1);	}
float4 calcReflect(float4 c0, float4 c1) : color	{	return bld((c1==1)?1:c0*c0/(1-c1),c0,c1);	}
float4 calcGlow(float4 c0, float4 c1) : color		{	return bld((c0==1)?1:c1*c1/(1-c0),c0,c1);	}
float4 calcFreeze(float4 c0, float4 c1) : color		{	return bld((c1==0)?0:1-pow(1-c0,2)/c1,c0,c1);	}
float4 calcHeat(float4 c0, float4 c1) : color {	return bld((c0==0)?0:1-pow(1-c1,2)/c0,c0,c1);	}


/*float4 processAddSubtract(float2 pos : TEXCOORD) : color {
	float4 c0 = tex2D(s0, pos);
	float4 c1 = applyOpacity1(tex2D(s1, pos));
	float4 c2 = applyOpacity2(tex2D(s2, pos));
	float4 c0and1 = calcAdd(c0, c1);
	return calcSubtract(c0and1, c2);
}*/

/*#define process(op1, op2, pos) \
	{ \
		float4 c0 = tex2D(s0, pos);\
		float4 c1 = applyOpacity1(tex2D(s1, pos));\
		float4 c2 = applyOpacity2(tex2D(s2, pos));\
		float4 c0and1 = op1(c0, c1);\
		return op2(c0and1, c2);\
	}*/
#define process(op1, op2, pos)\
	float4(\
		op2(\
			op1(\
				tex2D(s0, pos),\
				applyOpacity1(\
					tex2D(s1, pos)\
				)\
			),\
			applyOpacity2(\
				tex2D(s2, pos)\
			)\
		)\
	)

/*float4 zz__processAddSubtract(float2 pos : TEXCOORD) : color {
	return process(calcAdd, calcSubtract, pos);
}*/
#define defProc(op1, op2)  float4 process##op1##op2(float2 pos : TEXCOORD) : color { return process(calc##op1, calc##op2, pos); }

#define runForAllModes(macro)	\
	macro(Normal)\
	macro(Screen)\
	macro(Multiply)\
	macro(Add)\
	macro(Subtract)\
	macro(Darken)\
	macro(Lighten)\
	macro(Difference)\
	macro(Exclusion)\
	macro(Overlay)\
	macro(HardLight)\
	macro(SoftLight)\
	macro(Dodge)\
	macro(Burn)\
	macro(Reflect)\
	macro(Glow)\
	macro(Freeze)\
	macro(Heat)

#define defProcSet(op1)	\
	defProc(op1, Normal)\
	defProc(op1, Screen)\
	defProc(op1, Multiply)\
	defProc(op1, Add)\
	defProc(op1, Subtract)\
	defProc(op1, Darken)\
	defProc(op1, Lighten)\
	defProc(op1, Difference)\
	defProc(op1, Exclusion)\
	defProc(op1, Overlay)\
	defProc(op1, HardLight)\
	defProc(op1, SoftLight)\
	defProc(op1, Dodge)\
	defProc(op1, Burn)\
	defProc(op1, Reflect)\
	defProc(op1, Glow)\
	defProc(op1, Freeze)\
	defProc(op1, Heat)

//defProc(Add, Subtract)
//defProc(Add, Add)
//...
//defProcSet(Normal)	defProcSet(Add)	defProcSet(Subtract)	defProcSet(Screen)	defProcSet(Multiply)	defProcSet(Darken)	defProcSet(Lighten)	defProcSet(Difference)	defProcSet(Exclusion)	defProcSet(Overlay)	defProcSet(HardLight)	defProcSet(SoftLight)	defProcSet(Dodge)	defProcSet(Burn)	defProcSet(Reflect)	defProcSet(Glow)	defProcSet(Freeze)	defProcSet(Heat)
//runForAllModes(defProcSet)

void vs2d(inout float4 vp:POSITION0,inout float2 uv:TEXCOORD0)
{
	vp.xy*=2;
	uv+=.5/R;
}


#define defTechnique(op1, op2)	technique op1##_##op2 {\
		pass p0 {\
			vertexshader=compile vs_2_0 vs2d();\
			pixelshader=compile ps_2_0 process##op1##op2();\
		}\
	}
#define defTechniqueSet(op1)	\
	defTechnique(op1, Normal)\
	defTechnique(op1, Screen)\
	defTechnique(op1, Multiply)\
	defTechnique(op1, Add)\
	defTechnique(op1, Subtract)\
	defTechnique(op1, Darken)\
	defTechnique(op1, Lighten)\
	defTechnique(op1, Difference)\
	defTechnique(op1, Exclusion)\
	defTechnique(op1, Overlay)\
	defTechnique(op1, HardLight)\
	defTechnique(op1, SoftLight)\
	defTechnique(op1, Dodge)\
	defTechnique(op1, Burn)\
	defTechnique(op1, Reflect)\
	defTechnique(op1, Glow)\
	defTechnique(op1, Freeze)\
	defTechnique(op1, Heat)
#define defProcAndTechniqueSet(op1)	\
	defProcAndTechnique(op1, Normal)\
	defProcAndTechnique(op1, Screen)\
	defProcAndTechnique(op1, Multiply)\
	defProcAndTechnique(op1, Add)\
	defProcAndTechnique(op1, Subtract)\
	defProcAndTechnique(op1, Darken)\
	defProcAndTechnique(op1, Lighten)\
	defProcAndTechnique(op1, Difference)\
	defProcAndTechnique(op1, Exclusion)\
	defProcAndTechnique(op1, Overlay)\
	defProcAndTechnique(op1, HardLight)\
	defProcAndTechnique(op1, SoftLight)\
	defProcAndTechnique(op1, Dodge)\
	defProcAndTechnique(op1, Burn)\
	defProcAndTechnique(op1, Reflect)\
	defProcAndTechnique(op1, Glow)\
	defProcAndTechnique(op1, Freeze)\
	defProcAndTechnique(op1, Heat)

#define defProcAndTechnique(op1, op2)\
	defProc(op1, op2)\
	defTechnique(op1, op2)

//runForAllModes(defTechniqueSet)
runForAllModes(defProcAndTechniqueSet)


/*float4 dbgPassThruPS(float2 pos : TEXCOORD) : color { return tex2D(s0, pos); }
technique dbgPassThru {
	pass p0 {
		vertexshader=compile vs_2_0 vs2d();
		pixelshader=compile ps_2_0 dbgPassThruPS();
	}
}*/
/*
float4 pNORMAL(float2 x):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c1,c1,c0);
    return c;
}
float4 pADD(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c0+c1,c0,c1);
    return c;
}
float4 pSUBTRACT(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c0-c1,c0,c1);
    return c;
}
float4 pSCREEN(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c0+c1*saturate(1-c0),c0,c1);
    return c;
}
float4 pMUL(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c0*c1,c0,c1);
    return c;
}
float4 pDARKEN(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(min(c0,c1),c0,c1);
    return c;
}
float4 pLIGHTEN(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(max(c0,c1),c0,c1);
    return c;
}
float4 pDIFFERENCE(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(abs(c0-c1),c0,c1);
    return c;
}
float4 pEXCLUSION(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(c0+c1-2*c0*c1,c0,c1);
    return c;
}
float4 pOVERLAY(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c0<.5)?(2*c0*c1):1-2*(1-c0)*(1-c1),c0,c1);
    return c;
}
float4 pHARDLIGHT(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c1<.5)?(2*c0*c1):1-2*(1-c0)*(1-c1),c0,c1);
    return c;
}
float4 pSOFTLIGHT(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld(2*c0*c1+c0*c0-2*c0*c0*c1,c0,c1);
    return c;
}
float4 pDODGE(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c1==1)?1:c0/(1-c1),c0,c1);
    return c;
}
float4 pBURN(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c1==0)?0:1-(1-c0)/c1,c0,c1);
    return c;
}
float4 pREFLECT(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c1==1)?1:c0*c0/(1-c1),c0,c1);
    return c;
}
float4 pGLOW(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c0==1)?1:c1*c1/(1-c0),c0,c1);
    return c;
}
float4 pFREEZE(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c1==0)?0:1-pow(1-c0,2)/c1,c0,c1);
    return c;
}
float4 pHEAT(float2 x:TEXCOORD0):color
{
	float4 c0=tex2D(s0,x);
	float4 c1=tex2D(s1,x)*float4(1,1,1,Opacity);
    float4 c=bld((c0==0)?0:1-pow(1-c1,2)/c0,c0,c1);
    return c;
}*/

