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

// modified by Allen (alllen.tumblr.com) on March 31, 2016

Shader "Skybox/jSky/Atmospheric Scattering" //more descriptive name to make it easier to find
{
	//-----------------------------------------------------------------------------
	// Properties
	//-----------------------------------------------------------------------------

    Properties
    {
		_turbidity("Turbidity", Range(1.0, 2.0)) = 1.5

		_reileigh("Reileigh", Range(0.0, 4.0)) = 2.0

		_mieCoefficient("Mie Coefficient", Range(0.0, 1.0)) = 0.1

		_mieDirectionalG("Mie Directional", Range(0.0, 1.0)) = 0.7

		_sunIntensity("Sun Intensity", Float) = 1000.0

		[HideInInspector]
		_M_PI("pi", Float) = 3.14159265358979323846264338327

		[HideInInspector]
		_up ("up", Vector) = (0.0, 1.0, 0.0)

		[HideInInspector]
		_refractiveIndex("ri", Float) = 1.0003

		[HideInInspector]
		_moleculesPerUnit("mpu", Float) = 25450000000000000000000000

		[HideInInspector]
		_depolatizationFactor("depol", Float) = 0.035

		[HideInInspector]
		_wavelength ("wave", Vector) = (0.00000068, 0.00000055, 0.00000045)

		[HideInInspector]
		_kCoefficient ("kCoe", Vector) = (0.686, 0.678, 0.666)

		[HideInInspector]
		_vCoefficient("vCoe", Float) = 4.0

		[HideInInspector]
		_rayleighZenithLength("ray", Float) = 8400

		[HideInInspector]
		_mieZenithLength("zen", Float) = 1250

		[HideInInspector]
		_sunAngularDiameterCos("suncos", Float) = 0.9997993194915

		[HideInInspector]
		_cutoffAngle("cut", Float) = 1.6110731556870734556218684016769

		[HideInInspector]
		_steepness("steep", Float) = 1.5
	}

	//-----------------------------------------------------------------------------
	// Includes
	//-----------------------------------------------------------------------------

	CGINCLUDE

	#include "UnityCG.cginc"

	//-----------------------------------------------------------------------------
	// Variables
	//-----------------------------------------------------------------------------

	float _turbidity;
	float _reileigh;
	float _mieCoefficient;
	float _mieDirectionalG;
	float _sunIntensity;
	
	float _refractiveIndex;
	float _moleculesPerUnit;
	float _depolatizationFactor;
	float3 _wavelength;
	float3 _kCoefficient;
	float _vCoefficient;
	float _rayleighZenithLength;
	float _mieZenithLength;
	float _sunAngularDiameterCos;
	float _cutoffAngle;
	float _steepness;
	float3 _up;
	float _M_PI;

	//-----------------------------------------------------------------------------
	// Functions
	//-----------------------------------------------------------------------------

	float3 totalRayleigh(float3 lambda)
	{
		return (8.0 * pow(_M_PI, 3.0) * pow(pow(_refractiveIndex, 2.0) - 1.0, 2.0) * (6.0 + 3.0 * _depolatizationFactor)) / (3.0 * _moleculesPerUnit * pow(lambda, float3(4.0, 4.0, 4.0)) * (6.0 - 7.0 * _depolatizationFactor));
	}

	float rayleighPhase(float cosViewSunAngle)
	{
		return (2.0 / (4.0*_M_PI)) * (1.0 + pow(cosViewSunAngle, 2.0));
	}

	float3 totalMie(float3 lambda, float3 K, float T)
	{
		float c = (0.2 * T) * 10E-18;
		return 0.434 * c * _M_PI * pow((2.0 * _M_PI) / lambda, float3(_vCoefficient - 2.0, _vCoefficient - 2.0, _vCoefficient - 2.0)) * K;
	}

	float hgPhase(float cosViewSunAngle, float g)
	{
		return (1.0 / (4.0*_M_PI)) * ((1.0 - pow(g, 2.0)) / pow(1.0 - 2.0*g*cosViewSunAngle + pow(g, 2.0), 1.5));
	}

	float logLuminance(float3 c)
	{
		return log(c.r * 0.2126 + c.g * 0.7152 + c.b * 0.0722);
	}

	float3 Uncharted2Tonemap(float3 x)
	{
		float A = 0.15;
		float B = 0.50;
		float C = 0.10;
		float D = 0.20;
		float E = 0.02;
		float F = 0.30;

		return ((x*(A*x + C*B) + D*E) / (x*(A*x + B) + D*F)) - E / F;
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
		float ln = normalize(i.worldPos - _WorldSpaceLightPos0);

		float sunE = _sunIntensity * max(0.0, 1.0 - exp(-((_cutoffAngle - acos(dot(_WorldSpaceLightPos0, _up))) / _steepness)));

		// extinction (absorbtion + out scattering)
		// rayleigh coefficients
		float3 betaR = totalRayleigh(_wavelength) * _reileigh;

		// mie coefficients
		float3 betaM = totalMie(_wavelength, _kCoefficient, _turbidity) * _mieCoefficient;

		// optical length
		// cutoff angle at 90 to avoid singularity in next formula.
		float zenithAngle = acos(max(0.0, dot(_up, normalize(i.worldPos - _WorldSpaceCameraPos))));
		float sR = _rayleighZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / _M_PI), -1.253));
		float sM = _mieZenithLength / (cos(zenithAngle) + 0.15 * pow(93.885 - ((zenithAngle * 180.0) / _M_PI), -1.253));

		// combined extinction factor	
		float3 Fex = exp(-(betaR * sR + betaM * sM));

		// in scattering
		float cosViewSunAngle = dot(normalize(i.worldPos - _WorldSpaceCameraPos), _WorldSpaceLightPos0);

		float rPhase = rayleighPhase(cosViewSunAngle*0.5 + 0.5);
		float3 betaRTheta = betaR * rPhase;

		float mPhase = hgPhase(cosViewSunAngle, _mieDirectionalG);
		float3 betaMTheta = betaM * mPhase;

		float3 Lin = pow(sunE * ((betaRTheta + betaMTheta) / (betaR + betaM)) * (1.0 - Fex), float3(1.3, 1.3, 1.3));
		Lin *= lerp(float3(1.0, 1.0, 1.0),pow(sunE * ((betaRTheta + betaMTheta) / (betaR + betaM)) * Fex, float3(1.0 / 2.0, 1.0 / 2.0, 1.0 / 2.0)),clamp(pow(1.0 - dot(_up, _WorldSpaceLightPos0),5.0),0.0,1.0));

		// nightsky
		float3 direction = normalize(i.worldPos - _WorldSpaceCameraPos);
		float theta = acos(direction.y); // elevation --> y-axis, [-pi/2, pi/2]
		float phi = atan2(direction.y, direction.x); // azimuth --> x-axis [-pi/2, pi/2]
		float2 uv = float2(phi, theta) / float2(2.0*_M_PI, _M_PI) + float2(0.5, 0.0);
		float3 L0 = float3(0.1, 0.1, 0.1) * Fex;

		float sundisk = smoothstep(_sunAngularDiameterCos, _sunAngularDiameterCos +0.00009, cosViewSunAngle);
		if (normalize(i.worldPos - _WorldSpaceCameraPos).y>0.0)
			L0 += (sunE * 19000.0 * Fex)*sundisk;

		float W = 1000.0;
		float3 whiteScale = 1.0 / Uncharted2Tonemap(float3(W, W, W));

		float3 texColor = (Lin + L0);
		texColor *= 0.03;
		texColor += float3(0.0,0.001,0.0025)*0.3;

		float luminance = 1.0;
		float g_fMaxLuminance = 1.0;
		float fLumScaled = 0.1 / luminance;
		float fLumCompressed = (fLumScaled * (1.0 + (fLumScaled / (g_fMaxLuminance * g_fMaxLuminance)))) / (1.0 + fLumScaled);

		float ExposureBias = fLumCompressed;

		float3 curr = Uncharted2Tonemap((log2(2.0 / pow(luminance, 4.0)))*texColor);
		float3 color = curr*whiteScale;

		float3 retColor = pow(color, float3(1.0 / 1.2, 1.0 / 1.2, 1.0 / 1.2));

		return float4(retColor, 1.0);
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