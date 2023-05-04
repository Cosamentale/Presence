Shader "Unlit/WebCamCompo01"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainTex2("Texture2", 2D) = "white" {}
		_frame("_frame",Float) = 0
		_speed1("_speed1",Float) = 0
			_speed2("_speed2",Float) = 0
			_speed3("_speed3",Float) = 0
			_p1("_p1",Float) = 0
			_s1("_s1",Float) = 0
			_p2("_p2",Float) = 0
			_s2("_s2",Float) = 0
			_p3("_p3",Float) = 0
			_s3("_s3",Float) = 0
			_t1("_t1",Float)=0
			_t2("_t2",Float) = 0
			_f1("_f1",Float) = 0
			_f2("_f2",Float) = 0
			_ti("_ti",Float) = 0
			_resy("_resy", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
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
				float _speed1;
				float _speed2;
				float _speed3;
				float _p1;
				float _s1;
				float _p2;
				float _s2;
				float _p3;
				float _s3;
				float _t1;
				float _t2;
				float _f1;
				float _f2;
				float _ti;
				float _resy;
				v2f vert(appdata v)
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
				float rd(float t) { return frac(sin(dot(floor(t), 45.26))*7845.236); }
				fixed4 frag(v2f i) : SV_Target
				{
					float ff = lerp(1280.,1920.,step(0.5,_f1)); 
					float ti = _ti*7.;
				// ti += lerp(0., _frame *0.25,step(0.9,rd(_frame*0.01)));
				float tt1 = lerp(0., 4., rd(ti*0.5));
					float tt2 = clamp(tt1, 1., 4.);
				//float tt1 = _t1;
					float rr = lerp(tt2, tt2/0.66, step(0.5, _f1));
					float2 ui = (i.uv+float2(0.,_t2))*float2(1920./ff ,1.)*rr;
					float ff1 = _frame / _speed1;
					float ff2 = _frame / _speed2;
					float ff3 = _frame / _speed3;
					float t1 = tex2D(_MainTex2,ui).x*step(i.uv.x,ff/1920. / rr)*step(distance(ui.y,0.5),0.5);
					float2 us = float2(ui.x, i.uv.y);
					float r1 = max(pow(rd(ti+452.),2.),0.1);
					float rr2 = max(pow(rd(ti + 9745.),0.5),0.1);
					float r2 = (1.-r1)*rr2;
					float r3 = (1.-r1) - r2;
					float rp1 = rd(ti+ 1267.);
					float st = step(1.,tt1);
					float pr1 =lerp(lerp(1., 1.25, rp1), lerp(1., tt2, rp1),st);
					float rpr2 = rd(ti + 6713.);
					float pr3 = lerp(pr1, pr1+tt2, rd(ti + 923.));
					float pr2 = lerp(lerp(0., 0.05/pr1, rpr2), (pr3-pr1)*0.25*rpr2,st);
					float ss1 = lerp(1.5,pr3,st) - pr1-pr2*2.;
					float s1 = r1 *ss1;
					float s2 = r2 *ss1;
					float s3 = r3 *ss1;					

					float p1 = pr1 ;
					float p2 = pr1 +s1+pr2 ;
					float p3 = pr1+s1+s2+pr2*2. ;
					float pp1 = (p1 / s1);
					float pp2 = (p2 / s2);
					float pp3 = (p3 / s3);

					float t2 = tex2D(_MainTex,us*float2(1./s1,1.)-float2(pp1,0.)).x*step(p1,ui.x)*step(ui.x, p1+s1);
					float u2 = tex2D(_MainTex, us*float2(1. / s2, 1.) - float2(pp2, 0.)).y*step(p2, ui.x)*step(ui.x, p2 + s2);
					float v2 = tex2D(_MainTex, us*float2(1. / s3, 1.) - float2(pp3, 0.)).z*step(p3, ui.x)*step(ui.x, p3 + s3);
					
					float ts = 0.5 / rr - _t2;

					float c1 = step(distance(ui.x, frac(ff1)),1. / 1920.)*step(distance(ts,i.uv.y),5. / 1080.) +
					step(distance(ui.x,frac( ff1)), 8.88 / 1920.)*step(distance(ts, i.uv.y), 0.5625 / 1080.);

					float c2 = step(distance(ui.x, frac(ff2)), 1. / 1920.)*step(distance(ts, i.uv.y), 5. / 1080.) +
						step(distance(ui.x, frac(ff2)), 8.88 / 1920.)*step(distance(ts, i.uv.y), 0.5625 / 1080.);

					float c3 = step(distance(ui.x, frac(ff3)), 1. / 1920.)*step(distance(ts, i.uv.y), 5. / 1080.) +
						step(distance(ui.x, frac(ff3)), 8.88 / 1920.)*step(distance(ts, i.uv.y), 0.5625 / 1080.);

					float l1 = smoothstep(0.0015,0.,li(us, float2(1.,ts), float2(lerp(p1, p1+s1,frac( ff1)), frac(floor(ff1)*10. / _resy) - 5. / _resy)));
					float l2 = smoothstep(0.0015, 0., li(us, float2(1., ts), float2(lerp(p2, p2 + s2, frac(ff2)), frac(floor(ff2)*10. / _resy) - 5. / _resy)));
					float l3 = smoothstep(0.0015, 0., li(us, float2(1., ts), float2(lerp(p3, p3 + s3, frac(ff3)), frac(floor(ff3)*10. / _resy) - 5. / _resy)));

					float t3 = t1+t2+u2+v2 ;
					float t4 = pow(lerp(t3, 1. - t3, c1+c2+c3 + l1 + l2 + l3),_f2);
				return float4(t4,t4,t4, 1.);
				}
				ENDCG
			}
		}
}
