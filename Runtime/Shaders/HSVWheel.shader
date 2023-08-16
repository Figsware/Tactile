Shader "UI/HSVWheel"
{
    Properties
    {
        _Color ("Tint", Color) = (1,1,1,1)
        _Mode ("Mode", Int) = 0
        _H ("H", Range(0.0, 1.0)) = 0.0
        _S ("S", Range(0.0, 1.0)) = 1.0
        _V ("V", Range(0.0, 1.0)) = 1.0

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            #include "TactileShaderUtil.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            int _Mode;
            float _H;
            float _S;
            float _V;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = v.texcoord;

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 offset = IN.texcoord - (0.5, 0.5);

                float angle = atan2(-offset.x, -offset.y);
                angle += UNITY_PI;
                angle /= 2 * UNITY_PI;

                float dist = distance((0, 0), offset);
                dist *= 2;

                float h;
                float s;
                float v;

                switch (_Mode)
                {
                case 0:
                    h = angle;
                    s = _S;
                    v = _V;
                    break;

                case 1:
                    h = _H;
                    s = angle;
                    v = _V;
                    break;

                case 2:
                    h = _H;
                    s = _S;
                    v = angle;
                    break;

                case 3:
                    h = _H;
                    s = dist;
                    v = angle;
                    break;

                case 4:
                    h = angle;
                    s = _S;
                    v = dist;
                    break;

                case 5:
                    h = angle;
                    s = dist;
                    v = _V;
                    break;
                }

                float3 rgb = HSV2RGB(float3(h, s, v));
                fixed4 color = fixed4(rgb.xyz, 1);

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}