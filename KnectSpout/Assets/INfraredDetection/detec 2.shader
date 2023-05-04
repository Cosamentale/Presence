Shader "Unlit/detec2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
	    _float1 ("_float1",Float)=0
		_float2("_float2",Float) = 0 
		_float3("_float3",Float) = 0
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
			float _float1;
			float _float2;
			float _float3;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = tex2D(_MainTex2,lerp(float2(0.5,0.5),float2(_float1, 1.-_float2),_float3)+ (float2(i.uv.x,1. - i.uv.y) -0.5)*lerp(1.,0.25,_float3)).x;
			float t2 = smoothstep(0.,1.,pow(tex2D(_MainTex2,  (float2(i.uv.x,1.-i.uv.y) )).y,0.3));
			float t1 = tex2D(_MainTex, i.uv).y;
			float pt1 = smoothstep(0.01, 0., distance(i.uv, float2(_float1, _float2)  ));
                return float4(t,t,t,1.);
            }
            ENDCG
        }
    }
}
