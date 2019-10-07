Shader "VHS/ScreenDistort"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DisplacementTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0, .09)) = 0
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
				float2 disp = tex2D(_DisplacementTex, i.uv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;
				float4 color = tex2D(_MainTex, i.uv + disp);
				return color;
			}
			ENDCG
		}
	}
}