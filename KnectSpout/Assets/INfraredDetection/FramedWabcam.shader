Shader "Unlit/FramedWabcam"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_MainTex2("Texture2", 2D) = "white" {}
	_MainTex3("Texture3", 2D) = "white" {}
	_speed("_speed",Float) = 0
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
			float _speed;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float rd(float t) { return frac(sin(dot(floor(t), 45.266))*7845.236); }
            fixed4 frag (v2f i) : SV_Target
            {
				float t = _Time.y*_speed;
				float r4 = step(0.5, rd(t*0.5 + 954.69));
				float r5 = step(0.25, rd(t*0.5 + 98.56));
				float r6 = step(0.5, rd(t*0.5 + 672.145));
				float2 uv = i.uv;
				float2 uv2 = lerp(lerp(i.uv,float2(i.uv.x,frac(i.uv.y + 0.5)), r6),
					lerp(i.uv, float2(frac(i.uv.x + 0.5),i.uv.y ), r6),max(r4,1.-r5));
                float t1 = tex2D(_MainTex, uv2).x;
				float t2 = tex2D(_MainTex2, uv2).x;
				float t3 = tex2D(_MainTex3, uv2).x;
				float l1 = step(1. / 1920., length(uv.x - 0.5));
				float l2 = step(1. / 1080., length(uv.y - 0.5));
				float r1 = step(0.5,rd(t + 65.15));
				float r2 = step(0.5, rd(t + 45.12));
				float r3 = step(2. / 3., rd(t + 78.49));
				float u1 = step(0.5, uv.x);
				float u2 = step(0.5, uv.y);
				float tl1 = lerp(lerp(t1, t3, r2), t2, r3);
				float tm1 = lerp(t1, lerp(t3,t2,r1),u1);
				float tm2 = lerp(t2, lerp(t1, t3, r1), u1);
				float tm3 = lerp(t3, lerp(t1, t2, r1), u1);
				float tm4 = lerp(1.,lerp(lerp(tm1, tm2, r2), tm3, r3),l1);
				float tn1 = lerp(1.,lerp(lerp(1.,lerp(lerp(t2, t3, u1), lerp(t3, t2, u1), r1),l1),t1,u2),l2);
				float tn2 = lerp(1.,lerp(lerp(1., lerp(lerp(t1, t3, u1), lerp(t3, t1, u1), r1), l1), t2, u2),l2);
				float tn3 = lerp(1.,lerp(lerp(1., lerp(lerp(t2, t1, u1), lerp(t2, t1, u1), r1), l1), t3, u2),l2);
				float tn4 = lerp(lerp(tn1, tn2, r2), tn3,r3);
				float to1 = lerp(1., lerp(lerp(1., lerp(lerp(t2, t3, u2), lerp(t3, t2, u2), r1), l2), t1, u1), l1);
				float to2 = lerp(1., lerp(lerp(1., lerp(lerp(t1, t3, u2), lerp(t3, t1, u2), r1), l2), t2, u1), l1);
				float to3 = lerp(1., lerp(lerp(1., lerp(lerp(t2, t1, u2), lerp(t2, t1, u2), r1), l2), t3, u1), l1);
				float to4 = lerp(lerp(to1, to2, r2), to3, r3);
				float tf1 = lerp(tm4,lerp(tn4, to4, r4),r5);
                return float4(tf1,tf1,tf1,1.);
            }
            ENDCG
        }
    }
}
