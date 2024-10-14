Shader "Unlit/FinalTestyolo"
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
            float4 _data[10];
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            float li(float2 a, float2 b,float2 p){ float2 ba  = b-a; float2 pa = p-a; float h=  clamp(dot(pa,ba)/dot(ba,ba),0.,1.);
                return length(pa-ba*h);}
            fixed4 frag (v2f i) : SV_Target
            {
            float l1 = 0.;   		
            float2 uv = i.uv;
			for (int j = 0; j < 10; j++)
			{
				float4 data = _data[j];
                float p1 = data.x;
                float p2 = data.y;
                float p3 = data.z;
                float p4 = data.w;
				l1 =  max(l1,smoothstep(0.001,0.,li(float2(p1,p3),float2(p1,p4),uv)));
                l1 =  max(l1,smoothstep(0.001,0.,li(float2(p2,p3),float2(p2,p4),uv)));
                l1 =  max(l1,smoothstep(0.001,0.,li(float2(p1,p3),float2(p2,p3),uv)));
                l1 =  max(l1,smoothstep(0.001,0.,li(float2(p1,p4),float2(p2,p4),uv)));
            

			}
                fixed4 col = tex2D(_MainTex, i.uv);
                return col+l1;
            }
            ENDCG
        }
    }
}
