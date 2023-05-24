using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using neoludicGames.uDmx;
public class contorlLight : MonoBehaviour
{
    public DmxLightSource l1;
    public DmxLightSource l2;
    public DmxLightSource l3;
    public DmxLightSource l4;
    public Timer script;
    public float speed;
    public float speed2;
    private bool l1Changed = false;
    //private bool l2Changed = false;
    public float vl1;
    public float prevl1;
    public float v1;
    public float v2;
   // public float prevl2;
   // private bool l3Changed = false;
   // private bool l4Changed = false;
    void Start()
    {
        
    }

    float fract(float t) { return t - Mathf.Floor(t); }
    float rd(float x) { float fx = Mathf.Floor(x); return fract(Mathf.Sin(Vector2.Dot(new Vector2(fx, fx), new Vector2(54.56f, 54.56f))) * 7845.236f); }
    float no(float x) { return Mathf.Lerp(rd(x), rd(x + 1), Mathf.SmoothStep(0, 1, fract(x))); }

    void Update()
    {
        if (script.ds1 == 0)
        {
            if (vl1 != prevl1)
            {
                if (!l1Changed)
                {
                    l1.SetStrength(1);
                    l2.SetStrength(0);
                    l3.SetStrength(1);
                    l4.SetStrength(0);
                    l1Changed = true;
                    prevl1 = vl1;
                }
                else
                {
                    l1.SetStrength(0);
                    l2.SetStrength(1);
                    l3.SetStrength(0);
                    l4.SetStrength(1);
                    l1Changed = false;
                    prevl1 = vl1;
                }
            }
        }
        /*  if (rd(Time.time * speed + 985) > 0.5)
          {
              if (!l2Changed)
              {
                  l2.SetStrength(0);
                  l2Changed = true;
              }
          }
          else
          {
              if (l2Changed)
              {
                  l2.SetStrength(1);
                  l2Changed = false;
              }
          }   */
        /*  if (rd(Time.time * speed2 + 458) > 0.5)
          {
              if (!l3Changed)
              {
                  l3.SetStrength(0);
                  l3Changed = true;
              }
          }
          else
          {
              if (l3Changed)
              {
                  l3.SetStrength(1);
                  l3Changed = false;
              }
          }
          if (rd(Time.time * speed2 + 265) > 0.5)
          {
              if (!l4Changed)
              {
                  l4.SetStrength(0);
                  l4Changed = true;
              }
          }
          else
          {
              if (l4Changed)
              {
                  l4.SetStrength(1);
                  l4Changed = false;
              }
          } */
    }

    
}
