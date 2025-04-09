Shader "Custom/TestSkydome"
{
    Properties
    {
        _tint ("Tint Color", Color) = (1, 1, 1, 1)
        _texture01 ("Texture 01", 2D) = "white" {}
		_xScroll("Scroll X", Range(-5, 5)) = 1
		_yScroll("Scroll Y", Range(-5, 5)) = 1
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Lambert alpha:fade nofog


        struct Input
        {
            float2 uv_texture01;
        };

		float4 _tint;
		sampler2D _texture01;
		float _xScroll;
		float _yScroll;

        void surf (Input IN, inout SurfaceOutput o)
        {
			_xScroll *= _Time;
			_yScroll *= _Time;

			fixed4 c = (tex2D(_texture01, IN.uv_texture01 + float2(_xScroll, _yScroll))) * _tint;
			o.Emission = c.rgb;
			o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
