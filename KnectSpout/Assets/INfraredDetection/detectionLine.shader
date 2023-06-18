Shader "Unlit/detectionLine"
{
    Properties
    {
        _Tex ("_tex", 2D) = "black" {}
	_bl("bl", 2D) = "black" {}
		//_Tex2("_Tex2", 2D) = "white" {}
	_dither("_dither",Float)=0
		_c4("_c4", Float) = 0
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

            sampler2D _Tex;
			sampler2D _bl;
            float4 _Tex_ST;
			float _dither;
			float _c4;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Tex);
                return o;
            }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			float rd(float t) { return frac(sin(dot(floor(t*12.), 45.))*7845.) + 0.01; }
			float no(float t) { return lerp(rd(t), rd(t + 1.), smoothstep(0., 1., frac(t))); }
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
                float2 c1 = tex2D(_Tex, i.uv).xy;
			float r1 = c1.x + ov(c1.y, lerp(0.5, hs(uv + 23.69), 0.2));
			float r2 = pow(clamp(r1, 0., 1.), 1.);
			float r3 = lerp(r2,step(hn(uv + 98.), pow(r2,2.)),_dither*no(_c4*0.0001 + 95.24));
			//float n4 = smoothstep(0.4, 0.95, no(_c4*0.00007 + 152.))*_step2invert;
			//float pc6 = lerp(smoothstep(0.9, 0.1, pow(pc5, 0.5)), smoothstep(0.1, 0.9, pow(pc5, 2.)), 1. - n4);
                return float4(r3,r3,r3,1.);
            }
            ENDCG
        }
    }
}
