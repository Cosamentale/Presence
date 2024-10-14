Shader "Unlit/UIborder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}     // Main texture for the UI
        _Color ("Tint Color", Color) = (1,1,1,1)  // Tint color, including alpha for transparency
        _BorderColor ("Border Color", Color) = (1,1,1,1) // White border color
        _BorderWidth ("Border Width (px)", Float) = 2  // Border width (2px)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            Lighting Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;  // To get screen-space position
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _BorderColor;
            float _BorderWidth;  // Border width in pixels

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Pass the screen position for pixel-based calculations
                o.screenPos = o.vertex.xy / o.vertex.w;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Fetch the texture color
                float4 texColor = tex2D(_MainTex, i.uv) * _Color;

                // Get screen dimensions
                float screenWidth = _ScreenParams.x;
                float screenHeight = _ScreenParams.y;

                // Calculate pixel position relative to the screen
                float2 pixelPos = i.screenPos * float2(screenWidth, screenHeight) * 0.5 + 0.5;

                // Calculate distance from the borders in pixel space
                float borderDistX = min(pixelPos.x, screenWidth - pixelPos.x);
                float borderDistY = min(pixelPos.y, screenHeight - pixelPos.y);
                float minDist = min(borderDistX, borderDistY);

                // If we're within the border region, apply the border color
                if (minDist < _BorderWidth)
                {
                    return _BorderColor;
                }

                // Otherwise, return the texture color
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Transparent"
}