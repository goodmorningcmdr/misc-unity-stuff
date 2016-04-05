Shader "Custom/SurfaceCoverShader"
{
	Properties 
	{
_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
_BumpMap("Normalmap", 2D) = "bump" {}
_SpecularColor("Specular Color", Color) = (1,1,1,1)
_RimColor("Rim Color", Color) = (0.5524473,0,1,1)
_RimFalloff("Rim Falloff", Range(0,10) ) = 3
_CoverTex("Cover Texture", 2D) = "black" {}
_CoverLevel("Cover Level", Range(-5,5) ) = 1.3
_CoverNormalInfluance("Normal Map Influance", Range(-2,2) ) = -0.13
_CoverHeightVertexOffset("Cover Level offset for Vertex", Range(-2,2) ) = -0.46
_CoverVertexHeight("Vertex Height", Range(-5,5) ) = 0.38

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


sampler2D _MainTex;
sampler2D _BumpMap;
float4 _SpecularColor;
float4 _RimColor;
float _RimFalloff;
sampler2D _CoverTex;
float _CoverLevel;
float _CoverNormalInfluance;
float _CoverHeightVertexOffset;
float _CoverVertexHeight;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
float4 Multiply1=float4( s.Albedo.x, s.Albedo.y, s.Albedo.z, 1.0 ) * light;
float4 Splat0=light.w;
float4 Multiply0=float4( s.Gloss.x, s.Gloss.y, s.Gloss.z, 1.0 ) * Splat0;
float4 Multiply2=light * Multiply0;
float4 Add2=Multiply1 + Multiply2;
float4 Mask1=float4(Add2.x,Add2.y,Add2.z,0.0);
float4 Mask0=float4(0.0,0.0,0.0,s.Alpha.xxxx.w);
float4 Add1=Mask1 + Mask0;
return Add1;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}
			
			struct Input {
				float2 uv_CoverTex;
float3 sWorldNormal;
float2 uv_BumpMap;
float2 uv_MainTex;
float3 viewDir;

			};

			void vert (inout appdata_full v, out Input o) {
			
			UNITY_INITIALIZE_OUTPUT(Input,o);
float4 Add1=_CoverHeightVertexOffset.xxxx + _CoverLevel.xxxx;
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - Add1;
float4 UnpackNormal0=float4(UnpackNormal(float4( v.normal.x, v.normal.y, v.normal.z, 1.0 )).xyz, 1.0);
float4 Dot0=dot( UnpackNormal0.xyz, float4( 0,1,0,0).xyz ).xxxx;
float4 Step0=step(Invert1,Dot0);
float4 Multiply0=Step0 * float4( v.normal.x, v.normal.y, v.normal.z, 1.0 );
float4 Multiply1=Multiply0 * _CoverVertexHeight.xxxx;
float4 Add0=v.vertex + Multiply1;
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
v.vertex = Add0;

o.sWorldNormal = mul((float3x3)_Object2World, SCALED_NORMAL);

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D1=tex2D(_CoverTex,(IN.uv_CoverTex.xyxy).xy);
float4 Invert1= float4(1.0, 1.0, 1.0, 1.0) - _CoverLevel.xxxx;
float4 Tex2DNormal0=float4(UnpackNormal( tex2D(_BumpMap,(IN.uv_BumpMap.xyxy).xy)).xyz, 1.0 );
float4 Multiply1=Tex2DNormal0 * _CoverNormalInfluance.xxxx;
float4 Add1=float4( IN.sWorldNormal.x, IN.sWorldNormal.y,IN.sWorldNormal.z,1.0 ) + Multiply1;
float4 UnpackNormal0=float4(UnpackNormal(Add1).xyz, 1.0);
float4 Dot0=dot( UnpackNormal0.xyz, float4( 0,1,0,0).xyz ).xxxx;
float4 Step0=step(Invert1,Dot0);
float4 Multiply3=Tex2D1 * Step0;
float4 Tex2D0=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Multiply2=Tex2D0 * Invert0;
float4 Add0=Multiply3 + Multiply2;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,_RimFalloff.xxxx);
float4 Multiply0=_RimColor * Pow0;
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Add0;
o.Normal = Tex2DNormal0;
o.Emission = Multiply0;
o.Specular = Tex2D0.aaaa;
o.Gloss = _SpecularColor;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}