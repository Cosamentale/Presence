Shader "Unlit/detec"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
		_frame("_frame",Float) = 0
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
			float _frame;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float li(float2 uv, float2 a ,float2 b) {
				float2 ua = uv - a; float2 ba = b - a;
				float h = clamp(dot(ua, ba) / dot(ba, ba), 0., 1.);
				return length(ua - ba * h);
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float2 ui = (i.uv-float2(0.,512./1920./1.5))*float2(512./424. ,1.)*1.5;
				float ff = frac(_frame / 60.);
				float t1 = smoothstep(0., 1., pow(tex2D(_MainTex2,float2(ui.x,1.-ui.y)).x, 0.3))*step(i.uv.x,424./512./1.5)*step(distance(ui.y,0.5),0.5);
				float t2 = tex2D(_MainTex, i.uv*float2(5.,1.)-float2(3.8,0.)).x*step(i.uv.x,0.97)*step(0.75,i.uv.x);
				float c = step(distance(ui.x, ff),1. / 1920.)*step(distance(0.5,i.uv.y),5./1080.)+
					step(distance(ui.x, ff), 8.88 / 1920.)*step(distance(0.5, i.uv.y), 0.5625 / 1080.);
				float l = smoothstep(1./1080.,0.,li(i.uv, float2(0.55, 0.5), float2(lerp(0.75, 0.97, ff), frac(floor(_frame / 60.)*10. /1024.) - 5. / 1024.)));
				float t3 = t1 + smoothstep(0.,0.7,t2)+c+l;
			return float4(t3, t3, t3, 1.);
            }
            ENDCG
        }
    }
}
