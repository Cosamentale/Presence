Shader "Unlit/Compo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	 _MainTex2("Texture", 2D) = "white" {}
	_bl("_bl", 2D) = "white" {}
	_p1("_p1",Float) = 0
		_p2("_p2",Float) = 0
		_frame("_frame",Float) = 0
		_speed1("_speed1",Float) = 0
			_speed2("_speed2",Float) = 0
			_speed3("_speed3",Float) = 0
		_resy("_resy", Float) = 0
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
			float _p1;
			float _p2;
			float _frame;
			float _speed1;
			float _speed2;
			float _speed3;
			float _resy;
			sampler2D _bl;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float rd(float t) { return frac(sin(dot(floor(t), 45.236))*7845.236); }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float li(float2 uv, float2 a, float2 b) {
				float2 ua = uv - a; float2 ba = b - a;
				float h = clamp(dot(ua, ba) / dot(ba, ba), 0., 1.);
				return length(ua - ba * h);
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float ff1 = _frame / _speed1;
                float c = tex2D(_MainTex2,uv).a;
				float c2 = tex2D(_MainTex, float2(frac(uv.y*4.), uv.x)).x;
				float po = _p1;
				float c3 = lerp(c2, c, step(0.125,distance(uv.y ,_p1) ));
				float l1 = step(distance(uv.y, _p2), 0.5/1080.);
				float l = smoothstep(1. / 1080., 0., li(uv, float2(frac(ff1), _p2), float2( frac(floor(ff1)*10. / _resy - 5. / _resy),lerp(_p1-0.125, _p1+0.125,frac( ff1))) ));
                return float4(tex2D(_bl,uv*float2(1920.,1080.)/1024.).x*float3(1.,1.,1.)*0.+c3+l1,1.)+l;
            }
            ENDCG
        }
    }
}
