// WaterDistort.fx — Horizontal wave distortion for underwater refraction
// Displaces Y based on X position — creates horizontal wave patterns
// Strongest at pool edges (coastlines), fades to zero in the middle

#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

sampler TextureSampler : register(s0);

float Time;
float Amplitude;    // max UV displacement (e.g. 0.012)
float Frequency;    // wave cycles across width (e.g. 8.0)
float Speed;        // animation speed (e.g. 2.0)
float4 PoolRect;    // x, y, w, h in UV space (0-1 range relative to RT)

float4 WaterDistortPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    // How far from the nearest north/south edge (0 = at edge, 1 = center)
    float relY = (texCoord.y - PoolRect.y) / PoolRect.w; // 0-1 within pool
    float edgeDist = min(relY, 1.0 - relY) * 2.0; // 0 at edges, 1 at center
    float edgeFade = 1.0 - smoothstep(0.0, 0.5, edgeDist); // strong at edges, zero in middle
    
    // Horizontal waves: displace Y based on X
    float wave = sin(texCoord.x * Frequency * 6.28318 + Time * Speed);
    float2 distorted = texCoord;
    distorted.y += wave * Amplitude * edgeFade;
    
    return tex2D(TextureSampler, distorted);
}

technique WaterDistort
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL WaterDistortPS();
    }
}
