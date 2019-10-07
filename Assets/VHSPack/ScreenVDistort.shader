Shader "VHS/ScreenVDistort"
{
	Properties
	{
		_Color("Tint", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_LineTex("Line Texture", 2D) = "white" {}
		_NumOfLines("Number of lines", int) = 2
		_LineThickness("Line thickness", Range(.001, 1)) = .001
		_Speed("Speed", Range(-2, 2)) = 1
		_Magnitude("Mag", Range(0, 10)) = 1
		_Scan("Scan", int) = 1
		_TimeValue("TimeValue", float) = 0
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
			float _Speed;
			float _LineThickness;
			float _Magnitude;
			int _Scan;
			float _TimeValue;

			float4 frag(v2f i) : SV_Target
			{
				if (_Scan == 1)
				{
					float disp = tex2D(_LineTex, i.uv_line).y;
					disp = ((disp * 2) - 1) * _Magnitude;

					float4 horizontalLines = i.uv_line.y * _NumOfLines + _TimeValue * _Speed;
					float2 pos = floor(frac(horizontalLines.y) + _LineThickness) * disp;
					float4 col = tex2D(_MainTex, i.uv + pos);

					return col;
				}
				else
				{
					return tex2D(_MainTex, i.uv);
				}

			}
			ENDCG
		}
	}
}
