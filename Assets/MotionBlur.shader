Shader "Custom/MotionBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackTex ("Texture2", 2D) = "white" {}
        _Level ("Level", Range(0, 1)) = 1.0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _BackTex;
            float _Level;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col2 = tex2D(_BackTex, i.uv);
            // just invert the colors
                col.rgb = lerp(col2.rgb, col.rgb, _Level);
                return col;
            }
            ENDCG
        }
    }
}
