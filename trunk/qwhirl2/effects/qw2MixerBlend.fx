/*

% Scene blending effect for qwhirl2

Includes modified version of some code from VVVV's "Blend.fx" effect.

*/

#include "blending.fxh"

//Normal	Add	Subtract	Screen	Multiply	Darken	Lighten	Difference	Exclusion	Overlay
//HardLight	SoftLight	Dodge	Burn	Reflect	Glow	Freeze	Heat
// .. there are 18


float4x4 WorldViewProj : WorldViewProjection;

float2 R; // texture dimensions... of larger texture
float OpacityA <float uimin=0.0; float uimax=1.0;> = 1.0;
float OpacityB <float uimin=0.0; float uimax=1.0;> = 1.0;
int ModeA <int uimin=0; int uimax = MAX_MODE;> = 0;
int ModeB <int uimin=0; int uimax = MAX_MODE;> = 0;
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

#define applyOpacity(c, opacity) float4(c * float4(1,1,1,opacity))

/*#define z__applyMix2(mode, opacity, pos)\
	float4(\
		op2(\
			op1(\
				tex2D(s0, pos),\
				applyOpacity(\
					tex2D(s1, pos),\
					opacity\
				)\
			),\
			applyOpacity(\
				tex2D(s2, pos)\
			)\
		)\
	)
*/

float4 applyMix2(sampler sA, sampler sB, float2 pos) {
	return calcWithMode(tex2D(sA, pos), applyOpacity(tex2D(sB, pos), OpacityA), ModeA);
}
float4 applyMix3(sampler sA, sampler sB, sampler sC, float2 pos) {
	float4 mixAB;
	mixAB = calcWithMode(tex2D(sA, pos), applyOpacity(tex2D(sB, pos), OpacityA), ModeA);
	return calcWithMode(mixAB, applyOpacity(tex2D(sC, pos), OpacityB), ModeB);
}

#define ps_name_single(a)		ps_single_##a
#define ps_name_mix_2(a, b)		ps_mix_##a##_##b
#define ps_name_mix_3(a, b, c)	ps_mix_##a##_##b##_##c

#define def_ps_single(a)\
	float4 ps_name_single(a)(float2 pos: TEXCOORD) : color {\
		return tex2D(s##a, pos);\
	}

#define def_ps_mix_2(a, b)\
	float4 ps_name_mix_2(a, b)(float2 pos: TEXCOORD) : color {\
		return applyMix2(s##a, s##b, pos);\
	}
#define def_ps_mix_3(a, b, c)\
	float4 ps_name_mix_3(a, b, c)(float2 pos: TEXCOORD) : color {\
		return applyMix3(s##a, s##b, s##c, pos);\
	}

def_ps_single(0)
def_ps_single(1)
def_ps_single(2)

def_ps_mix_2(0, 1)
def_ps_mix_2(0, 2)
def_ps_mix_2(1, 0)
def_ps_mix_2(1, 2)
def_ps_mix_2(2, 0)
def_ps_mix_2(2, 1)

def_ps_mix_3(0, 1, 2)
def_ps_mix_3(0, 2, 1)
def_ps_mix_3(1, 0, 2)
def_ps_mix_3(1, 2, 0)
def_ps_mix_3(2, 0, 1)
def_ps_mix_3(2, 1, 0)

float4 mainVS(float3 pos : POSITION) : POSITION{
	return mul(float4(pos.xyz, 1.0), WorldViewProj);
}

float4 mainPS() : COLOR {
	return float4(1.0, 1.0, 1.0, 1.0);
}

#define def_technique(name, ps)	technique name {\
		pass p0 {\
			vertexshader=compile vs_2_0 mainVS();\
			pixelshader=compile ps_2_0 ps();\
		}\
	}
/*
#define def_techique_single(a)\
	technique single_##a {\
		pass p0 {\
			VertexShader = compile vs_2_0 mainVS();\
			PixelShader = compile ps_2_0 ps_name_single(a)();\
		}\
	}
*/
//def_technique_single(0)

/*technique single_0 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_single_0();
	}
}*/
def_technique(single_0, ps_name_single(0))

technique single_1 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_single_1();
	}
}
technique single_2 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_single_2();
	}
}

technique mix_0_1 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(0, 1)();
	}
}

technique mix_0_2 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(0, 2)();
	}
}

technique mix_1_0 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(1, 0)();
	}
}

technique mix_1_2 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(1, 2)();
	}
}

technique mix_2_0 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(2, 0)();
	}
}

technique mix_2_1 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_2(2, 1)();
	}
}

technique mix_0_1_2 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(0, 1, 2)();
	}
}

technique mix_0_2_1 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(0, 2, 1)();
	}
}

technique mix_1_0_2 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(1, 0, 2)();
	}
}

technique mix_1_2_0 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(1, 2, 0)();
	}
}

technique mix_2_0_1 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(2, 0, 1)();
	}
}

technique mix_2_1_0 {
	pass p0 {
		VertexShader = compile vs_2_0 mainVS();
		PixelShader = compile ps_2_0 ps_name_mix_3(2, 1, 0)();
	}
}

//technique technique0 {
//	pass p0 {
//		CullMode = None;
//		VertexShader = compile vs_3_0 mainVS();
//		PixelShader = compile ps_3_0 mainPS();
//	}
//}
