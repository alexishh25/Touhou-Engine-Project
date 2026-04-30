Shader "Custom/RenderTexture"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}

    }

    SubShader {

        Tags 
        {
            "Queue"="Transparent" 
            "RenderType" = "Transparent" 
        }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass{
            CGPROGRAM
                #pragma vertex vertexFunction
                #pragma fragment fragmentFunction

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 position : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _TintColor;
                float _Transparency;
                float _CutoutThresh;
                float _Distance;
                float _Amplitude;
                float _Speed;
                float _Amount;

                v2f vertexFunction (appdata IN) 
                {
                   v2f OUT;
                   OUT.position = UnityObjectToClipPos(IN.vertex);
                   OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                   return OUT;
                }

                fixed4 fragmentFunction (v2f IN) : SV_TARGET 
                {
                    float2 uv = IN.uv;
                    uv.x += sin(_Time.y * _Speed + uv.y * _Amplitude) * _Amount;

                    fixed4 col = tex2D(_MainTex, uv) * _TintColor;
                    col.a *= _Transparency;
                    clip(col.r - _CutoutThresh);
                    return col;
                }
            ENDCG
        }    
    }
}
