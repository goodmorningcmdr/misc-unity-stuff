/*
The MIT License

Copyright (c) 2016 Jasper Houghton http://jasperhoughton.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

/* 
Based on The Preetham Model
http://www.cs.utah.edu/~shirley/papers/sunsky/sunsky.pdf

and implementation by Simon Wallner
http://www.simonwallner.at/projects/atmospheric-scattering
*/

Shader "Skybox/jSky"
{
	//-----------------------------------------------------------------------------
	// Properties
	//-----------------------------------------------------------------------------

	Properties
	{
		_turbidity("Turbidity", Range(1.0, 2.0)) = 1.5

		_reileigh("Reileigh", Range(0.0, 4.0)) = 2.0

		_mieCoefficient("Mie coefficient", Range(0.0, 1.0)) = 0.1

		_mieDirectionalG("Mie directional", Range(0.0, 1.0)) = 0.7

		_sunIntensity("Sun intensity", Float) = 1000.0
	}

	//-----------------------------------------------------------------------------
	// Includes - Thanks to allen for reminding me about the UnityCG.cginc values
	//-----------------------------------------------------------------------------

	CGINCLUDE

	#include "UnityCG.cginc"

	#pragma target 3.0
	//-----------------------------------------------------------------------------
	// Variables
	//-----------------------------------------------------------------------------

	half _turbidity;
	half _reileigh;
	half _mieCoefficient;
	half _mieDirectionalG;
	half _sunIntensity;

	static const half _refractiveIndex = 1.0003;
	static const half _moleculesPerUnit = 2.545E25;
	static const half _depolatizationFactor = 0.035;
	static const half3 _wavelength = half3(680E-9, 550E-9, 450E-9);
	static const half3 _kCoefficient = half3(0.686, 0.678, 0.666);
	static const half _vCoefficient = 4.0;
	static const half _rayleighZenithLength = 8.4E3;
	static const half _mieZenithLength = 1.25E3;
	static const float _sunAngularDiameterCos = 0.9997993194915;
	static const half _cutoffAngle = 1.61;
	static const float _pi  = 3.14159265358979323846264338327;

	//-----------------------------------------------------------------------------
	// Functions
	//-----------------------------------------------------------------------------

	half3 Uncharted2Tonemap(half3 x)
	{
		half A = 0.15;
		half B = 0.50;
		half C = 0.10;
		half D = 0.20;
		half E = 0.02;
		half F = 0.30;

		return ((x * (A * x + C * B) + D * E) / (x * (A * x + B) + D * F)) - E / F;
	}

	//-----------------------------------------------------------------------------
	// Vertex
	//-----------------------------------------------------------------------------

	struct v2f
	{
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 worldPos : TEXCOORD1;
	};

	v2f vert(appdata_img v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
		o.worldPos = mul(_Object2World, v.vertex).xyz;

		return o;
	}

	//-----------------------------------------------------------------------------
	// Fragment
	//-----------------------------------------------------------------------------

	half4 frag(v2f i) : COLOR
	{
		float3 vPos = normalize(i.worldPos - _WorldSpaceCameraPos);

		half sunE = _sunIntensity * max(0.0, 1.0 - exp(-((_cutoffAngle - acos(dot(_WorldSpaceLightPos0, half3(0.0, 1.0, 0.0)))) / 1.5)));

		// Extinction (absorbtion + out scattering)
		// Rayleigh coefficients
		half3 betaRayleigh = (8.0 * pow(_pi, 3.0) * pow(pow(_refractiveIndex, 2.0) - 1.0, 2.0) * (6.0 + 3.0 * _depolatizationFactor)) / (3.0 * _moleculesPerUnit * pow(_wavelength, half3(4.0, 4.0, 4.0)) * (6.0 - 7.0 * _depolatizationFactor)) * _reileigh;

		// Mie coefficients
		half c = (0.2 * _turbidity) * 10E-18;
		half3 betaMie = 0.434 * c * _pi * pow((2.0 * _pi) / _wavelength, half3(_vCoefficient - 2.0, _vCoefficient - 2.0, _vCoefficient - 2.0)) * _kCoefficient * _mieCoefficient;

		// Optical length
		// Cutoff angle at 90 to avoid singularity in next formula
		half zenithAngle = acos(max(0.0, dot(half3(0.0, 1.0, 0.0), vPos)));
		half sR = _rayleighZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / _pi), -1.253));
		half sM = _mieZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / _pi), -1.253));

		// Combined extinction factor	
		half3 Fex = exp(-(betaRayleigh * sR + betaMie * sM));

		// In scattering
		half cosViewSunAngle = dot(vPos, _WorldSpaceLightPos0);

		half rayleighPhase = (2.0 / (4.0 * _pi)) * (1.0 + pow(cosViewSunAngle * 0.5 + 0.5, 2.0));
		half3 betaRayleighTheta = betaRayleigh * rayleighPhase;

		half miePhase = (1.0 / (4.0 * _pi)) * ((1.0 - pow(_mieDirectionalG, 2.0)) / pow(1.0 - 2.0 * _mieDirectionalG * cosViewSunAngle + pow(_mieDirectionalG, 2.0), 1.5));
		half3 betaMieTheta = betaMie * miePhase;

		half3 Lin = pow(sunE * ((betaRayleighTheta + betaMieTheta) / (betaRayleigh + betaMie)) * (1.0 - Fex), half3(1.3, 1.3, 1.3));
		Lin *= lerp(half3(1.0, 1.0, 1.0), pow(sunE * ((betaRayleighTheta + betaMieTheta) / (betaRayleigh + betaMie)) * Fex, half3(1.0 / 2.0, 1.0 / 2.0, 1.0 / 2.0)), clamp(pow(1.0 - dot(half3(0.0, 1.0, 0.0), _WorldSpaceLightPos0), 5.0), 0.0, 1.0));

		// Night sky
		half theta = acos(vPos.y);
		half phi = atan2(vPos.y, vPos.x);
		half2 uv = half2(phi, theta) / half2(2.0 * _pi, _pi) + half2(0.5, 0.0);
		half3 L0 = half3(0.1, 0.1, 0.1) * Fex;

		// Sun disk
		half sundisk = smoothstep(_sunAngularDiameterCos, _sunAngularDiameterCos +0.00009, cosViewSunAngle);
		if (vPos.y > 0.0)
			L0 += (sunE * 19000.0 * Fex) * sundisk;

		// Composition
		half3 scatteringColor = (Lin + L0) * 0.03;
		scatteringColor += half3(0.0, 0.001, 0.0025) * 0.3;

		// Tonemapping
		half3 whiteScale = 1.0 / Uncharted2Tonemap(half3(1000.0, 1000.0, 1000.0));

		half3 tonemappedColor = Uncharted2Tonemap((log2(2.0 / pow(1.0, 4.0))) * scatteringColor);
		half3 finalColor = pow(tonemappedColor * whiteScale, half3(1.0 / 1.2, 1.0 / 1.2, 1.0 / 1.2));

		return half4(finalColor, 1.0);
	}

	ENDCG

	//-----------------------------------------------------------------------------
	// Subshader
	//-----------------------------------------------------------------------------

	Subshader
	{
		Tags{ "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	}
	Fallback off
}