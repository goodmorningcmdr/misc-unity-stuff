Shader "Hidden/Letterbox"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Offsets ("Top/Left (X) Bottom/Right (Y)", Vector) = (0, 0, 0, 0)
	}

	CGINCLUDE

		#include "UnityCG.cginc"

		sampler2D _MainTex;
		half2 _Offsets;
		static const half4 _FillColor = half4(0, 0, 0, 0);

		half4 frag_letter(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half cond = saturate(step(i.uv.y, _Offsets.x) + step(_Offsets.y, i.uv.y));
			color.rgb = lerp(color.rgb, _FillColor.rgb, cond * _FillColor.a);
			return color;
		}

		half4 frag_pillar(v2f_img i) : SV_Target
		{
			half4 color = tex2D(_MainTex, i.uv);
			half cond = saturate(step(i.uv.x, _Offsets.x) + step(_Offsets.y, i.uv.x));
			color.rgb = lerp(color.rgb, _FillColor.rgb, cond * _FillColor.a);
			return color;
		}

	ENDCG

	SubShader
	{
		ZTest Always Cull Off ZWrite Off
		Fog { Mode off }

		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_letter
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}

		Pass
		{			
			CGPROGRAM

				#pragma vertex vert_img
				#pragma fragment frag_pillar
				#pragma fragmentoption ARB_precision_hint_fastest

			ENDCG
		}
	}
	FallBack off
}