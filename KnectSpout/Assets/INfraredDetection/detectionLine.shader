Shader "Unlit/detectionLine"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_bl("bl", 2D) = "white" {}
		//_MainTex2("_MainTex2", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
			sampler2D _bl;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
                float2 c1 = tex2D(_MainTex, i.uv).xy;
			float r1 = c1.x + ov(c1.y, lerp(0.5, hs(uv + 23.69), 0.2));
                return float4(r1,r1,r1,1.);
            }
            ENDCG
        }
    }
}
