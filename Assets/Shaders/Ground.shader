Shader "Custom/Ground"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _LineColor ("Line color", Color) = (1,1,1,1)
        _WaterColor ("Water color", Color) = (1,1,1,1)
        _EmissionColor("Emission color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _LineThickness ("Line thickness", Float) = 0.1
        _LineScale ("Line scale", Float) = 0.1
        _WaterHeight ("Water height", Float) = 0.001
        
        
        }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
                float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        half _LineThickness;
        half _LineScale;
        half _WaterHeight;
        fixed4 _Color;
        fixed4 _LineColor;
        fixed4 _WaterColor;
        fixed4 _EmissionColor;


        

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            float3 localPos = IN.worldPos -  mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz;


            float ddxDist = length(float2(ddx(localPos.y),ddy(localPos.y)));
            ddxDist = sqrt(ddxDist);
            float yVal = localPos.y  * _LineScale;
            _LineThickness *= ddxDist;
            yVal = frac(yVal);
            yVal = smoothstep(0.5 - _LineThickness, 0.5, yVal) * (1 - smoothstep(0.5, 0.5 + _LineThickness, yVal));
            
            
            
            fixed4 c = lerp(_Color,_LineColor,yVal);

            c = lerp(_WaterColor,c,step(_WaterHeight,localPos.y));
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            o.Emission = _EmissionColor * yVal;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
