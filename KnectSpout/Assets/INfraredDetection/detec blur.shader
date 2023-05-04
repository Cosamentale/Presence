Shader "Unlit/detec blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
	    _float1 ("_float1",Float)=0
		_float2("_float2",Float) = 0 
		_float3("_float3",Float) = 0
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
			sampler2D _MainTex2;
            float4 _MainTex_ST;
			float _float1;
			float _float2;
			float _float3;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			float3 exclusion(float3 s, float3 d)
			{
				return s + d - 2.0 * s * d;
			}
#define Pi 3.14159265359
			float hs(float2 uv) { return frac(sin(dot(uv, float2(45.23, 98.22)))*7845.236); }
			fixed4 frag(v2f i) : SV_Target
			{
				float Directions = 16.0; 
	float Quality = 4.0; 
	float ta = smoothstep(0.25,0.75, sin(_Time.y)*0.5 + 0.5);
	float b = 0.1*lerp(0.6, 1., hs(i.uv))*float2(1., 16. / 9.);
	float2 Size = ta*b;
	float2 Size2 = (1.-ta)*b;
	float c = 0.;
	float c2 = 0.;
	float2 p = lerp(float2(0.5, 0.5), float2(_float1, 1. - _float2), _float3) + (float2(i.uv.x, 1. - i.uv.y) - 0.5)*lerp(1., 0.25, _float3);
	float2 uv2 = float2(i.uv.x, 1. - i.uv.y);
	for (float d = 0.0; d < Pi; d += Pi / Directions)
	{
		for (float i = 1.0 / Quality; i <= 1.0; i += 1.0 / Quality)
		{
			float2 bd = float2(cos(d), sin(d))*i;
			c += tex2D(_MainTex2, uv2 +bd * Size).y;
			c2 += tex2D(_MainTex2, p + bd * Size2).x;
		}
	}
	c /= Quality * Directions - 15.;
	c2 /= Quality * Directions - 15.;
                float t = smoothstep(0.,1.,pow(c2,0.4));
				float t2 = smoothstep(0.,1.,pow(tex2D(_MainTex2,  uv2 ).y,0.4));
				float tt2 = smoothstep(0., 1., pow(c, 0.4));
				float t3 = lerp(t2, pow(exclusion(t, tt2),1.5),_float3);
                return float4(t3,t3,t3,1.);
            }
            ENDCG
        }
    }
}
