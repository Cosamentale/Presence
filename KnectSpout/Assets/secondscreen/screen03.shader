Shader "Unlit/screen03"
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
			sampler2D _MainTex2;
			sampler2D _MainTex3;
            float4 _MainTex_ST;
			float _float1;
			float _float2;
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
				float2 res = float2(1366., 768.);
				float2 un = uc * (res / 1024.);
				float t0 = tex2D(_MainTex, uc).y;
				float t1 = lerp(step(tex2D(_MainTex3, un).y,t0),t0,0.75);
				
				/*float2 e = float2(0.1, 0.);
				float l1 = smoothstep(0.02, 0.2, tex2D(_MainTex2, floor(uc*64.+e.xy) / 64.).y);
				float l2 = smoothstep(0.02, 0.2, tex2D(_MainTex2, floor(uc*64.-e.xy) / 64.).y);
				float l3 = smoothstep(0.02, 0.2, tex2D(_MainTex2, floor(uc*64.+e.yx) / 64.).y);
				float l4 = smoothstep(0.02, 0.2, tex2D(_MainTex2, floor(uc*64.-e.yx) / 64.).y);
				float ll = max(normalize(float2(max(abs(l1 - l2), abs(l3 - l4)), 0.5))*0.5, *0.75);*/
				float d1 = smoothstep(1./res.x, 0., distance(uc.x, _float1))+ smoothstep(1./res.y, 0., distance(uc.y, _float2));
				fixed4 col0 = float4(lerp(float3(t1,t1,t1),float3(0.,0.,1.), smoothstep(0.02, 0.2, tex2D(_MainTex2, floor(uc*64.) / 64.).y)*0.75),1.)+d1;
			
				
                return col0;
            }
            ENDCG
        }
    }
}
