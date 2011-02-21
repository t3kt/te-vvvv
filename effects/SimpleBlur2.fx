// this is a very basic template. use it to start writing your own effects.
// if you want effects with lighting start from one of the GouraudXXXX or PhongXXXX effects

// --------------------------------------------------------------------------------------------------
// PARAMETERS:
// --------------------------------------------------------------------------------------------------

//transforms
float4x4 tW: WORLD;        //the models world matrix
float4x4 tV: VIEW;         //view matrix as set via Renderer (EX9)
float4x4 tP: PROJECTION;
float4x4 tWVP: WORLDVIEWPROJECTION;

float Angle : register(C0);  // Defines the blurring direction
float BlurAmount : register(C1);  // Defines the blurring magnitude

//texture
texture Tex <string uiname="Texture";>;
sampler Samp = sampler_state    //sampler for doing the texture-lookup
{
    Texture   = (Tex);          //apply a texture to the sampler
    MipFilter = LINEAR;         //sampler states
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

//texture transformation marked with semantic TEXTUREMATRIX to achieve symmetric transformations
float3x3 tTex: TEXTUREMATRIX <string uiname="Texture Transform";>;

//the data structure: "vertexshader to pixelshader"
//used as output data with the VS function
//and as input data with the PS function
struct vs2ps
{
    float4 Pos : POSITION;
    float2 TexCd : TEXCOORD0;
};

// --------------------------------------------------------------------------------------------------
// VERTEXSHADERS
// --------------------------------------------------------------------------------------------------

vs2ps VS(
    float4 Pos : POSITION,
    float2 TexCd : TEXCOORD0)
{
    //inititalize all fields of output struct with 0
    vs2ps Out = (vs2ps)0;

    //transform position
    Out.Pos = mul(Pos, tWVP);
    
    //transform texturecoordinates
    Out.TexCd = mul(float3(TexCd, 1), tTex);

    return Out;
}

// --------------------------------------------------------------------------------------------------
// PIXELSHADERS:
// --------------------------------------------------------------------------------------------------

float4 PSBlurInternal(float2 uv, float angle, int count, float amt)
{
	float4 output = 0;
	float2 offset;
	sincos(angle, offset.y, offset.x);
	offset *= amt;
	for(int i = 0; i < count; i++)
	{
		output += tex2D(Samp, uv - offset * i);
	}
	output /= count;
	return output;
}

float4 PS( float2 uv : TEXCOORD) : COLOR
{
    /*float4 output = 0;  // Defines the output color of a pixel
    float2 offset;  // Defines the blurring direction as a direction vector
    int count = 24;  // Defines the number of blurring iterations

    // First compute a direction vector which defines the direction of blurring. This is
    // done using the sincos instruction and the Angle input parameter, and the result
    // is stored in the variable offset. This vector is of unit length. Multiply this
    // unit vector by BlurAmount to adjust its length to reflect the blurring magnitude.
    sincos(Angle, offset.y, offset.x);
    offset *= BlurAmount;

    // To generate the blurred image, we generate multiple copies of the input image, shifted according
    // to the blurring direction vector, and then sum them all up to get the final image.
    for(int i=0; i<count; i++)
    {
        output += tex2D(Samp, uv - offset * i);
    }
    output /= count; // Normalize the color

    return output;*/
    int count = 24;
    return PSBlurInternal(uv, Angle, count, BlurAmount);
}

float4 PSHorizontal(float2 uv: TEXCOORD) : COLOR
{
	
}

// --------------------------------------------------------------------------------------------------
// TECHNIQUES:
// --------------------------------------------------------------------------------------------------

technique TSimpleShader
{
    pass P0
    {
        //Wrap0 = U;  // useful when mesh is round like a sphere
        VertexShader = compile vs_3_0 VS();
        PixelShader  = compile ps_3_0 PS();
    }
}
