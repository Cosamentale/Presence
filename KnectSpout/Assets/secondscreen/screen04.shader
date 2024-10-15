Shader "Unlit/screen04"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _MainTex;
			sampler2D _MainTex3;
            float4 _MainTex_ST;
			float4 _pos;
			float2 _d;
			float2 _d2;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
			{
				float2 uv = i.uv;
				float2 res = float2(1366., 768.);
				float2 un = uv * (res / 1024.);
				float t0 = tex2D(_MainTex, lerp(_pos.xy - float2(0.25, 0.), _pos.zw + float2(0.25, 0.), step(0.5, uv.x)) + (uv - 0.5)*0.1).x;
				float t1 = lerp(step(tex2D(_MainTex3, un).y, t0), t0, 0.75);
				
				return float4(t1,t1,t1,1.)*lerp(step(0.75,_d.x),step(0.75,_d.y),step(0.5,uv.x))*step(0.025,uv.x)*step(uv.x,0.975)*step(0.0125,length(uv.x-0.5))
					*step(0.044, uv.y)*step(uv.y, 0.966);
			}
            ENDCG
        }
    }
}
