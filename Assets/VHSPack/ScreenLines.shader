Shader "VHS/ScreenLines"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_NumOfLines("Number of lines", int) = 2
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _NumOfLines;
			float4 _Color;

			float4 frag(v2f i) : SV_Target
			{
				float4 horizontalLines = i.uv.y * (_ScreenParams.y / _NumOfLines);
				float4 verticalLines = i.uv.x * (_ScreenParams.x / _NumOfLines);
				float4 col = tex2D(_MainTex, i.uv);
				//* frac(verticalLines.x) 
				col *= frac(horizontalLines.y) + _Color;
				return col;
			}
			ENDCG
		}
	}
}