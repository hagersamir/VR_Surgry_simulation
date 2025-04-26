Shader "Custom/XrayVision"
{
    Properties
    {
        _Transparency ("Transparency", Range(0,1)) = 0.5
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _Transparency;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 normal : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float grayscale = dot(i.normal, float3(0, 0, 1));
                grayscale = saturate(grayscale * 0.5 + 0.5);  

                // Ensure proper transparency blending
                return fixed4(grayscale, grayscale, grayscale, lerp(1.0, 0.3, _Transparency));
            }
            ENDCG
        }
    }
}