Shader "Unlit/webcam4cam"
{
    Properties
    {

	_MainTex4("Texture4", 2D) = "black" {}
	_Date("_Date",2D) = "black"{}
	_bl("_bl", 2D) = "black" {}
	_gui1("_gui1",2D) = "black"{}
	//_gui2("_gui2",2D) = "black"{}
	  _float1("_float1",Float) = 0
		_float2("_float2",Float) = 0
		_float3("_float3",Float) = 0
		  _float4("_float4",Float) = 0
		  _blur("_blur",Float) = 0
		  _step0to1("_step0to1", Range(0,1)) = 0
	    _step1to2("_step1to2", Range(0,1)) = 0
		_fondu("_fondu",Float) = 0
	_bluractivation("_bluractivation",Range(0,1)) = 0
	_powermodification("_powermodification",Range(0,1)) = 0
	_step2invert("_step2invert",Range(0,1)) = 0
	_dither("_dither",Range(0,1)) = 0
	[Toggle] _final("_final",Float) = 0
		_c1("_c1", Float) = 0
		_c2("_c2", Float) = 0
		_c3("_c3", Float) = 0
		_c4("_c4", Float) = 0
		/*_c5("_c5", Float) = 0
		_c6("_c6", Float) = 0
		_c7("_c7", Float) = 0*/
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            /*sampler2D _MainTex;
			sampler2D _MainTex2;
			sampler2D _MainTex3;*/
			sampler2D _MainTex4;
			sampler2D _bl;
			sampler2D _gui1;
			sampler2D _Date;
            float4 _MainTex4_ST;
			float _float1;
			float _float2;
			float _float3;
			float _float4;
			float _blur;
			float _step0to1;
			float _step1to2;
			float _bluractivation;
			float _powermodification;
			float _step2invert;
			float _dither;
			float _c1;
			float _c2;
			float _c3;
			float _c4;
			/*float _c5;
			float _c6;
			float _c7;*/
			float _final;
			float _fondu;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex4);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			float3 exclusion(float3 s, float3 d)
			{
				return s + d - 2.0 * s * d;
			}
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			float rd(float t) { return frac(sin(dot(floor(t*12.), 45.))*7845.)+0.01; }
			float rs(float t) { return frac(sin(dot(t, 45.269))*7845.236); }
			float hs(float2 uv ) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 +_Time.y*30.)*0.5+0.5; }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float no(float t) { return lerp(rd(t), rd(t + 1.), smoothstep(0., 1., frac(t))); }
			float rd2(float2 t, float ti) { return frac(sin(dot(floor(t), float2(45., 98.)))*(7845. + ti*12.)); }

			#define Pi 3.14159265359

            fixed4 frag (v2f i) : SV_target
            {
				float ut = 1./3.;
				float dt = 2. / 3.;
				float us1 = 1./6.;float us2 = 2./6.;float us3 = 3./6.;float us4 = 4./6.;float us5 = 5./6.;
				float rf1 = step(0.5, rd(954.+_c3));
				float rf2 = rd(98.+ _c3);
				//float rf3 = step(0.15, rd(91.+ _c3));
				float r6 = step(0.5, rd(672.+ _c3));
				float r7 = step(0.5, rd(983. + _c3));

				float2 uv = (i.uv);
				float u1 = step(0.5, uv.x);
				float u2 = step(0.5, uv.y);
				float uto = lerp(u1, 1. - u1, r7);
				float utn = lerp(u2, 1. - u2, r7);
				float2 coy = lerp(uv, float2(uv.x, frac(uv.y + 0.5)), r6);
				float2 cox = lerp(uv, float2(frac(uv.x + 0.5), uv.y), r6);
				float2 uv2 = lerp(uv,lerp(	lerp (cox,coy, r7),		lerp(lerp(cox, uv, utn), lerp(coy, uv, uto), rf1)	, step(2./3.,rf2)),step(1/6.,rf2)*_step0to1);
				float Directions = 16.0;
				float Quality = 4.0;
				float Size =( 1.+hs(uv+986.5)*0.5)*_blur;
				float2 Radius = Size / float2(1920., 1080.);
				float4 c = float4(0., 0., 0.,0.);
				for (float d = 0.0; d < Pi; d += Pi / Directions){
					for (float e = 1.0 / Quality; e <= 1.0; e += 1.0 / Quality){c += tex2D(_MainTex4, uv2 + float2(cos(d), sin(d))*Radius*e);}}
				c /= Quality * Directions - 15.;
				float4 b = tex2D(_MainTex4, uv2);
				float n1 = smoothstep(0.45, 0.55, no(_c2+98.))*_bluractivation;
				float n2 = smoothstep(0.45, 0.55, no(_c2 + 125.))*_bluractivation;
				float n3 = smoothstep(0.45, 0.55, no(_c2 + 78.))*_bluractivation;
				float n4 = smoothstep(0.45, 0.55, no(_c2 + 320.))*_bluractivation;
				float hc = lerp(0.5, hs(uv + 23.69), 0.2);
				float ca = clamp(ov(lerp(b.x, c.x,  n1), hc), 0, 1.);
				float cb = clamp(ov(lerp(b.y, c.y,  n2), hc), 0., 1.);
				float cc = clamp(ov(lerp(b.z, c.z, n3), hc), 0., 1.);
				float cd = clamp(ov(lerp(b.a, c.a, n4), hc), 0., 1.);
				//////////////
				float t = _c1;
				float4 t1 = float4(ca,cb,cc,cd);
				float4 t2 = float4(cb,cc,cd,ca);
				float4 t3 = float4(cc,cd,ca,cb);
				float4 t4 = float4(cd,ca,cb,cc);
				float l1 = step(0.5 / 1920., length(uv.x - 0.5));
				float l2 = step(0.5 / 1080., length(uv.y - 0.5));
				float rr1 = rd(65.+t);
				float r1 = step(0.5,rr1);
				float rr2 = rd( 45.+t);
				float r2 = step(0.5, rr2);
				float rr3 = rd( 78.+t);
				float r3 = step(dt, rr3);
				float r4 = step(0.5, rd( 78.+t));
				float bd = lerp(lerp(u1, 1. - u1, r2), 1., r3);
				float g1 = tex2D(_gui1, uv*float2(2., 1.)+float2(0.,0.077)).a*(1. - _final)*step(0.5, _powermodification)*bd;
				//modif01
				float4 tl1 =  lerp(t4,lerp(t3,lerp(t1,t2,step(0.75,rr2)),step(0.5,rr2)),step(0.25,rr2));
				float um1 = lerp(u1, u2, r7);

				float t23 = lerp(t2, t3, r1);
				float t24 = lerp(t2, t4, r1);
				float t34 = lerp(t3, t4, r1);
				float t12 = lerp(t1, t2, r1);
				float t13 = lerp(t1, t3, r1);
				float t14 = lerp(t4, t1, r1);
			
				float4 tm1 = lerp(t1,  lerp(t23,t4,r3),um1);
				float4 tm2 = lerp(t2,  lerp(t34,t1,r3),um1);
				float4 tm3 = lerp(t3, lerp(t14,t2,r3),um1);
				float4 tm4 = lerp(t4,  lerp(t12,t3,r3),um1);
				float4 tm5 = lerp(tm4,lerp(tm3,lerp(tm1,tm2,step(0.75,rr2)),step(0.5,rr2)),step(0.25,rr2));

				float utf = lerp(utn,uto,rf1); 
				float uf= lerp(u1,u2,rf1);
				float4 tn1 = lerp(lerp(t2, t34, uf), t1, utf);
				float4 tn2 = lerp(lerp(t3, t24, uf), t1, utf);
				float4 tn3 = lerp(lerp(t4, t23, uf), t1, utf);
				float4 tn111 =  lerp(lerp(tn1,tn2, r4), tn3, r3);
				float4 tn4 = lerp(lerp(t3, t14, uf), t2, utf);
				float4 tn5 = lerp(lerp(t4, t13, uf), t2, utf);
				float4 tn6 = lerp(lerp(t1, t34, uf), t2, utf);
				float4 tn22 =  lerp(lerp(tn4,tn5, r4), tn6, r3);
				float4 tn7 = lerp(lerp(t4, t12, uf), t3, utf);
				float4 tn8 = lerp(lerp(t1, t24, uf), t3, utf);
				float4 tn9 = lerp(lerp(t2, t14, uf), t3, utf);
				float4 tn33 =  lerp(lerp(tn7,tn8, r4), tn9, r3);
				float4 tn10 = lerp(lerp(t1, t23, uf), t4, utf);
				float4 tn11 = lerp(lerp(t2, t13, uf), t4, utf);
				float4 tn12 = lerp(lerp(t3, t12, uf), t4, utf);
				float4 tn44 =  lerp(lerp(tn10,tn11, r4), tn12, r3);
				float4 tnf = lerp(tn44,lerp(tn3,lerp(tn111,tn2,step(0.75,rr2)),step(0.5,rr2)),step(0.25,rr2));

				float uu1 = lerp(u1,1.-u1,r1);
				float uu2 = lerp(u1,1.-u1,r2);
				float uuv2 = lerp(u2,1.-u2, r4);
				float4 to1 = lerp(lerp(t1,t2,uu2),lerp(t3,t4,uu1),uuv2);
				float4 to2 = lerp(lerp(t1,t3,uu2),lerp(t2,t4,uu1),uuv2);
				float4 to3 = lerp(lerp(t1,t4,uu2),lerp(t2,t3,uu1),uuv2);

				float tof =lerp(to1,lerp(to2,to3,step(dt,rr3)),step(ut,rr3));

				float4 tf1 = lerp(tl1,lerp(tof,lerp(tm4, tnf, step(dt,rf2)), step(ut,rf2)), step(us1,rf2));

				float tll = 1.-lerp(1.,lerp(l1*l2, lerp(lerp(l1,l2,r7), lerp(max(l1,utn)*l2, l1 *max(l2, uto), rf1),step(dt,rf2)), step(ut,rf2)), step(us1,rf2));
				float g2 = lerp(g1, lerp(g1, lerp(g1, g1*u1, rf1),  step(dt,rf2)), step(us1,rf2));
				//////////
				float2 up = float2(frac(i.uv*4.)*float2(1., 4.)) - float2(0., 3.);
				up = (((up - 0.5)*2.)*1.1)*0.5 + 0.5;
				float tee = tex2D(_Date, up).x*(1.-u2);
				float2 up2 = float2(frac(i.uv*2.)*float2(1., 4.)) - float2(0., 3.3);
				up2 = (((up2 - 0.5)*2.)*2.2)*0.5 + 0.5;
				float tee2 = tex2D(_Date, up2).x*u2;
				float2 up3 = float2(frac(i.uv*2.)*float2(1., 4.)) - float2(-0.27, 3.3);
				up3 = (((up3 - 0.5)*2.)*2.2)*0.5 + 0.5;
				float tee4 = tex2D(_Date, up3).x*u2*(1.-u1);
				float tee3 = lerp(tee4,(tee2 + tee),_final);

				float4 pc = lerp(tl1,tf1, _step0to1);

				float pc1 = exclusion(pc.x, pc.y);
				float pc2 = exclusion(pc.x, pc.z);
				float pc3 = exclusion(pc.x, pc.a);
				float pc4 = exclusion(pc.y, pc.z);
				float pc5 = exclusion(pc.y, pc.a);
				float pc6 = exclusion(pc.z, pc.a);

				float rpc = rd2(95.,_c1/6.);float dds = 0.05/6.;
				float pcf = clamp(lerp(pc6,lerp(pc5,lerp(pc4,lerp(pc3,lerp(pc2,pc1,
					smoothstep(us5-dds,us5+dds,rpc)),smoothstep(us4-dds,us4+dds,rpc)),smoothstep(us3-dds,us3+dds,rpc)),smoothstep(us2-dds,us2+dds,rpc)),smoothstep(us1-dds,us1+dds,rpc)),0.,1.);
				float nv4 = smoothstep(0.4, 0.95, no(_c4*0.00007+152.))*_step2invert;
				float pcff = lerp(smoothstep(0.8, 0.1, pow(pcf, .5)), smoothstep(0.1, 0.9, pow(pcf, 1.)),1.-nv4);

				float c2 = lerp(pc.x, pcff, _step1to2) ;
				float c3 = pow(c2,lerp(1.,lerp(0.75,1.5,no(_c4*0.0001)*0.+0.5),_powermodification));
				float c4 = step(hn(uv + 98.), (c3)*_float4)*no(_c4*0.0001 + 95.24)*_dither;

				float3 b1 = lerp(c3+c4, float3(0., 0., 1.), lerp(step(0.75,rd2(uv*4.,_c3*6.25))*(1.-u2),step(0.75,rd2(uv*2.,_c3*6.25))*u2,step(0.75,rr1))*_final);
				float lf = max((1. - step(0.5 / 1920.,min(length(uv.x - 0.5) , (u2)+length(abs(uv.x - 0.5) - 0.25)))), 1. - step(0.5 / 1080., min(length(uv.y - 0.5),length(uv.y - 0.25))));
				float3 b2 = lerp(b1, float3(1., 1., 1.),max(_final* lf,tll* _step0to1)+g2);
				
                return float4(lerp(lerp(smoothstep(1. - _fondu, 1., b2), b2, _fondu),float3(1.,1.,1.),tee3)*_fondu,1.);
            }
            ENDCG
        }
    }
}
