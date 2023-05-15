Shader "Unlit/DefaultBuffer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
		_Tex ("_Tex", 2D) = "black" {}
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
			sampler2D _Tex;
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
				float t1 = tex2D(_MainTex, uv).a;

				float t2 = tex2D(_Tex, uv).x;

				float t3 = smoothstep(0.1,0.2,normalize(float2(abs(t2 - t1),0.5)).x);
				float3 t4 = lerp(float3(t2, t2, t2), float3(0., 0., 1.),t3);
                return float4(t4,1.);
            }
            ENDCG
        }
    }
}
