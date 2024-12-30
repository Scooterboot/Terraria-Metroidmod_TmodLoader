sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
float3 uColor;
float3 uSecondaryColor;
float uOpacity;
float uSaturation;
float uRotation;
float uTime;
float4 uSourceRect;
float2 uWorldPosition;
float uDirection;
float3 uLightSource;
float2 uImageSize0;
float2 uImageSize1;
float2 uTargetPosition;
float4 uLegacyArmorSourceRect;
float2 uLegacyArmorSheetSize;


float4 FlashShaderFunction(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float3x4 rowB;
    float3x4 rowM;
    float3x4 rowT;
    
    float len = uSourceRect.z / 4;
    
    float t = frac(uOpacity);
    float s = max(2, len * t); //Makes the outline start thick and gradually thin
    
    rowM[1] = tex2D(uImage0, coords);

    float2 pixCoords = coords * uImageSize0 - uSourceRect.xy;
    float2 pCoords = pixCoords;
    pCoords.x -= s;
    pCoords.y += s;
    float2 coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowB[0] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.y += s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowB[1] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.x += s;
    pCoords.y += s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowB[2] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.x -= s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;
    rowM[0] = tex2D(uImage0, coordsFromPix);

    
    pCoords = pixCoords;
    pCoords.x += s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowM[2] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.x -= s;
    pCoords.y -= s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowT[0] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.y -= s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowT[1] = tex2D(uImage0, coordsFromPix);
    pCoords = pixCoords;
    pCoords.x += s;
    pCoords.y -= s;
    coordsFromPix = (pCoords + uSourceRect.xy) / uImageSize0;

    rowT[2] = tex2D(uImage0, coordsFromPix);
    
    float4 cA = rowB[1] + rowM[0] + rowM[2] + rowT[1]; //Alphas of adjacent pixels added together
    float4 cM = rowB[1] * rowM[0] * rowM[2] * rowT[1]; //Alphas of adjacent pixels multiplied together
    float4 color = rowM[1]; //Color of center pixel
    float luminosity = (color.r + color.g + color.b) / 3;
    
    color *= sampleColor;
    //float3 testColor = float3(1, 1, 1);
    //float3 testColor2 = float3(0, 1, 1);
    float intensity = t;
    
    /*
    float frameX = (coords.x * uImageSize0.x - uSourceRect.x) / uSourceRect.z;
    float frameY = (coords.y * uImageSize0.y - uSourceRect.y) / uSourceRect.w;
    t *= 0.5;
    if (frameX < t)
    {
        intensity = 1 - (frameX * 1 / t);
    }
    if (frameX > 1 - t)
    {
        intensity = 1 - ((1 - frameX) * 1 / t);
    }
    
    if (frameY < t + 0.1)
    {
        intensity += 1 - (frameY * 1 / (t + 0.1));
    }
    if (frameY > 1 - (t / 2))
    {
        intensity += 1 - ((1 - frameY) * 1 / (t / 2));
    }
    
    intensity *= 2;
*/

    
    if (cM.a == 0 && rowM[1][3] != 0)
    {
        color.r += max(uSaturation * uSecondaryColor.r, uColor.r * uSaturation * 2);
        color.g += max(uSaturation * uSecondaryColor.g, uColor.g * uSaturation * 2);
        color.b += max(uSaturation * uSecondaryColor.b, uColor.b * uSaturation * 2);
        //color.gb = max(intensity * 0.5, uColor * intensity * 2);
    }
    else if (color.a != 0)
    {
        color.rgb += uColor * intensity * sampleColor.rgb;
    }
    
    
    return color;

}
technique Technique1
{
    pass FlashShaderPass
    {
        PixelShader = compile ps_2_0 FlashShaderFunction();
    }
}