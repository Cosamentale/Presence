Shader "Unlit/position"
{
	Properties
	{
		_Texture("_Texture", 2D) = "black" {}
	_cam("_cam", 2D) = "black" {}
	_bl("_bl", 2D) = "black" {}
	_float1("_float1",Range(0,1)) = 0
		_float2("_float2",Range(0,1)) = 0
		_float3("_float3",Range(0,1)) = 0
	 
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
	
			#include "UnityCG.cginc"
			//#include "Packages/jp.keijiro.ultraface/Shader/Common.hlsl"

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

			sampler2D _Texture;
			float4 _Texture_ST;
			sampler2D _cam;
			sampler2D _bl;
			float _float1;
			float _float2;
			float _float3;
			float4 _data[10];
			float _r3;
			float _c3;
			float _r4;
			float _c4;
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Texture);

				return o;
			}
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float rd(float t) { return frac(sin(dot(floor(t*12.), 45.))*7845.) + 0.01; }
			float no(float t) { return lerp(rd(t), rd(t + 1.), smoothstep(0., 1., frac(t))); }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			fixed4 frag(v2f i) : SV_Target
			{
		
				float c = tex2D(_cam, i.uv).x;
			float2 uv =float2( i.uv.x,1.-i.uv.y);
			int d = 0.;			
			for (int j = 0; j < 10; j++)
			{
				float4 data = _data[j];
				float stepX = step(data.x, uv.x);
				float stepY = step(data.y, uv.y);
				float stepXInv = step(uv.x, data.z);
				float stepYInv = step(uv.y, data.w);
				d += (step(distance(uv.x, data.x), 0.5 / 1920.)+ step(distance(uv.x, data.z), 0.5 / 1920.))*stepY*stepYInv
					+ (step(distance(uv.y, data.y), 0.5 / 1080.) + step(distance(uv.y, data.w), 0.5 / 1080.))*stepX*stepXInv;
			}
			float2 c2 = tex2D(_Texture, i.uv).xy;
			float s3 = step(0.5, _float3);
			/*float nby = floor(5.*_r3 + 3.);
			float r4 = floor(_r4*nby);
			float yd =step(1. / (nby),distance(0.5*nby+1./nby,uv.y*nby));*/
			float ma = lerp(0., step(0.5, c2.y),step(0.5, _float3));//step(0.5, c2.y);
			float c3 = pow(lerp(c, c2.x,ma),1.);
			float c4 = ov(c3, lerp(0.5, hs(uv + 23.69), 0.2));
			float c5 = lerp(smoothstep(.2,0.8,c4),step( hn(uv + 98.),pow(c4,2.)), no(_c4*0.0001 + 95.24));
			float m = step(0.5, frac(uv.y*2.5));

			float baf3 = step(min(min(length(uv.x - 0.3), length(uv.x -0.6)), min(length(uv.x - 0.7), length(uv.x - 0.999))), 0.5/1920.)
				*step(0.2, uv.y)*step(uv.y,0.8);
			float bf3 = step(min(length(uv.y - 0.7999), length(uv.y - 0.2)), 0.5/1080.)*step(0.3, uv.x)*step(0.05,length(uv.x-0.65)) +baf3;
			float ll = lerp(bf3, d, _float1)*(1.-ma) +c5*_float2;
			return float4(ll, ll, ll, 1.);
		}
		ENDCG
	}
	}
}
