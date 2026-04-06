// PostProcess.fx — Bloom, Vignette, IGN debanding
// MonoGame DesktopGL (OpenGL backend)

#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Shared sampler
sampler TextureSampler : register(s0);

// ═══════════════ PASS 1: Brightness Extract ═══════════════

float BloomThreshold;

float4 BrightnessExtractPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(TextureSampler, texCoord);
    float brightness = dot(color.rgb, float3(0.2126, 0.7152, 0.0722));
    float contribution = max(0, brightness - BloomThreshold);
    return float4(color.rgb * (contribution / max(brightness, 0.001)), 1.0);
}

technique BrightnessExtract
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL BrightnessExtractPS();
    }
}

// ═══════════════ PASS 2: Gaussian Blur ═══════════════

float2 BlurDirection;

float4 GaussianBlurPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    // 5-tap gaussian weights inlined (no static const array — better cross-platform compat)
    float4 result = tex2D(TextureSampler, texCoord) * 0.227027;
    
    float2 off1 = BlurDirection * 1.0;
    float2 off2 = BlurDirection * 2.0;
    float2 off3 = BlurDirection * 3.0;
    float2 off4 = BlurDirection * 4.0;
    
    result += tex2D(TextureSampler, texCoord + off1) * 0.194594;
    result += tex2D(TextureSampler, texCoord - off1) * 0.194594;
    result += tex2D(TextureSampler, texCoord + off2) * 0.121621;
    result += tex2D(TextureSampler, texCoord - off2) * 0.121621;
    result += tex2D(TextureSampler, texCoord + off3) * 0.054054;
    result += tex2D(TextureSampler, texCoord - off3) * 0.054054;
    result += tex2D(TextureSampler, texCoord + off4) * 0.016216;
    result += tex2D(TextureSampler, texCoord - off4) * 0.016216;
    
    return result;
}

technique GaussianBlur
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL GaussianBlurPS();
    }
}

// ═══════════════ PASS 3: Composite (Bloom + Vignette + IGN) ═══════════════

sampler BloomSampler : register(s1);

float BloomIntensity;
float VignetteRadius;
float VignetteSmooth;
float2 VignetteCenter;
float Time;

float4 CompositePS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 scene = tex2D(TextureSampler, texCoord);
    float4 bloom = tex2D(BloomSampler, texCoord);
    
    // Add bloom
    float3 color = scene.rgb + bloom.rgb * BloomIntensity;
    
    // Player-centered vignette
    float2 diff = texCoord - VignetteCenter;
    diff.x *= 1.777;
    float dist = length(diff);
    float vignette = smoothstep(VignetteRadius, VignetteRadius - VignetteSmooth, dist);
    color *= lerp(0.3, 1.0, vignette);
    
    // IGN dithering (Interleaved Gradient Noise — kills color banding)
    float2 screenPos = texCoord * float2(1280.0, 720.0) + float2(Time * 100.0, Time * 57.0);
    float noise = frac(52.9829189 * frac(dot(screenPos, float2(0.06711056, 0.00583715))));
    color += (noise - 0.5) / 255.0;
    
    return float4(saturate(color), 1.0);
}

technique Composite
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL CompositePS();
    }
}
