Shader "Custom/Pulsate"
{
	Properties 
	{
_Color("Main Color", Color) = (1,1,1,1)
_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
_BumpMap("Normalmap", 2D) = "bump" {}
_RimColor("Rim Color", Color) = (0.7202797,0,1,1)
_RimPower("Rim Power", Range(0,10) ) = 2.84
_PulseColor("Pulse Bulge Color", Color) = (0.4328358,0,0,1)
_PulseIntensity("Pulse Intensity", Range(0,5) ) = 5
_PulseRate("Pulse Rate", Range(0,5) ) = 1.27

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
#pragma target 2.0


float4 _Color;
sampler2D _MainTex;
sampler2D _BumpMap;
float4 _RimColor;
float _RimPower;
float4 _PulseColor;
float _PulseIntensity;
float _PulseRate;

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
				float4 color : COLOR;
float2 uv_MainTex;
float2 uv_BumpMap;
float3 viewDir;

			};

			void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
float4 Split0=v.color;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - float4( Split0.x, Split0.x, Split0.x, Split0.x);
float4 Multiply3=_PulseIntensity.xxxx * Invert0;
float4 Multiply2=_PulseRate.xxxx * _Time;
float4 Sin0=sin(Multiply2);
float4 Splat0=Sin0.w;
float4 Abs0=abs(Splat0);
float4 Multiply0=Abs0 * float4( v.normal.x, v.normal.y, v.normal.z, 1.0 );
float4 Multiply1=Multiply3 * Multiply0;
float4 Add0=Multiply1 + v.vertex;
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);
v.vertex = Add0;


			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Split0=IN.color;
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - float4( Split0.x, Split0.x, Split0.x, Split0.x);
float4 Multiply1=_PulseRate.xxxx * _Time;
float4 Sin0=sin(Multiply1);
float4 Splat0=Sin0.w;
float4 Abs0=abs(Splat0);
float4 Multiply0=Invert0 * Abs0;
float4 Lerp0=lerp(_Color,_PulseColor,Multiply0);
float4 Sampled2D0=tex2D(_MainTex,IN.uv_MainTex.xy);
float4 Multiply2=Lerp0 * Sampled2D0;
float4 Sampled2D1=tex2D(_BumpMap,IN.uv_BumpMap.xy);
float4 UnpackNormal0=float4(UnpackNormal(Sampled2D1).xyz, 1.0);
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=(1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx;
float4 Pow0=pow(Fresnel0,_RimPower.xxxx);
float4 Multiply3=_RimColor * Pow0;
float4 Master0_4_NoInput = float4(0,0,0,0);
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Multiply2;
o.Normal = UnpackNormal0;
o.Emission = Multiply3;
o.Specular = Sampled2D0.aaaa;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}