Shader "Custom/ClipShader(NoRim)"
{
	Properties 
	{
_Color("Main Color", Color) = (0.3006994,1,0,1)
_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
_BumpMap("Normalmap", 2D) = "bump" {}
_SpecularColor("Spec Color", Color) = (1,1,1,1)
_clipMask("Clip Mask", 2D) = "black" {}
_clipEdgeColor("Clip Edge Color", Color) = (1,0,0,1)
_clipDistance("Clip Amount", Range(-1,1.01) ) = 0.015
_clipEdgeDistance("Clip Edge Thickness", Range(0,1) ) = 0.043

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Transparent"
"IgnoreProjector"="False"
"RenderType"="Transparent"

		}

		
Cull Off
ZWrite On
ZTest LEqual
ColorMask RGBA
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


float4 _Color;
sampler2D _MainTex;
sampler2D _BumpMap;
float4 _SpecularColor;
sampler2D _clipMask;
float4 _clipEdgeColor;
float _clipDistance;
float _clipEdgeDistance;

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
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec) * s.Alpha;
c.a = s.Alpha;
return c;

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
				float2 uv_MainTex;
float2 uv_clipMask;
float2 uv_BumpMap;

			};

			void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Sampled2D1=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply4=_Color * Sampled2D1;
float4 Negative0= -_clipEdgeDistance.xxxx; 
 float4 Sampled2D0=tex2D(_clipMask,IN.uv_clipMask.xy);
float4 Splat0=Sampled2D0.x;
float4 Subtract0=Splat0 - _clipDistance.xxxx;
float4 Add1=Negative0 + Subtract0;
float4 Step0=step(float4( 0.0, 0.0, 0.0, 0.0 ),Add1);
float4 Multiply1=Multiply4 * Step0;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Step0;
float4 Multiply2=Invert0 * _clipEdgeColor;
float4 Add0=Multiply1 + Multiply2;
float4 Sampled2D2=tex2D(_BumpMap,IN.uv_BumpMap.xy);
float4 UnpackNormal0=float4(UnpackNormal(Sampled2D2).xyz, 1.0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
clip( Subtract0 );
o.Albedo = Add0;
o.Normal = UnpackNormal0;
o.Emission = Multiply2;
o.Specular = Sampled2D1.aaaa;
o.Gloss = _SpecularColor;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}