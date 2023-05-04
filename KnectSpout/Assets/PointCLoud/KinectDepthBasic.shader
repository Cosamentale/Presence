Shader "Custom/KinectDepthBasic"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_noise("_noise", 2D) = "white" {}
		_Displacement("Displacement", Range(0, 0.1)) = 0.03
		_comp("_complexity", Range(0, 1.)) = 0
		_dim("_dimention", Range(0, 1.)) = 0
		_ec("_eclatement", Range(0, 1.)) = 0
		/*_b("_b", Float) = 0
		_m("_m", Float) = 0
		_h("_h", Float) = 0*/
		_c1("_c1", Float) = 0
		_c2("_c2", Float) = 0
		_c3("_c3", Float) = 0

	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque" }
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
				float4 col : COLOR;
			};


			sampler2D _MainTex;
			float _Displacement;
			fixed4 _Color;
			sampler2D _noise;
			float4 _MainTex_ST;
			float _audio1;
			float _audio2;
			float _liquide;
			float _centerx;
			float _centery;
			float _centerz;
			float _comp;
			float _dim;
			float _ec;
			/*float _b;
			float _m;
			float _h;*/
			float _c1;
			float _c2;
			float _c3;
			float no(float3 p) {
				float3 f = floor(p); p = smoothstep(0., 1., frac(p));
				float3 se = float3(45., 78., 945.); float4 v1 = dot(se, f) + float4(0., se.y, se.z, se.y + se.z);
				float4 v2 = lerp(frac(sin(v1)*7845.236), frac(sin(v1 + se.x)*7845.236), p.x);
				float2 v3 = lerp(v2.xz, v2.yw, p.y);
				return lerp(v3.x, v3.y, p.z);
			}
			float rd(float3 p) { return frac(sin(dot(floor(p), float3( 45.2, 98.4, 72.1)))*7845.236); }
			float rd1(float p) { return frac(sin(dot(floor(p), 45.265))*7845.236); }
			float n(float t) { return lerp(rd1(t), rd1(t + 1.), frac(t)); }
			float2x2 rot(float t) { float c = cos(t); float s = sin(t); return float2x2(c, -s, s, c); }
			v2f vert(appdata v)
			{
				float4 col = tex2Dlod(_MainTex, float4(v.uv, 0, 0));
				float ta = _c1 * _ec;
				float tb = _c2 * _ec;
				float tc = _c3 * _ec;
				//float time = _Time.y*0.25;
				float dd = n(tc*0.7 + 95.)*0.1;
				float d = col.x * 8000 * dd;
				v.vertex.x = v.vertex.x * d / 3.656 ;
				v.vertex.y = v.vertex.y * d / 3.656 ;
				v.vertex.z = d - dd*200.;

	

				v.vertex.xz = mul(v.vertex.xz, rot(ta*0.25));	
				v.vertex.x = lerp(v.vertex.y, v.vertex.x,max(frac(v.vertex.x),smoothstep(1.,0.5,n(tb*0.25+472.236))));
				v.vertex = lerp(v.vertex,floor(v.vertex*(10.*_comp+1.))/(10.*_comp+1.),n(ta+4568.25)*step(0.8,n(tb*0.1)));
				v.vertex += (float4(rd(v.vertex), rd(v.vertex+45.236), rd(v.vertex+98.14),0.)-0.5)*10.*clamp(n(tc*0.25)-0.5,0.,1.)*2.;

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = float4(1.,1.,1.,1.);

				return col;
			}
			ENDCG
		}
	}
}