Shader "Touhou/LOLK_DeathEffect"
{
    Properties
    {
        _Radius ("Radius", Range(0, 2)) = 0.5
        _Thickness ("Thickness", Range(0, 1)) = 0.1
        _Distortion ("Distortion Strength", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "DisableBatching"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            TEXTURE2D(_CameraSortingLayerTexture);
            SAMPLER(sampler_CameraSortingLayerTexture);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float _Radius;
                float _Thickness;
                float _Distortion;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;


                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;

                output.screenPos = ComputeScreenPos(output.positionCS);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float dist = distance(input.uv, float2(0.5, 0.5));

                float outerRing = step(dist, _Radius);
                float innerRing = step(dist, _Radius - _Thickness);

                float ringMask = outerRing - innerRing;

                float2 dir = input.uv - float2(0.5, 0.5);
                float2 screenUV = input.screenPos.xy / input.screenPos.w;
                screenUV += dir * _Distortion * ringMask;

                half3 sceneColor = SAMPLE_TEXTURE2D(_CameraSortingLayerTexture, sampler_CameraSortingLayerTexture, screenUV).rgb;
                half3 invertedColor = 1.0 - sceneColor;

                return half4(invertedColor, ringMask);
                
            }

            ENDHLSL
        }
    }
}
