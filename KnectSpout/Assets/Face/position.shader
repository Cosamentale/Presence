Shader "Unlit/position"
{
	Properties
	{
		_Texture("_Texture", 2D) = "black" {}
	 
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
			
			//float4 _data1;
			//float4 _data2;
			float4 _data[10];
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Texture);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
		
				fixed4 col = tex2D(_Texture, i.uv);
			float2 uv =float2( i.uv.x,1.-i.uv.y);
			//float d1 = step(_data1.x, uv.x)*step(_data1.y, uv.y)*step(uv.x, _data1.z)*step(uv.y, _data1.w)*0.7;
			//float d2 = step(_data2.x, uv.x)*step(_data2.y, uv.y)*step(uv.x, _data2.z)*step(uv.y, _data2.w)*0.7;
			//float d3 = d1 + d2;
			float d = 0.0;
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
			return float4(0.,0.,0.,1.)+col;
		}
		ENDCG
	}
	}
}
