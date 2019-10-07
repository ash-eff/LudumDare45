Shader "VHS/ScreenTracking"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_NumOfLines("Number of lines", int) = 2
		_LineWidth("Line Width", Range(.95, .998)) = .998
		_Speed("Speed", Range(-1, 1)) = 1
		_Scan("Scan", int) = 1
		_TimeValue("Time Value", float) = 0
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

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_line : TEXCOORD1;
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_line : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_line = v.uv_line;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _LineTex;
			float _NumOfLines;
			float4 _Color;
			float _LineWidth;
			float _Speed;
			int _Scan;
			float _TimeValue;

			float4 frag(v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);
				
				if (_Scan == 1)
				{
					float4 horizontalLines = i.uv.y * _NumOfLines + (_TimeValue * _Speed);
					col *= floor(frac(horizontalLines.y + _LineWidth) + .001) + _Color;
				}

				return col;
			}
			ENDCG
		}
	}
}
