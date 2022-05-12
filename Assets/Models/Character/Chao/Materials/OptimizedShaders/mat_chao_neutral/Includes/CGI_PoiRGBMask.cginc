#ifndef POI_RGBMASK
    #define POI_RGBMASK
    #if defined(PROP_RGBMASK) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_RGBMask); float4 _RGBMask_ST;
    #endif
    #if defined(PROP_REDTEXURE) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_RedTexure); float4 _RedTexure_ST;
    #endif
    #if defined(PROP_GREENTEXTURE) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_GreenTexture); float4 _GreenTexture_ST;
    #endif
    #if defined(PROP_BLUETEXTURE) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_BlueTexture); float4 _BlueTexture_ST;
    #endif
    #if defined(PROP_ALPHATEXTURE) || !defined(OPTIMIZER_ENABLED)
        UNITY_DECLARE_TEX2D_NOSAMPLER(_AlphaTexture); float4 _AlphaTexture_ST;
    #endif
    float4 _RedColor;
    float4 _GreenColor;
    float4 _BlueColor;
    float4 _AlphaColor;
    float2 _RGBMaskPanning;
    float2 _RGBRedPanning;
    float2 _RGBGreenPanning;
    float2 _RGBBluePanning;
    float2 _RGBAlphaPanning;
    float _RGBBlendMultiplicative;
    float _RGBMaskUV;
    float _RGBRed_UV;
    float _RGBGreen_UV;
    float _RGBBlue_UV;
    float _RGBAlpha_UV;
    float _RGBUseVertexColors;
    float _RGBNormalBlend;
    static float4 rgbMask;
    void calculateRGBNormals(inout half3 mainTangentSpaceNormal)
    {
    }
    float3 calculateRGBMask(float3 baseColor)
    {
        #ifndef RGB_MASK_TEXTURE
            #define RGB_MASK_TEXTURE
            
            if (float(0))
            {
                rgbMask = poiMesh.vertexColor;
            }
            else
            {
                #if defined(PROP_RGBMASK) || !defined(OPTIMIZER_ENABLED)
                    rgbMask = POI2D_SAMPLER_PAN(_RGBMask, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
                #else
                    rgbMask = 1;
                #endif
            }
        #endif
        #if defined(PROP_REDTEXURE) || !defined(OPTIMIZER_ENABLED)
            float4 red = POI2D_SAMPLER_PAN(_RedTexure, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        #else
            float4 red = 1;
        #endif
        #if defined(PROP_GREENTEXTURE) || !defined(OPTIMIZER_ENABLED)
            float4 green = POI2D_SAMPLER_PAN(_GreenTexture, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        #else
            float4 green = 1;
        #endif
        #if defined(PROP_BLUETEXTURE) || !defined(OPTIMIZER_ENABLED)
            float4 blue = POI2D_SAMPLER_PAN(_BlueTexture, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        #else
            float4 blue = 1;
        #endif
        #if defined(PROP_ALPHATEXTURE) || !defined(OPTIMIZER_ENABLED)
            float4 alpha = POI2D_SAMPLER_PAN(_AlphaTexture, _MainTex, poiMesh.uv[float(0)], float4(0,0,0,0));
        #else
            float4 alpha = 1;
        #endif
        
        if(_RGBBlendMultiplicative)
        {
            float3 RGBColor = 1;
            RGBColor = lerp(RGBColor, red.rgb * _RedColor.rgb, rgbMask.r * red.a * _RedColor.a);
            RGBColor = lerp(RGBColor, green.rgb * _GreenColor.rgb, rgbMask.g * green.a * _GreenColor.a);
            RGBColor = lerp(RGBColor, blue.rgb * _BlueColor.rgb, rgbMask.b * blue.a * _BlueColor.a);
            RGBColor = lerp(RGBColor, alpha.rgb * _AlphaColor.rgb, rgbMask.a * alpha.a * _AlphaColor.a);
            baseColor *= RGBColor;
        }
        else
        {
            baseColor = lerp(baseColor, red.rgb * _RedColor.rgb, rgbMask.r * red.a * _RedColor.a);
            baseColor = lerp(baseColor, green.rgb * _GreenColor.rgb, rgbMask.g * green.a * _GreenColor.a);
            baseColor = lerp(baseColor, blue.rgb * _BlueColor.rgb, rgbMask.b * blue.a * _BlueColor.a);
            baseColor = lerp(baseColor, alpha.rgb * _AlphaColor.rgb, rgbMask.a * alpha.a * _AlphaColor.a);
        }
        return baseColor;
    }
#endif
