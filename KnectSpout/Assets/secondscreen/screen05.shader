Shader "Unlit/screen05"
{
    Properties
    {
        _cam ("Texture", 2D) = "white" {}
	_MainTex3("Texturenoise", 2D) = "white" {}
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

			sampler2D _cam;
			sampler2D _MainTex3;
            float4 _cam_ST;
			float4 _data;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _cam);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				float2 res = float2(1366., 768.);
				float2 un = uv * (res / 1024.);
				float t0 = tex2D(_cam, uv).x;
				float t1 = lerp(step(tex2D(_MainTex3, un).y, t0), t0, 0.75);
				uv.y = 1. - uv.y;
				float4 data = _data;
				float stepX = step(data.x, uv.x);
				float stepY = step(data.y, uv.y);
				float stepXInv = step(uv.x, data.z);
				float stepYInv = step(uv.y, data.w);
				float d = (step(distance(uv.x, data.x), 0.5 / 1920.) + step(distance(uv.x, data.z), 0.5 / 1920.))*stepY*stepYInv
					+ (step(distance(uv.y, data.y), 0.5 / 1080.) + step(distance(uv.y, data.w), 0.5 / 1080.))*stepX*stepXInv;
				return t1 + d;
			}
            ENDCG
        }
    }
}
