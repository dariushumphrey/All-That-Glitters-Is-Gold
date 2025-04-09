Shader "Custom/ColorVF"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_SideTex ("Texture", 2D) = "white" {}
		_scalar("Scalar", Range(-5, 6)) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 color: COLOR;
            };

            sampler2D _MainTex;
			sampler2D _SideTex;
            float4 _MainTex_ST;
			float4 _SideTex_ST;
			float _scalar;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv.x = TRANSFORM_TEX(v.uv, _SideTex) * _scalar;
				//o.color.r = v.vertex.x + _scalar;
				//o.color.g = (v.vertex.z + _scalar) / _scalar;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				//fixed4 col;
				fixed4 col = tex2D(_MainTex, i.uv);
				//col = i.color;
				//col.r = i.vertex.x / 1000;
				//col.g = i.vertex.y / 1000;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
