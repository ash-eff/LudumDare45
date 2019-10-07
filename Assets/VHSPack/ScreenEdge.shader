Shader "VHS/ScreenEdge"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0, .5)) = 0
	}
	SubShader
	{
		Cull Off 
		ZWrite Off 
	    ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _DisplacementTex;
			float _Magnitude;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0; 
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				half4 n = tex2D(_DisplacementTex, i.uv);
				half4 d = n * 2 - 1;
				i.uv += d * _Magnitude;
				i.uv = saturate(i.uv);
				
				float4 c = tex2D(_MainTex, i.uv);
				return c;
			}
			ENDCG
		}
	}
}