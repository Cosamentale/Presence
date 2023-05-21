Shader "Unlit/DetecFrame"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "black" {}
		//_MainTex2("_MainTex2", 2D) = "white" {}
		_Date("Date", 2D) = "black" {}
		_Texte("Texte",2D)="black"{}
		_bl("_bl", 2D) = "black" {}
		_float1("_float1",Float) = 0
		_float2("_float2",Float) = 0
		_float3("_float3",Float) = 0
		_taille2("_taille2",Float) = 0
		_floatA("_floatA",Float) = 0
			_c1("_c1", Float) = 0
		_c2("_c2", Float) = 0
		_c3("_c3", Float) = 0
		_c4("_c4", Float) = 0
			_c6("_c6", Float) =0
			_c7("_c7", Float) =0

	_powermodification("_powermodification",Range(0,1)) = 0
	_dither("_dither",Range(0,1)) = 0
			
			_float4("_float4",Float) = 0
			_float5("_float5",Float) = 0
			_float6("_float6",Float) = 0
			_secondPhase("_secondPhase",Float) = 0
			_troisiemePhase("_troisiemePhase",Float) = 0
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
			//sampler2D _MainTex2;
			sampler2D _Date;
			sampler2D _bl;
			sampler2D _Texte;
            float4 _MainTex_ST;
			float _float1;
			float _float2;
			float _float3;
			float _taille2;
			float _floatA;
			float _c1;
			float _c2;
			float _c3;
			float _c4;
			float _c6;
			float _c7;
			float _powermodification;
			float _dither;
			float _float4;
			float _float5;
			float _float6;
			float _secondPhase;
			float _troisiemePhase;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			float rd(float t) { return frac(sin(dot(floor(t), 45.266))*7845.236 + _floatA*t); }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float rdn(float t) { return frac(sin(dot(floor(t), 45.269))*7845.236); }
			float no(float t) { return lerp(rdn(t), rdn(t + 1.), smoothstep(0., 1., frac(t))); }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float isEven(float number) {
				return number % 2. == 0. ? 1. : 0.;
			}
            fixed4 frag (v2f i) : SV_Target
            {
				float t2 = tex2D(_MainTex, i.uv).z;
				float ta = floor(t2*_taille2 + 2.);
				float tb = floor((t2*_taille2 + 2.)*(9. / 16.));
				float t1 = tex2D(_MainTex,i.uv).x;
				float r5 = step(2. / 3., rd(67.15));
				float r6 = step(0.5, rdn(_c3*45.));
				float r7 = step(0.5, rdn(_c3*74.));
				float r8 = step(0.9, rdn(_c3*32.));
				float d3 = step(0.01, _float3);
				float ver = isEven(tb)*step(2., tb)*d3*r5*(1.- _secondPhase);
				float t3 = lerp(t1, tex2D(_MainTex, i.uv + float2(0., lerp(0.05, -0.05, step(0.5, i.uv.y)))).x, ver);
				float t4 = pow(tex2D(_MainTex, frac(i.uv*2.)).y,1.);
				//float t5 = lerp(t3, t4, step(0.5, i.uv.x));
				float t6 = pow(tex2D(_MainTex, (lerp(float2(0.5, 0.5), float2(_float1, _float2), _float3) + (frac(i.uv*2.) - 0.5)*lerp(1., 0.25, _float3))).y, 1.5);
				float tb1 = lerp(tb,4., _troisiemePhase);
				float v2 = floor(rd( 78.26)*tb1)*0.5;
				float tta = distance(i.uv.y*tb1, -.5*step(0.1, frac(v2)) + max(floor(rd(98.45)*tb1), 1.));
				float ttb = step(v2, tta)*d3;
				float ttl = step(tta, v2 +4./1080.)*ttb;
				float t8 = lerp(pow(tex2D(_MainTex, i.uv).y, 1.5) ,t1,ttb);
				float t7 = lerp(lerp(t3, lerp(t6,t4, lerp(step(0.5, i.uv.x), step(i.uv.x, 0.5), r7)), step(0.5, i.uv.y)), t8, _troisiemePhase);
				float rr = rd(45.56);
				float sp = floor(lerp(0.5, 1., rr)*ta);
				float sp2 = floor(lerp(0.5, 1.,rr)*tb);
				float r1 = step(sp,i.uv.x*ta);
				float r2 = step(sp2, i.uv.y*tb);
				
				float2 us = (i.uv - 0.5)*2.;
				us.x -= (sp / ta);
				float2 uq = us;
				us.y -= 0.85;
				//us = us * 3.;
				//us.y -= clamp(max(3., ta / (ta - sp)) - 3., 0., 1.)*0.5;
				//us *= 3.;
				us *= float2(1.1,4.4)*clamp( ta / (ta - sp)*2. - 4., 4., 10.);				
				us = (us*0.5) + 0.5;
				uq.y += 0.85;
				uq *= clamp(  ta/(ta - sp)*2. - 6., 6., 10.);
				uq = (uq*0.5) + 0.5;
				float te = tex2D(_Date, us).x;
				float l1 = step(distance(i.uv.x*ta, sp), ta*.5/1920.);
				float f1  = clamp(ov(lerp(t3,t7, _secondPhase), lerp(0.5, hs(i.uv + 23.69), 0.1)), 0, 1.);
				float f2 = pow(f1, lerp(1.5, lerp(1.25, 1.75, no(_c4*0.0001)), _powermodification));
				float f3 = step(hn(i.uv + 98.), (f2)*_float4)*no(_c4*0.0001 + 95.24)*_dither;
				float mv4 = tex2D(_Texte, uq).x*d3;
				float3 c1 = lerp(f2+f3,lerp(float3(0., 0., 1.),float3(1.,1.,1.),l1+ te * step(ta / (ta - sp), 6.)+mv4), r1*step(0.01,_float3));
				float2 ur = i.uv * float2(4.,16.);
				ur.x -= 2.95;
				ur.y -= lerp((sp2 / tb)*16., 7.375, r5);
				//ur.y -= 7.5;
				float te2 = tex2D(_Date, ur).x*d3;
				float l2 = step(distance(i.uv.y*tb, sp2), tb*0.5 / 1080.);
				float2 un = float2((i.uv.x - .5)*2.,i.uv.y);
				un.y -= (sp2 / tb);
				un.y += 0.0475;
				un *= 6.;
				un.x = un.x * 0.5 + 0.5;
				float mv3 = tex2D(_Texte, un).x*d3;
				float3 c2 = lerp(f2+f3,lerp(float3(0.,0.,1.),float3(1.,1.,1.),te2+l2+mv3), r2*d3*step(2.,tb));
				float3 c3 = lerp(c1, c2, step(0.5, rd(72.6)));
				float mv2 = tex2D(_Texte, (((i.uv-.5)*2.)*6.)*0.5+0.5).x*d3;
				float l3 = step(length(abs(i.uv.y - 0.5) - 0.05), 1./1080.);
				float3 c4 = lerp(f3+f2, lerp(float3(0., 0., 1.),float3(1.,1.,1.),te2*ver+l3+mv2), step(length(i.uv.y - 0.5),0.05)*ver);				
				float3 c5 = lerp(c3, c4, r5);
				float bl4 = step(min(length(abs(i.uv.x - 0.5) - 0.25), length(i.uv.x - 0.5)), 0.5 / 1920.);
				float cl4 = step(min(length(i.uv.y - 0.5), length(i.uv.y - 0.25)), 0.5 / 1080.);
				float ml4 = max(step(i.uv.y, 0.5), step(length(i.uv.x - 0.5), 0.1));
				float l4 =max( bl4*ml4,cl4);
				float2 up = float2(frac(i.uv*4.)*float2(1.,4.))-float2(0.,3.);
				up = (((up-0.5)*2.)*1.1)*0.5+0.5;
				float tee = tex2D(_Date, up).x;
				float te3 = tee*step(i.uv.y,0.5);
				float2 um = float2(frac(i.uv*4.)) - float2(0., _float6);
				um = (((um - 0.5)*2.)*_float5)*0.5 + 0.5;
				float mv1 = tex2D(_Texte, um).x;
				float3 c6 = lerp(c5, lerp(lerp(f2 + f3,max(float3(0.,0.,1.),mv1*d3),step(lerp(i.uv.y,1.-i.uv.y,r8),0.5)*lerp(step(0.5,i.uv.x), step( i.uv.x,0.5),r6)
				),float3(1.,1.,1.), l4+te3), _secondPhase);
				float2 uu = ((i.uv - 0.5)*16.+float2(0.,2. ))*0.5 + 0.5;
				float mmv5 = step(tta,0.1);
				float mv5 = tex2D(_Texte, float2(uu.x,frac(uu.y))).x*mmv5;
				float cl5 = step(min(min(length(i.uv.y - 0.5), length(i.uv.y - 0.25)), length(i.uv.y - 0.75)), 0.5 / 1080.);
				float3 c7 = lerp(c6, lerp(max(f3+f2,max((max(bl4,cl5)*d3)*ttb,ttl)),1.,lerp(mv5*d3,tee,ttb)), _troisiemePhase);
                return float4(c7 ,1.);
            }
            ENDCG
        }
    }
}
