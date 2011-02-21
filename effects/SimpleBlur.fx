//@author: vvvv group
//@help: this is a very basic template. use it to start writing your own effects. if you want effects with lighting start from one of the GouraudXXXX or PhongXXXX effects
//@tags:
//@credits:

// --------------------------------------------------------------------------------------------------
// PARAMETERS:
// --------------------------------------------------------------------------------------------------

//texture
texture Tex <string uiname="Texture";>;
sampler Samp = sampler_state    //sampler for doing the texture-lookup
{
    Texture   = (Tex);          //apply a texture to the sampler
    MipFilter = LINEAR;         //sampler states
    MinFilter = LINEAR;
    MagFilter = LINEAR;
};

//the data structure: "vertexshader to pixelshader"
//used as output data with the VS function
//and as input data with the PS function
struct vs2ps
{
    float4 Pos  : POSITION;
    float2 TexCd : TEXCOORD0;
};

// --------------------------------------------------------------------------------------------------
// PIXELSHADERS:
// --------------------------------------------------------------------------------------------------

float2 PixelSize;

float4 PSHorizontalBlur(vs2ps In): COLOR
{
    float4 sum = 0;
    int weightSum = 0;
    //the weights of the neighbouring pixels
    int weights[15] = {1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1};
    //we are taking 15 samples
    for (int i = 0; i < 15; i++)
    {
        //7 to the left, self and 7 to the right
        float2 cord = float2(In.TexCd.x + PixelSize.x * (i-7), In.TexCd.y);
        //the samples are weighed according to their relation to the current pixel
        sum += tex2D(Samp, cord) * weights[i];
        //while going through the loop we are summing up the weights
        weightSum += weights[i];
    }
    sum /= weightSum;
    return float4(sum.rgb, 1);
}

float4 PSHorizontalBlur21(vs2ps In): COLOR
{
    float4 sum = 0;
    int weightSum = 0;
    //the weights of the neighbouring pixels
    int weights[21] = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};
    //we are taking 21 samples
    for (int i = 0; i < 21; i++)
    {
        //10 to the left, self and 10 to the right
        float2 cord = float2(In.TexCd.x + PixelSize.x * (i-10), In.TexCd.y);
        //the samples are weighed according to their relation to the current pixel
        sum += tex2D(Samp, cord) * weights[i];
        //while going through the loop we are summing up the weights
        weightSum += weights[i];
    }
    sum /= weightSum;
    return float4(sum.rgb, 1);
}

int VarBlurFactor = 10;

float getDistWeight(int i, int factorOver2)
{
	int dist;
	if(i<factorOver2)
		dist = factorOver2 - i;
	else
		dist = i - factorOver2;
	//return float(dist) *####################################
	return -1;
}

float4 PSHorizontalVarBlur(vs2ps In) : COLOR
{
	float4 sum = 0;
	int weightSum = 0;
	int factorOver2 = VarBlurFactor / 2;
	for(int i = 0; i < VarBlurFactor; i++)
	{
		float2 coord = float2(In.TexCd.x + PixelSize.x * (i-factorOver2), In.TexCd.y);
		//sum += tex2D(Samp, coord) 
		
	}
	return float4(sum.rgb, 1);
}

float4 PSVerticalBlur(vs2ps In): COLOR
{
    float4 sum = 0;
    int weightSum = 0;
    //the weights of the neighbouring pixels
    int weights[15] = {1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1};
    //we are taking 15 samples
    for (int i = 0; i < 15; i++)
    {
        //7 upwards, self and 7 downwards
        float2 cord = float2(In.TexCd.x, In.TexCd.y + PixelSize.y * (i-7));
        //the samples are weighed according to their relation to the current pixel
        sum += tex2D(Samp, cord) * weights[i];
        //while going through the loop we are summing up the weights
        weightSum += weights[i];
    }
    sum /= weightSum;
    return float4(sum.rgb, 1);
}

float4 PSVerticalBlur21(vs2ps In): COLOR
{
    float4 sum = 0;
    int weightSum = 0;
    //the weights of the neighbouring pixels
    int weights[21] = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1};
    //we are taking 21 samples
    for (int i = 0; i < 21; i++)
    {
        //10 upwards, self and 10 downwards
        float2 cord = float2(In.TexCd.x, In.TexCd.y + PixelSize.y * (i-10));
        //the samples are weighed according to their relation to the current pixel
        sum += tex2D(Samp, cord) * weights[i];
        //while going through the loop we are summing up the weights
        weightSum += weights[i];
    }
    sum /= weightSum;
    return float4(sum.rgb, 1);
}

// --------------------------------------------------------------------------------------------------
// TECHNIQUES:
// --------------------------------------------------------------------------------------------------

technique THorizontalBlur
{
    pass P0
    {
        PixelShader  = compile ps_2_0 PSHorizontalBlur();
    }
}
 
technique TVerticalBlur
{
    pass P0
    {
        PixelShader  = compile ps_2_0 PSVerticalBlur();
    }
}

technique THorizontalBlur21
{
    pass P0
    {
        PixelShader  = compile ps_2_0 PSHorizontalBlur21();
    }
}
 
technique TVerticalBlur21
{
    pass P0
    {
        PixelShader  = compile ps_2_0 PSVerticalBlur21();
    }
}
