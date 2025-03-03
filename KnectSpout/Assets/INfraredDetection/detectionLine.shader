Shader "Unlit/detectionLine"
{
    Properties
    {
        _Tex ("_tex", 2D) = "black" {}
	_bl("bl", 2D) = "black" {}
		//_Tex2("_Tex2", 2D) = "white" {}
	_dither("_dither",Float)=0
		_c4("_c4", Float) = 0
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

            sampler2D _Tex;
			sampler2D _bl;
            float4 _Tex_ST;
			float _dither;
			float _c4;
            float3 _pos[17];
			float3 _pos2[17];
			float3 _pos3[17];


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _Tex);
                return o;
            }
			float hs(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return sin(tex2D(_bl, u).x*6.2831853071 + _Time.y*30.)*0.5 + 0.5; }
			float hn(float2 uv) { float2 u = uv * float2(1920., 1080.) / 1024.; return (tex2D(_bl, u).x); }
			float ov(float a, float b) {
				return a > 0.5 ? 2.*a*b : 1. - 2.*(1. - a)*(1. - b);
			}
			float rd(float t) { return frac(sin(dot(floor(t*12.), 45.))*7845.) + 0.01; }
			float no(float t) { return lerp(rd(t), rd(t + 1.), smoothstep(0., 1., frac(t))); }
float4 rd4(float t){float ft = floor(t); return frac(sin(float4(dot(ft,45.236),dot(ft,98.147),dot(ft,23.15),dot(ft,67.19)))*7845.236);}
            
      float spo8(float2 v[8], float2 p)
      {
          float d = dot(p - v[0], p - v[0]);
          float s = 1.0;
          for (int i = 0, j = 7; i < 8; j = i, i++)
          {
              float2 e = v[j] - v[i];
              float2 w = p - v[i];
              float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
              d = min(d, dot(b, b));
              bool3 c = bool3(p.y >= v[i].y, p.y < v[j].y, e.x * w.y > e.y * w.x);
              if (all(c) || all(!c))
                  s *= -1.0;
          }
          return s * sqrt(d);
      }
      float spo5(float2 v[5], float2 p)
      {
          float d = dot(p - v[0], p - v[0]);
          float s = 1.0;
          for (int i = 0, j = 4; i < 5; j = i, i++)
          {
              float2 e = v[j] - v[i];
              float2 w = p - v[i];
              float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
              d = min(d, dot(b, b));
              bool3 c = bool3(p.y >= v[i].y, p.y < v[j].y, e.x * w.y > e.y * w.x);
              if (all(c) || all(!c))
                  s *= -1.0;
          }
          return s * sqrt(d);
      }
      float spo3(float2 v[3], float2 p)
      {
          float d = dot(p - v[0], p - v[0]);
          float s = 1.0;
          for (int i = 0, j = 2; i < 3; j = i, i++)
          {
              float2 e = v[j] - v[i];
              float2 w = p - v[i];
              float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
              d = min(d, dot(b, b));
              bool3 c = bool3(p.y >= v[i].y, p.y < v[j].y, e.x * w.y > e.y * w.x);
              if (all(c) || all(!c))
                  s *= -1.0;
          }
          return s * sqrt(d);
      }
      float spo4(float2 v[4], float2 p)
      {
          float d = dot(p - v[0], p - v[0]);
          float s = 1.0;
          for (int i = 0, j = 3; i < 4; j = i, i++)
          {
              float2 e = v[j] - v[i];
              float2 w = p - v[i];
              float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
              d = min(d, dot(b, b));
              bool3 c = bool3(p.y >= v[i].y, p.y < v[j].y, e.x * w.y > e.y * w.x);
              if (all(c) || all(!c))
                  s *= -1.0;
          }
          return s * sqrt(d);
      }
      float spo13(float2 v[13], float2 p)
      {
          float d = dot(p - v[0], p - v[0]);
          float s = 1.0;
          for (int i = 0, j = 12; i < 13; j = i, i++)
          {
              float2 e = v[j] - v[i];
              float2 w = p - v[i];
              float2 b = w - e * clamp(dot(w, e) / dot(e, e), 0.0, 1.0);
              d = min(d, dot(b, b));
              bool3 c = bool3(p.y >= v[i].y, p.y < v[j].y, e.x * w.y > e.y * w.x);
              if (all(c) || all(!c))
                  s *= -1.0;
          }
          return s * sqrt(d);
      }

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
                float4 c1 = tex2D(_Tex, i.uv);
			float r1 = c1.a + ov(c1.x, lerp(0.5, hs(uv + 23.69), 0.2));
			float r2 = pow(clamp(r1, 0., 1.), 1.);
			float r3 = lerp(r2,step(hn(uv + 98.), pow(r2,2.)),_dither*no(_c4*0.0001 + 95.24));
			//float n4 = smoothstep(0.4, 0.95, no(_c4*0.00007 + 152.))*_step2invert;
			//float pc6 = lerp(smoothstep(0.9, 0.1, pow(pc5, 0.5)), smoothstep(0.1, 0.9, pow(pc5, 2.)), 1. - n4);
            float2 re = float2(16. / 9.,1.);
            float2 fac = re;
			float d1 = 0.;
            float ligne =0.8;
            float zp = 0.00075;
            float2 uli = i.uv;
            float2 ulif = uli*re;
            float trp = 12.;
       
        float pof = 0.;float pof2 = 0.;float pof3 = 0.;

        if(max(_pos[0].z,max(max(_pos[11].z,_pos[10].z),max(_pos[5].z,_pos[6].z)))>0.4){
        float4 rpo  = rd4(_Time.y*trp);
        float4 rpo2 = rd4(_Time.y*trp+78.45);
        float4 rpo3 = rd4(_Time.y*trp+425.36);
          float2 p0 = _pos[0].xy *re;
            float2 p1 = _pos[9].xy *re;
            float2 p2 = _pos[10].xy *re;
            float2 p3 = _pos[13].xy *re;
            float2 p4 = _pos[14].xy *re;
            float2 p0s = _pos[0].xy *re;
            float2 p1s = _pos[5].xy *re;
            float2 p4s = _pos[6].xy *re;
            float2 p2s = _pos[7].xy *re;
            float2 p5s = _pos[8].xy *re;
            float2 p3s = _pos[9].xy *re;
            float2 p6s = _pos[10].xy *re;
            float2 p7s = _pos[11].xy *re;
            float2 p8s = _pos[12].xy *re;
            float2 p9s = _pos[13].xy *re;
            float2 p11s = _pos[14].xy *re;
            float2 p10s = _pos[15].xy *re;
            float2 p12s = _pos[16].xy *re;
      float2 pr0s[8];pr0s[0] = p1s;pr0s[1] = p2s;pr0s[2] = p3s;pr0s[3] = p7s;pr0s[4] = p6s;pr0s[5] = p5s;pr0s[6] = p4s;pr0s[7] = p0s;
      float2 pr2s[5];pr2s[0] = p0s;pr2s[1] = p3s;pr2s[2] = p10s;pr2s[3] = p12s;pr2s[4] = p6s;
      float2 pr3s[13];pr3s[0] =p0s;pr3s[1] = p1s;pr3s[2] = p2s;pr3s[3] = p3s;pr3s[4] = p7s;pr3s[5] = p9s;pr3s[6] = p10s;pr3s[7] = p12s;
      pr3s[8] =p11s;pr3s[9] = p8s;pr3s[10] = p6s;pr3s[11] = p5s;pr3s[12] = p4s;
      float2 pr4s[3];pr4s[0] = p1s;pr4s[1] = p2s;pr4s[2] = p3s;
      float2 pr5s[3];pr5s[0] = p4s;pr5s[1] = p5s;pr5s[2] = p6s;
      float2 pr6s[3];pr6s[0] = p7s;pr6s[1] = p8s;pr6s[2] = p10s;
      float2 pr7s[3];pr7s[0] = p8s;pr7s[1] = p11s;pr7s[2] = p12s;
      float2 pr8s[4];pr8s[0] = p1s;pr8s[1] = p7s;pr8s[2] = p8s;pr8s[3] = p4s;
      float2 pr9s[4];pr9s[0] = p1s;pr9s[1] = p10s;pr9s[2] = p12s;pr9s[3] = p4s;

      float ps1 = 0.;float ps2 = 0;float ps3 = 0.;float ps4 =0.;float ps5 = 0.;float ps6 = 0.;float ps7= 0.;float ps8 = 0.; float ps9 = 0.;
             if(rpo.x>ligne){ps1 = smoothstep(zp,0.,length(spo8(pr0s,ulif)));}
             if(rpo.y>ligne){ps2 = smoothstep(zp,0.,length(spo5(pr2s,ulif)));}
             if(rpo.z>ligne){ps3 = smoothstep(zp,0.,length(spo13(pr3s,ulif)));}
             if(rpo.w>ligne){ps4 = smoothstep(zp,0.,length(spo3(pr4s,ulif)));}
             if(rpo2.x>ligne){ps5 = smoothstep(zp,0.,length(spo3(pr5s,ulif)));}
             if(rpo2.y>ligne){ps6 = smoothstep(zp,0.,length(spo3(pr6s,ulif)));}
             if(rpo2.z>ligne){ps7 = smoothstep(zp,0.,length(spo3(pr7s,ulif)));}
             if(rpo2.w>ligne){ps8 = smoothstep(zp,0.,length(spo4(pr8s,ulif)));}
             if(rpo3.x>ligne){ps9 = smoothstep(zp,0.,length(spo4(pr9s,ulif)));}

      pof = ps1+ps2+ps3+ps4+ps5+ps6+ps7+ps8+ps9; 
      }
              if(max(_pos2[0].z,max(max(_pos2[11].z,_pos2[10].z),max(_pos2[5].z,_pos2[6].z)))>0.4){
        float4 rpo  = rd4(_Time.y*trp);
        float4 rpo2 = rd4(_Time.y*trp+78.45);
        float4 rpo3 = rd4(_Time.y*trp+425.36);
          float2 p0 = _pos2[0].xy *re;
            float2 p1 = _pos2[9].xy *re;
            float2 p2 = _pos2[10].xy *re;
            float2 p3 = _pos2[13].xy *re;
            float2 p4 = _pos2[14].xy *re;
            float2 p0s = _pos2[0].xy *re;
            float2 p1s = _pos2[5].xy *re;
            float2 p4s = _pos2[6].xy *re;
            float2 p2s = _pos2[7].xy *re;
            float2 p5s = _pos2[8].xy *re;
            float2 p3s = _pos2[9].xy *re;
            float2 p6s = _pos2[10].xy *re;
            float2 p7s = _pos2[11].xy *re;
            float2 p8s = _pos2[12].xy *re;
            float2 p9s = _pos2[13].xy *re;
            float2 p11s = _pos2[14].xy *re;
            float2 p10s = _pos2[15].xy *re;
            float2 p12s = _pos2[16].xy *re;
      float2 pr0s[8];pr0s[0] = p1s;pr0s[1] = p2s;pr0s[2] = p3s;pr0s[3] = p7s;pr0s[4] = p6s;pr0s[5] = p5s;pr0s[6] = p4s;pr0s[7] = p0s;
      float2 pr2s[5];pr2s[0] = p0s;pr2s[1] = p3s;pr2s[2] = p10s;pr2s[3] = p12s;pr2s[4] = p6s;
      float2 pr3s[13];pr3s[0] =p0s;pr3s[1] = p1s;pr3s[2] = p2s;pr3s[3] = p3s;pr3s[4] = p7s;pr3s[5] = p9s;pr3s[6] = p10s;pr3s[7] = p12s;
      pr3s[8] =p11s;pr3s[9] = p8s;pr3s[10] = p6s;pr3s[11] = p5s;pr3s[12] = p4s;
      float2 pr4s[3];pr4s[0] = p1s;pr4s[1] = p2s;pr4s[2] = p3s;
      float2 pr5s[3];pr5s[0] = p4s;pr5s[1] = p5s;pr5s[2] = p6s;
      float2 pr6s[3];pr6s[0] = p7s;pr6s[1] = p8s;pr6s[2] = p10s;
      float2 pr7s[3];pr7s[0] = p8s;pr7s[1] = p11s;pr7s[2] = p12s;
      float2 pr8s[4];pr8s[0] = p1s;pr8s[1] = p7s;pr8s[2] = p8s;pr8s[3] = p4s;
      float2 pr9s[4];pr9s[0] = p1s;pr9s[1] = p10s;pr9s[2] = p12s;pr9s[3] = p4s;

      float ps1 = 0.;float ps2 = 0;float ps3 = 0.;float ps4 =0.;float ps5 = 0.;float ps6 = 0.;float ps7= 0.;float ps8 = 0.; float ps9 = 0.;
             if(rpo.x>ligne){ps1 = smoothstep(zp,0.,length(spo8(pr0s,ulif)));}
             if(rpo.y>ligne){ps2 = smoothstep(zp,0.,length(spo5(pr2s,ulif)));}
             if(rpo.z>ligne){ps3 = smoothstep(zp,0.,length(spo13(pr3s,ulif)));}
             if(rpo.w>ligne){ps4 = smoothstep(zp,0.,length(spo3(pr4s,ulif)));}
             if(rpo2.x>ligne){ps5 = smoothstep(zp,0.,length(spo3(pr5s,ulif)));}
             if(rpo2.y>ligne){ps6 = smoothstep(zp,0.,length(spo3(pr6s,ulif)));}
             if(rpo2.z>ligne){ps7 = smoothstep(zp,0.,length(spo3(pr7s,ulif)));}
             if(rpo2.w>ligne){ps8 = smoothstep(zp,0.,length(spo4(pr8s,ulif)));}
             if(rpo3.x>ligne){ps9 = smoothstep(zp,0.,length(spo4(pr9s,ulif)));}

      pof2 = ps1+ps2+ps3+ps4+ps5+ps6+ps7+ps8+ps9; 
      }
              if(max(_pos3[0].z,max(max(_pos3[11].z,_pos3[10].z),max(_pos3[5].z,_pos3[6].z)))>0.4){
        float4 rpo  = rd4(_Time.y*trp);
        float4 rpo2 = rd4(_Time.y*trp+78.45);
        float4 rpo3 = rd4(_Time.y*trp+425.36);
          float2 p0 = _pos3[0].xy *re;
            float2 p1 = _pos3[9].xy *re;
            float2 p2 = _pos3[10].xy *re;
            float2 p3 = _pos3[13].xy *re;
            float2 p4 = _pos3[14].xy *re;
            float2 p0s = _pos3[0].xy *re;
            float2 p1s = _pos3[5].xy *re;
            float2 p4s = _pos3[6].xy *re;
            float2 p2s = _pos3[7].xy *re;
            float2 p5s = _pos3[8].xy *re;
            float2 p3s = _pos3[9].xy *re;
            float2 p6s = _pos3[10].xy *re;
            float2 p7s = _pos3[11].xy *re;
            float2 p8s = _pos3[12].xy *re;
            float2 p9s = _pos3[13].xy *re;
            float2 p11s = _pos3[14].xy *re;
            float2 p10s = _pos3[15].xy *re;
            float2 p12s = _pos3[16].xy *re;
      float2 pr0s[8];pr0s[0] = p1s;pr0s[1] = p2s;pr0s[2] = p3s;pr0s[3] = p7s;pr0s[4] = p6s;pr0s[5] = p5s;pr0s[6] = p4s;pr0s[7] = p0s;
      float2 pr2s[5];pr2s[0] = p0s;pr2s[1] = p3s;pr2s[2] = p10s;pr2s[3] = p12s;pr2s[4] = p6s;
      float2 pr3s[13];pr3s[0] =p0s;pr3s[1] = p1s;pr3s[2] = p2s;pr3s[3] = p3s;pr3s[4] = p7s;pr3s[5] = p9s;pr3s[6] = p10s;pr3s[7] = p12s;
      pr3s[8] =p11s;pr3s[9] = p8s;pr3s[10] = p6s;pr3s[11] = p5s;pr3s[12] = p4s;
      float2 pr4s[3];pr4s[0] = p1s;pr4s[1] = p2s;pr4s[2] = p3s;
      float2 pr5s[3];pr5s[0] = p4s;pr5s[1] = p5s;pr5s[2] = p6s;
      float2 pr6s[3];pr6s[0] = p7s;pr6s[1] = p8s;pr6s[2] = p10s;
      float2 pr7s[3];pr7s[0] = p8s;pr7s[1] = p11s;pr7s[2] = p12s;
      float2 pr8s[4];pr8s[0] = p1s;pr8s[1] = p7s;pr8s[2] = p8s;pr8s[3] = p4s;
      float2 pr9s[4];pr9s[0] = p1s;pr9s[1] = p10s;pr9s[2] = p12s;pr9s[3] = p4s;

      float ps1 = 0.;float ps2 = 0;float ps3 = 0.;float ps4 =0.;float ps5 = 0.;float ps6 = 0.;float ps7= 0.;float ps8 = 0.; float ps9 = 0.;
             if(rpo.x>ligne){ps1 = smoothstep(zp,0.,length(spo8(pr0s,ulif)));}
             if(rpo.y>ligne){ps2 = smoothstep(zp,0.,length(spo5(pr2s,ulif)));}
             if(rpo.z>ligne){ps3 = smoothstep(zp,0.,length(spo13(pr3s,ulif)));}
             if(rpo.w>ligne){ps4 = smoothstep(zp,0.,length(spo3(pr4s,ulif)));}
             if(rpo2.x>ligne){ps5 = smoothstep(zp,0.,length(spo3(pr5s,ulif)));}
             if(rpo2.y>ligne){ps6 = smoothstep(zp,0.,length(spo3(pr6s,ulif)));}
             if(rpo2.z>ligne){ps7 = smoothstep(zp,0.,length(spo3(pr7s,ulif)));}
             if(rpo2.w>ligne){ps8 = smoothstep(zp,0.,length(spo4(pr8s,ulif)));}
             if(rpo3.x>ligne){ps9 = smoothstep(zp,0.,length(spo4(pr9s,ulif)));}

      pof3 = ps1+ps2+ps3+ps4+ps5+ps6+ps7+ps8+ps9; 
      }
      float3 rr = float3(r3,r3,r3) + (pof+pof2+pof3)*float3(0.5,0.5,1.);
                return float4(rr,1.);
            }
            ENDCG
        }
    }
}
