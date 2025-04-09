Shader "Custom/TestWater"
{
    Properties
    {
        _tint ("Tint Color", Color) = (1, 1, 1, 1)
        _bump01 ("Texture 01", 2D) = "bump" {}
		_bump02("Texture 01", 2D) = "bump" {}
		_normalStrength("Normal Strength", Range(0, 1)) = 0
		_metallic("Metallic", Range(0, 1)) = 0
		_roughness("Roughness", Range(0, 1)) = 0
		_xScrollTex01("Texture 01 Scroll X", Range(-5, 5)) = 1
		_yScrollTex01("Texture 01 Scroll Y", Range(-5, 5)) = 1
		_xScrollTex02("Texture 02 Scroll X", Range(-5, 5)) = 1
		_yScrollTex02("Texture 02 Scroll Y", Range(-5, 5)) = 1
    }
    SubShader
    {

        CGPROGRAM
        #pragma surface surf Standard

        struct Input
        {
			float2 uv_bump01;
			float2 uv_bump02;
        };

		float4 _tint;
		sampler2D _bump01;
		sampler2D _bump02;
		half _metallic;
		half _roughness;
		half _normalStrength;
		float _xScrollTex01;
		float _yScrollTex01;
		float _xScrollTex02;
		float _yScrollTex02;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			_xScrollTex01 *= _Time;
			_yScrollTex01 *= _Time;
			_xScrollTex02 *= _Time;
			_yScrollTex02 *= _Time;

			float3 first = UnpackNormal(tex2D(_bump01, IN.uv_bump01 + float2(_xScrollTex01, _yScrollTex01))).rgb;
			float3 second = UnpackNormal(tex2D(_bump02, IN.uv_bump02 + float2(_xScrollTex02 / 2.0, _yScrollTex02 / 2.0))).rgb;
			o.Albedo = _tint;

			o.Normal = (first + second) / 2.0;
			//o.Normal = UnpackNormal(tex2D(_bump01, IN.uv_bump01 + float2(_xScroll, _yScroll)));
			o.Normal *= float3(_normalStrength, _normalStrength, 1);

			o.Metallic = _metallic;
			o.Smoothness = _roughness;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
