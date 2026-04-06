#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Scene texture (what was drawn to _sceneRT)
sampler2D SceneSampler : register(s0);

// Light mask texture (drawn separately — bright = lit, dark = shadow)
Texture2D LightMask;
sampler2D LightMaskSampler = sampler_state
{
    Texture = <LightMask>;
};

// Ambient light level (so fully dark areas aren't pure black)
float AmbientLevel = 0.08;

float4 MainPS(float4 position : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 sceneColor = tex2D(SceneSampler, texCoord);
    float4 lightColor = tex2D(LightMaskSampler, texCoord);
    
    // Combine: scene * max(light, ambient)
    // This ensures even unlit areas have a minimum visibility
    float3 light = max(lightColor.rgb, float3(AmbientLevel, AmbientLevel, AmbientLevel));
    
    return float4(sceneColor.rgb * light, sceneColor.a);
}

technique LightMaskTechnique
{
    pass Pass0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};
