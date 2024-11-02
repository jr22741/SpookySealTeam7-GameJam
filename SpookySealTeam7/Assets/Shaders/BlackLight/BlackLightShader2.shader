Shader "Custom/BlackLightShader2"
{
    Properties
    {
        MyColor("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        MyGlossiness("Smoothness", Range(0,1)) = 0.5
        MyMetallic("Metallic", Range(0,1)) = 0.0
        MyLightDirection("Light Direction", Vector) = (0,0,1)
        MyLightPosition("Light Position", Vector) = (0,0,0)
        MyLightAngle("Light Angle", Range(0,180)) = 45
        MyStrengthScaler("Strength", Float) = 50
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half   MyGlossiness;
        half   MyMetallic;
        fixed4 MyColor;
        float3 MyLightPosition;
        float3 MyLightDirection;
        float  MyLightAngle;
        float  MyStrengthScaler;

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input input, inout SurfaceOutputStandard o)
        {
            float3 Dir      = normalize(MyLightPosition - input.worldPos);
            float  Scale    = dot(Dir, normalize(MyLightDirection));
            float  Strength = Scale - cos(MyLightAngle * UNITY_PI / 180.0);
            Strength        = clamp(Strength * MyStrengthScaler, 0, 1);
            fixed4 RC       = tex2D(_MainTex, input.uv_MainTex) * MyColor;
            o.Albedo        = RC.rgb;
            o.Emission      = RC.rgb * RC.a * Strength;
            o.Metallic      = MyMetallic;
            o.Smoothness    = MyGlossiness;
            o.Alpha         = Strength * RC.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}