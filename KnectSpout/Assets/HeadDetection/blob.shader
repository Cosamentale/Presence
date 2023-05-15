Shader "Unlit/blob"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_noi("_noi", 2D) = "white" {}
		_cam("_cam", 2D) = "white" {}
		_m("_m", 2D) = "white" {}
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
			sampler2D _noi;
			sampler2D _cam;
			sampler2D _m;
            float4 _MainTex_ST;

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
				float2 u = uv * float2(1920., 1080.) / 1024.;
				float c =tex2D(_cam, uv).x;
				float c1 = pow(c, 1.);
				float c2 = tex2D(_m, uv).x;
				float m = tex2D(_MainTex, uv).x;
				float n = tex2D(_noi, u).x;
				float m2 = smoothstep(0.1, 0.2, abs(c - c2));
				float3 r0 = lerp(float3(0., 0., 1.), float3(1., 1., 1.), step(n, pow(c,2.))*m2);
				float3 r1 = lerp(float3(c1, c1, c1),r0, m);
                return float4(r1,1.);
            }
            ENDCG
        }
    }
}
