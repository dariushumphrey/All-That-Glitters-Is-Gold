Shader "Custom/TestSkybox"
{
    Properties
    {
		_skybox("Skybox CUBEmap", CUBE) = "" {}
		_skyboxHorizon("Skybox Horizon CUBEmap", CUBE) = "" {}
        _skyColor ("Sky Color", Color) = (1,1,1,1)
		_skyHorizonColor ("Sky Horizon Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert nofog

        #pragma target 3.0

        samplerCUBE _skybox;
		samplerCUBE _skyboxHorizon;

        struct Input
        {
			float2 uv_skybox;
			float2 uv_skyboxHorizon;
			float3 worldRefl;
        };

        float4 _skyColor;
		float4 _skyHorizonColor;

        void surf (Input IN, inout SurfaceOutput o)
        {
			fixed4 a = texCUBE(_skybox, IN.worldRefl) * _skyColor;
			fixed4 b = texCUBE(_skyboxHorizon, IN.worldRefl) * _skyHorizonColor;
			o.Emission = (a + b);
			//o.Emission = (texCUBE(_skybox, IN.worldRefl) + texCUBE(_skyboxHorizon, IN.worldRefl)) * _skyColor;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
