Shader"Custom/TestShader"
{
    Properties
    {
        _MainTex("Main Texture (RGB)", 2D) = "white" {}
        _MaskTex("Mask Texture (BW)", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
sampler2D _MaskTex;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

half4 frag(v2f i) : SV_Target
{
    half4 col = tex2D(_MainTex, i.uv);
    half maskValue = tex2D(_MaskTex, i.uv).r; // using red channel for BW mask

    half4 invertedCol = half4(1, 1, 1, 1) - col; // invert the color
    half4 finalColor = lerp(invertedCol, col, maskValue); // linearly interpolate between inverted and original color based on the mask value

    return finalColor;
}
            ENDCG
        }
    }
}