Shader "Custom/Animate UVs" {
	Properties {
		_Color ("Main Color", Color) = (1, 1, 1, 1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_HorizontalSpeed ("X Speed", Float) = 1
		_VerticalSpeed ("Y Speed", Float) = 1
		_Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType"="TransparentCutout" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert alphatest:_Cutoff
 
		sampler2D _MainTex;
		
		float4 _Color;
		float _HorizontalSpeed;
		float _VerticalSpeed;
 
		struct Input {
			float2 uv_MainTex;
		};
 
		void surf(Input IN, inout SurfaceOutput o) {
			fixed2 scrollUV = IN.uv_MainTex;
			fixed scrollX = _HorizontalSpeed * _Time.x;
			fixed scrollY = _VerticalSpeed * _Time.x;
			scrollUV += fixed2(scrollX, scrollY);
			half4 c = tex2D(_MainTex, scrollUV) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}