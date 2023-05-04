Shader "Unlit/histogram"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_iteration("_iteration", Float)=0
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
			float _iteration;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv =i.uv;
				float v =_iteration;
				float bucket_min =log(floor(uv.y*v));
				float bucket_max =log(floor((uv.y*v)+1.));
				float c = 0.;
				for (int i = 0; i <int(v); i++) {
				float j = float(i) / v;
				float pixel = tex2D(_MainTex, float2(uv.x,j)).x*v;
				float logpixel = log(pixel);
				if (logpixel >= bucket_min && logpixel < bucket_max) c += 1.;
				}
				float gain = 0.3;
				c = log(c)*gain;
				return c;
            }
            ENDCG
        }
    }
}
