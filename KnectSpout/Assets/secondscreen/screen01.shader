Shader "Unlit/screen01"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            float4 _MainTex_ST;
			float4 _data[4];
			float _ratio[4];
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float map(float low1, float low2, float high1, float high2, float value) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }
			float map2(float low1, float high1, float value) { return  (value - low1) / (high1 - low1); }
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uc = i.uv;
				float2 uv = frac(i.uv*1.);
				uv.x -= 0.5;
				uv.x *= 2.;
				float2 u0 = uv; float2 u1 = uv; float2 u2 = uv; float2 u3 = uv;
				u0.x /= _ratio[0];
				/*u1.x /= _ratio[1];
				u2.x /= _ratio[2];
				u3.x /= _ratio[3];*/
				float2 ud0 = float2(map(-1.,_data[0].x,1., _data[0].y, u0.x), map(1.,_data[0].z,0., _data[0].w,u0.y));
				/*float2 ud1 = float2(map(-1., _data[1].x, 1., _data[1].y, u0.x), map(1., _data[1].z, 0., _data[1].w, u1.y));
				float2 ud2 = float2(map(-1., _data[2].x, 1., _data[2].y, u0.x), map(1., _data[2].z, 0., _data[2].w, u1.y));
				float2 ud3 = float2(map(-1., _data[3].x, 1., _data[3].y, u0.x), map(1., _data[3].z, 0., _data[3].w, u1.y));
				float lx = step( uc.x,0.5);
				float ly = step( uc.y,0.5);*/
				fixed4 col0 = tex2D(_MainTex, ud0)*step(length(u0.x), 1.);
				/*fixed4 col1 = tex2D(_MainTex, ud1)*step(length(u1.x), 1.);
				fixed4 col2 = tex2D(_MainTex, ud2)*step(length(u2.x), 1.);
				fixed4 col3 = tex2D(_MainTex, ud3)*step(length(u3.x), 1.);*/
				//fixed4 col = lerp(lerp(col0,col1, lx), lerp(col2, col3, lx), ly);
				
                return col0;
            }
            ENDCG
        }
    }
}
