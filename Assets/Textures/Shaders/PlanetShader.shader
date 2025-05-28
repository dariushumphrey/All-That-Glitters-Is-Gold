Shader "Custom/PlanetShader"
{
    Properties
	{
		_planetTex("Planet Texture", 2D) = "white" {}
		_planetNormals("Planet Normals", 2D) = "bump" {}
		_cloudsTexture("Clouds Texture", 2D) = "white" {}
		_cloudStrength("Cloud Strength", Range(0, 3)) = 1
		_planetNormalStrength("Planet Normal Strength", Range(-1, 1)) = 1
		_rim("Rim Color", Color) = (0, 0.5, 0.5, 0.0)
		_rimPower("Rim Power", Range(0.5, 8)) = 3
		_xScroll("Scroll X", Range(-5, 5)) = 1
		_yScroll("Scroll Y", Range(-5, 5)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert nofog

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0


        struct Input
        {
			float2 uv_planetTex;
			float2 uv_planetNormals;
			float2 uv_cloudsTexture;
			float3 viewDir;
        };

		sampler2D _planetTex;
		sampler2D _planetNormals;
		sampler2D _cloudsTexture;
		float _cloudStrength;
		float _planetNormalStrength;
		float4 _rim;
		float _rimPower;
		float _xScroll;
		float _yScroll;
       

        void surf (Input IN, inout SurfaceOutput o)
        {
			_xScroll *= _Time;
			_yScroll *= _Time;

			half rim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
            fixed4 c = tex2D (_cloudsTexture, IN.uv_cloudsTexture + float2(_xScroll, _yScroll)) * _cloudStrength;

			o.Albedo = tex2D(_planetTex, IN.uv_planetTex) + c;
			o.Normal = UnpackNormal(tex2D(_planetNormals, IN.uv_planetNormals));
			o.Normal *= float3(_planetNormalStrength, _planetNormalStrength, 1);
			//o.Alpha = c.a;
			o.Emission = _rim.rgb * pow(rim, _rimPower);

        }
        ENDCG
    }
    FallBack "Diffuse"
}
