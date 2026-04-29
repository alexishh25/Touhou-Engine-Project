Shader "Custom/UVEditMapping"
{
    Properties 
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _BaseTexture("Base Texture", 2D) = "white" {}
        _ScrollSpeed("Scroll Speed", Vector) = (0, 0, 0, 0)
        
        // Añadimos una propiedad para definir qué parte de la textura queremos (X, Y, Ancho, Alto)
        _SpriteRect("Sprite Rect (X, Y, Width, Height)", Vector) = (0, 0, 1, 1)
    }

    SubShader 
    {
        Tags 
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/GlobalSamplers.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseTexture_ST;
                float2 _ScrollSpeed;
                float4 _SpriteRect; // x = PosX, y = PosY, z = Ancho, w = Alto
            CBUFFER_END

            TEXTURE2D(_BaseTexture);
            SAMPLER(sampler_BaseTexture);

            struct appdata
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o = (v2f)0;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _BaseTexture);

                return o;
            }

            float4 frag(v2f i) : SV_TARGET
            {

                float2 localUV = frac(i.uv + _Time.x * _ScrollSpeed);
                
                float2 finalUV = (localUV * _SpriteRect.zw) + _SpriteRect.xy;

                float4 textureColor = SAMPLE_TEXTURE2D(_BaseTexture, sampler_BaseTexture, finalUV);
                return textureColor * _BaseColor;
            }

            ENDHLSL
        } 
    }
}
