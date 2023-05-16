Shader "Unlit/Compo"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
		_MainTex2("Texture", 2D) = "black" {}
		_Text("_Text",2D) = "black"{}
		_bl("_bl", 2D) = "black" {}
		_p1("_p1",Float) = 0
		_p2("_p2",Float) = 0
		_p3("_p3",Float) = 0
		_p4("_p4",Float) = 0
		_frame("_frame",Float) = 0
		_speed1("_speed1",Float) = 0
		_speed2("_speed2",Float) = 0
		_speed3("_speed3",Float) = 0
		_resy("_resy", Float) = 0
		_state("_state",Float) = 0
		_phase2("phase2",Float) = 0
		_phase2st("phase2st", Float) = 0
		_phase2v("_phase2v",Float) = 0
		_phase3("_phase3",Float) = 0
			_phase3st("_phase3st",Float) = 0
			_timePhase32("_timePhase32",Float)= 0
			_timePhase31("_timePhase31",Float) = 0
		_phase3d("_phase3d",Float) = 0
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
			sampler2D _Text;
            float4 _MainTex_ST;
			float _p1;
			float _p2;
			float _p3;
			float _p4;
			float _frame;
			float _speed1;
			float _speed2;
			float _speed3;
			float _resy;
			float _state;
			float _phase2;
			float _phase2st;
			float _phase2v;
			float _phase3;
			float _phase3d;
			float _phase3st;
			float _timePhase32;
			float _timePhase31;
			sampler2D _bl;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float rd(float t) { return frac(sin(dot(floor(t), 45.236))*7845.236); }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float li(float2 uv, float2 a, float2 b) {
				float2 ua = uv - a; float2 ba = b - a;
				float h = clamp(dot(ua, ba) / dot(ba, ba), 0., 1.);
				return length(ua - ba * h);
			}
			float map(float low1, float low2 , float high1, float high2, float value) { return low2 + (value - low1) * (high2 - low2) / (high1 - low1); }
			float map2(float low1,  float high1, float value) { return  (value - low1)/ (high1 - low1); }
            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float ff1 = _frame / _speed1;
				float la = 0.5/1080.;
				float la2 = 2.*la;
				float ha = 0.5 / 1920.;
				float c = tex2D(_MainTex2, uv).a*max(step(length(uv.y - 0.5),.125), 1. - _state);
				float lv2 = step(0.4, uv.x);
				float c2 = lerp(lerp(tex2D(_MainTex, float2(frac(uv.y*4.), uv.x)).x, tex2D(_MainTex, float2(frac(uv.x*3.), uv.y)).x, _state),
					lerp(tex2D(_MainTex, float2(frac(uv.x*lerp(2.5,5.,lv2)), uv.y)).x, tex2D(_MainTex, float2(frac(uv.x*5.), uv.y)).y,step(0.6,uv.x)), _phase2v);
				float po = _p1;
				float c3 = lerp(c2, c, lerp(step(0.125,distance(uv.y ,_p1) ),step(1./6.,length(uv.x-0.5)),_state));
				float l1 = step(distance(uv.y, _p2), la);
				float l = smoothstep(la2, 0., li(uv, float2(frac(ff1), _p2), float2( frac(floor(ff1)*10. / _resy - 5. / _resy),lerp(_p1-0.125, _p1+0.125,frac( ff1))) ));
				float ll = smoothstep(la2, 0., li(uv, float2(frac(ff1), _p2), float2( lerp(0.5 -1./6., 0.5 + 1./6., frac(ff1)),frac(floor(ff1)*10. / _resy - 5. / _resy))));
				float ll1 = lerp(l, ll, _state);
				float l2 = lerp(step( min(distance(uv.y, _p1+0.125), distance(uv.y, _p1 - 0.125)),la),
				step(length(abs(uv.x-0.5)-1./6.),ha),_state);
				if (_phase2 >0.5) {
					float pl2 = lerp(0.75, 0.25, _phase2st );
					l2 += step(min(distance(uv.y, _p1 + pl2 + 0.125), distance(uv.y, _p1 +pl2 - 0.125)), la);
					c3 = lerp(c3, tex2D(_MainTex, float2(frac(uv.y*4.), uv.x)).y, step( distance(uv.y, 1. - _p1),0.125));
					l1 += step(distance(1.-uv.y, _p2), la);
					ll1 = smoothstep(la2, 0., li(uv, float2(frac(ff1),_p2), float2(frac(floor(ff1)*10. / _resy - 5. / _resy), lerp(_p1 - 0.125, _p1 + 0.125, frac(ff1)))));
					ll1 += smoothstep(la2, 0., li(uv, float2(frac(ff1),1.- _p2), float2(frac(floor(ff1)*10. / _resy - 5. / _resy), lerp( 1.-_p1 - 0.125,1.-  _p1 + 0.125, frac(ff1)))));
				}
				if (_phase2v > 0.5) {
					float mc3 = lv2 * step(0.1, length(uv.x - 0.7));
					c3 = lerp(c2, c, lerp(mc3,1.-mc3,_phase2st));
					float pp1 = lerp(_p1, _p2, _phase2st); float pp2 = lerp(_p2, _p1, _phase2st); float pp3 = lerp(_p3, _p4, _phase2st); float pp4 = lerp(_p4, _p3, _phase2st);
					float pll1 = 0.2 - _phase2st * 0.1;
					ll1 = smoothstep(la2, 0., li(uv, float2(pp2,frac(ff1)), float2( lerp(pp1 - pll1, pp1 + pll1, frac(ff1)), frac(floor(ff1)*10. / _resy - 5. / _resy))));
					ll1 += smoothstep(la2, 0., li(uv, float2(pp4, frac(ff1)), float2(lerp(pp3 - 0.1, pp3 + 0.1, frac(ff1)), frac(floor(ff1)*10. / _resy - 5. / _resy))));
					l1 = step(min(distance(uv.x, pp4), distance(uv.x, pp2 )), ha);
					l2 = max(step(length(frac(uv.x*5.+2.5)-0.5), 2.5/1920.),step(distance((uv-0.5)*2.,0.5),la2))*step(2./5.,uv.x)*step(uv.x-1./1920.,4./5.);					
				}
				float tex = +tex2D(_Text, i.uv*float2(3.5, 14.)).x*_state;
				float tex2 = +tex2D(_Text, i.uv*float2(3.5, 14.)-lerp(float2(0.,9.5), float2(2.5, 12.5), _phase3st)).x;
				float mc4 = step(uv.y, 0.8)*step(0.2, length(uv.y - 0.4));
				float c4 = c ;
				float buc = map2(0.2, 0.6, uv.y);
				float bh =step( length(buc - 0.5), 0.5);
				float duc = map2(0.3, 0.6, uv.x);
				float duc2 = map2(0.7, 1., uv.x);
				float2 uc5 = float2( buc, duc);
				float mc5 =bh*step(length(uc5.y - 0.5), 0.5);
				float c5 = tex2D(_MainTex, uc5).x*mc5;
				float2 uc6 = float2(buc ,duc2);
				float mc6 = bh*step(length(uc6.y - 0.5), 0.5);
				float c6 = tex2D(_MainTex, uc6).y*mc6;
				float c7 =max(c5, c6);
				float buc2 = map2(0.6, 0.8, uv.y);
				float bh2 = step(length(buc2 - 0.5), 0.5);
				float2 uc8 = float2(buc2, duc);
				float mc8 = bh2 * step(length(uc8.y - 0.5), 0.5);
				float c8 = tex2D(_MainTex, uc8).z*mc8;
				float2 uc9 = float2(buc2, duc2);
				float mc9 = bh2 * step(length(uc9.y - 0.5), 0.5);
				float c9 = tex2D(_MainTex, uc9).z*mc9;
				float c10 = max(c9, c8);
				float cf = lerp(c3,lerp(lerp(c4*mc4, c7,mc5+mc6),
					lerp(c4*lerp(1. - mc4,mc4,_phase3st) + tex2,c10+c7,mc8+mc9+mc5+mc6),
					_phase3d),  _phase3);
				float ff2 = _timePhase31 / _speed1;
				float ff3 = _timePhase32 / _speed1;
				float ll2 = smoothstep(la2, 0., li(uv, float2(frac(ff2), _p2), float2(lerp(0.3,0.6,frac(floor(ff2)*10. / _resy - 5. / _resy)), lerp(0.2, 0.6, frac(ff2)))));
				ll2 += smoothstep(la2, 0., li(uv, float2(frac(ff2), _p4), float2(lerp(0.7, 1., frac(floor(ff2)*10. / _resy - 5. / _resy)), lerp(0.2, 0.6, frac(ff2)))));
				float l3 = step(min(length(uv.y - _p2), length(uv.y- _p4)),0.5/1080.);
				float l4 = step(min(length(uv.y - _p1), length(uv.y - _p3)), 0.5 / 1080.);
				float ll3 = smoothstep(la2, 0., li(uv, float2(frac(ff3), _p1), float2(lerp(0.3,0.6, frac(floor(ff3)*10. / _resy - 5. / _resy)), lerp(0.6, 0.8, frac(ff3)))));
				ll3 += smoothstep(la2, 0., li(uv, float2(frac(ff3), _p3), float2(lerp(0.7, 1., frac(floor(ff3)*10. / _resy - 5. / _resy)), lerp(0.6, 0.8, frac(ff3)))));
				float lf = lerp(l1*0.2 + ll1*0.25 + l2,lerp(ll3*0.25 + l4*0.2, ll2*.25 + l3*0.2, _phase3st), _phase3);
				
                return float4(float3(1.,1.,1.)*cf+lf+tex,1.);
            }
            ENDCG
        }
    }
}
