Shader "Hidden/ExtractChannel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Channel ("Channel", Float) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float _Channel;

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

            // Use UnityCG for cross-pipeline support
            #include "UnityCG.cginc"

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // Built-in pipeline-compatible function
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);

                float value = _Channel == 0 ? col.r :
                              _Channel == 1 ? col.g :
                              _Channel == 2 ? col.b : 0;

                return float4(value, value, value, 1);
            }
            ENDCG
        }
    }
}
