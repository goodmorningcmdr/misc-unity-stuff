Shader "Particles/Master" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_Brightness ("Brightness", Range(-10.0,10.0)) = 0
	_MainSpeed ("Main Offset Speed", Range(-100.0,100.0)) = 0
	_MainTex ("Particle Texture", 2D) = "white" {}
	_ModSpeed ("Mod Offset Speed", Range(-100.0,100.0)) = 0
	_SecondaryTex ("Particle Modifier", 2D) = "white" {}
	_MaskSpeed ("Mask Offset Speed", Range(-100.0,100.0)) = 0
	_Mask ("Particle Mask", 2D) = "white" {}
	_SecondaryMask ("(Optional) Secondary Particle Mask", 2D) = "white" {}

	_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha One
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _SecondaryTex;
			sampler2D _SecondaryMask;

			sampler2D _Mask;

			fixed4 _TintColor;

			
			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD2;
				float2 texcoord4 : TEXCOORD3;

			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float2 texcoord2 : TEXCOORD2;
				float2 texcoord4 : TEXCOORD3;

				UNITY_FOG_COORDS(1)
				#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD4;
				#endif
			};
			
			float4 _MainTex_ST;
			float4 _SecondaryTex_ST;
			float4 _Mask_ST;
			float4 _SecondaryMask_ST;
			float _InvFade;
			float _ModSpeed;
			float _MaskSpeed;
			float _Brightness;
			float _MainSpeed;


			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.texcoord2 = TRANSFORM_TEX(v.texcoord2, _SecondaryTex);
				o.texcoord4 = TRANSFORM_TEX(v.texcoord4, _Mask);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;



			
			fixed4 frag (v2f i) : SV_Target
			{
				#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
				#endif

				float2 modcoord;
				modcoord.x = i.texcoord2.x;
				modcoord.y = i.texcoord2.y + (_ModSpeed * _Time);

				float2 maskcoord;
				maskcoord.x = i.texcoord4.x;
				maskcoord.y = i.texcoord4.y +(_MaskSpeed * _Time);

				float2 maincoord;
				maincoord.x = i.texcoord.x;
				maincoord.y = i.texcoord.y +(_MainSpeed * _Time);

				fixed4 col = (tex2D(_MainTex, maincoord) * tex2D(_SecondaryTex, modcoord))*2 ;
				col *=  i.color * _Brightness * _TintColor;
				col.a =  i.color.a ;
				col.a *= tex2D(_Mask, maskcoord).a * tex2D(_SecondaryMask, i.texcoord).a;
				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode
				return col;
			}
			ENDCG 
		}
	}	
}
}
