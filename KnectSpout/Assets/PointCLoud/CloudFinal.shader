Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("_MainTex", 2D) = "white" {}
		_CloudTex("_CloudTex", 2D) = "white" {}
		_v1("_v1", Range(0, 1)) = 0
		_v2("_v2", Range(0, 1)) = 0
		_c4("_c4", Float) = 0
		_c5("_c5", Float) = 0
		_c6("_c6", Float) = 0
		_c1("_c1", Float) = 0
		_c2("_c2", Float) = 0
		_c3("_c3", Float) = 0
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
			sampler2D _CloudTex;
            float4 _MainTex_ST;
			float _v1;
			float _v2;
			float _c4;
			float _c5;
			float _c6;
			float _c1;
			float _c2;
			float _c3;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float rd(float t) { return frac(sin(dot(floor(t), 45.236)) * 78545); }
			float n(float t) { return lerp(rd(t), rd(t + 1.), frac(t)); }
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float time = _Time.y;
				float2 u2 = float2(lerp(lerp(1.-uv.x,uv.x,step(0.5,uv.x)),uv.x,step(0.3,rd(_c2*0.25))),uv.y);
				float t1 = pow(tex2D(_MainTex, u2).x, _v1-0.3*n(_c1));
				float t2 = tex2D(_CloudTex, u2).x;
				float t3 = max(t1, t2*_v2);
				float t4 = lerp(t3, 1. - t3, step(0.9, rd(_c3*0.1)));
				float s = (1.-lerp(lerp(step(_c4*0.1, uv.y - 0.9),
					step(_c5*0.1, uv.y-0.9), step(0.01, uv.x)), 
					step(_c6*0.1, uv.y - 0.9), step(0.02, uv.x)))*step(uv.x,0.03)*step(0.9,uv.y)*0.1;
					return float4(t4, t4, t4, 1.)+s;
            }
            ENDCG
        }
    }
}
