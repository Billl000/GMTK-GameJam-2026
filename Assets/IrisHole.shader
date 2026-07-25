Shader "UI/IrisHole"
{
    Properties
    {
        _Color    ("Color", Color) = (0,0,0,1)
        _Radius   ("Radius", Range(0,1.5)) = 1.5
        _Aspect   ("Aspect", Float) = 1.777
        _Softness ("Edge Softness", Range(0,0.1)) = 0.005
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off  Cull Off  Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f     { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            fixed4 _Color;
            float _Radius, _Aspect, _Softness;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 d = i.uv - 0.5;
                d.x *= _Aspect;                 // keep the hole round, not oval
                float dist = length(d);
                // opaque outside the radius, transparent inside
                float a = smoothstep(_Radius, _Radius + _Softness, dist) * _Color.a;
                return fixed4(_Color.rgb, a);
            }
            ENDCG
        }
    }
}