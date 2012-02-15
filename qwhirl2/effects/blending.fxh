/*

% Core stuff for scene blending effect(s) for qwhirl2

Includes modified version of some code from VVVV's "Blend.fx" effect.

*/

//Normal	Add	Subtract	Screen	Multiply	Darken	Lighten	Difference	Exclusion	Overlay
//HardLight	SoftLight	Dodge	Burn	Reflect	Glow	Freeze	Heat
// .. there are 18

#define MODE_NORMAL		0
#define	MODE_ADD		1
#define	MODE_SUBTRACT	2
#define MODE_SCREEN		3
#define MODE_MULTIPLY	4
#define MODE_DARKEN		5
#define MODE_LIGHTEN	6
#define MODE_DIFFERENCE	7
#define MODE_EXCLUSION	8
#define MODE_OVERLAY	9
#define MODE_HARDLIGHT	10
#define MODE_SOFTLIGHT	11
#define MODE_DODGE		12
#define MODE_BURN		13
#define MODE_REFLECT	14
#define MODE_GLOW		15
#define MODE_FREEZE		16
#define MODE_HEAT		17

#define MAX_MODE		MODE_HEAT

#define NUM_MODES		18

float4 bld(float4 op, float4 c0, float4 c1) {
	float3 rgbcolor;
	float alpha;
	rgbcolor = lerp(
		(c0 * c0.a + c1 * c1.a * (1 - c0.a)) / saturate(c0.a + c1.a * (1 - c0.a)),
		saturate(op),c0.a*c1.a);
	alpha = saturate(c0.a+c1.a*(1-c0.a));
	return float4( rgbcolor.rgb, alpha );
}

//#define bld(op, c0, c1) float4(lerp((c0*c0.a+c1*c1.a*(1-c0.a))/saturate(c0.a+c1.a*(1-c0.a)),saturate(op),c0.a*c1.a).rgb,saturate(c0.a+c1.a*(1-c0.a)))

float4 calcNormal(float4 c0, float4 c1) : color		{	return bld(c1, c1, c0);	}
float4 calcAdd(float4 c0, float4 c1) : color		{	return bld(c0+c1,c0,c1);	}
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
float4 calcHeat(float4 c0, float4 c1) : color		{	return bld((c0==0)?0:1-pow(1-c1,2)/c0,c0,c1);	}

float4 calcWithMode(float4 c0, float4 c1, int mode) {
	switch(mode) {
		case MODE_NORMAL:		return calcNormal(c0, c1);
		case MODE_ADD:			return calcAdd(c0, c1);
		case MODE_SUBTRACT:		return calcSubtract(c0, c1);
		case MODE_SCREEN:		return calcScreen(c0, c1);
		case MODE_MULTIPLY:		return calcMultiply(c0, c1);
		case MODE_DARKEN:		return calcDarken(c0, c1);
		case MODE_LIGHTEN:		return calcLighten(c0, c1);
		case MODE_DIFFERENCE:	return calcDifference(c0, c1);
		case MODE_EXCLUSION:	return calcExclusion(c0, c1);
		case MODE_OVERLAY:		return calcOverlay(c0, c1);
		case MODE_HARDLIGHT:	return calcHardLight(c0, c1);
		case MODE_SOFTLIGHT:	return calcSoftLight(c0, c1);
		case MODE_DODGE:		return calcDodge(c0, c1);
		case MODE_BURN:			return calcBurn(c0, c1);
		case MODE_REFLECT:		return calcReflect(c0, c1);
		case MODE_GLOW:			return calcGlow(c0, c1);
		case MODE_FREEZE:		return calcFreeze(c0, c1);
		case MODE_HEAT:			return calcHeat(c0, c1);
		default:				return c0;
	}
}

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



