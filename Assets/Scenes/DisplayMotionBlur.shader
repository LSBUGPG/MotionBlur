Shader "Hidden/Custom/DisplayMotionBlur"
{
    HLSLINCLUDE
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    TEXTURE2D_SAMPLER2D(_GhostTex, sampler_GhostTex);

    float _Fade;

    float4 frag(VaryingsDefault i) : SV_Target
    {
        float2 uv = i.texcoord;
        float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
        float4 col2 = SAMPLE_TEXTURE2D(_GhostTex, sampler_GhostTex, uv);
        col.rgb = col.rgb + col2.rgb * _Fade;
        return col;
    }

    ENDHLSL
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
    }
}
