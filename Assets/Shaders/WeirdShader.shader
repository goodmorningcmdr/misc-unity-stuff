Shader "Custom/Weird Shader" {
    Properties{
        //Tint
        _Color("Main Color", Color) = (1,1,1,1)
 
        //Textures and Alphas
        _TexOne("Texture One (RGB)", 2D) = "white" {}
        _TexTwo("Texture Two (RGB)", 2D) = "white" {}
        _AlphaTexOne("Alpha One (A)", 2D) = "white" {}
        _AlphaTexTwo("Alpha Two(A)", 2D) = "white" {}
        _AlphaTexThree("Alpha Two(A)", 2D) = "white" {}
 
        _Brightness("Brightness", Range(0,5)) = 1
        _AlphaWeakness("Alpha Weakness", Range(0,10)) = 1
           
        _ScrollSpeed1X("Scroll Speed Texture One X", Range(-10,10)) = 0
        _ScrollSpeed1Y("Scroll Speed Texture One Y", Range(-10,10)) = 0
        _ScrollSpeed2X("Scroll Speed Texture Two X", Range(-10,10)) = 0
        _ScrollSpeed2Y("Scroll Speed Texture Two Y", Range(-10,10)) = 0
 
        _ScrollSpeedAlpha1X("Scroll Speed Alpha One X", Range(-10,10)) = 0
        _ScrollSpeedAlpha1Y("Scroll Speed Alpha One Y", Range(-10,10)) = 0
        _ScrollSpeedAlpha2X("Scroll Speed Alpha Two X", Range(-10,10)) = 0
        _ScrollSpeedAlpha2Y("Scroll Speed Alpha Two Y", Range(-10,10)) = 0
 
        _RotationSpeed1("Rotation Speed Texture 1", Float) = 0.0
        _RotationCenter1("Rotation Center Texture 1", Range(0,1)) = 0.5
 
        _RotationSpeed2("Rotation Speed Texture 2", Float) = 0.0
        _RotationCenter2("Rotation Center Texture 2", Range(0,1)) = 0.5
 
        _Speed("Wave Speed", Range(-80, 80)) = 5
        _Freq("Frequency", Range(0, 5)) = 2
        _Amp("Amplitude", Range(-1, 1)) = 1
 
    }
	SubShader {
	//Default Queues -  Background, Geometry, AlphaTest, Transparent, and Overlay
	Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
	LOD 200

	CGPROGRAM
	#pragma surface surf Lambert alpha:fade vertex:vert
	#pragma target 3.0
	
    //sampler2D _Color;
    sampler2D _TexOne;
    sampler2D _TexTwo;
    sampler2D _AlphaTexOne;
    sampler2D _AlphaTexTwo;
    sampler2D _AlphaTexThree;
    fixed4 _Color;
   
    float _ScrollSpeed1X;
    float _ScrollSpeed1Y;
    float _ScrollSpeed2X;
    float _ScrollSpeed2Y;
 
    float _ScrollSpeedAlpha1X;
    float _ScrollSpeedAlpha1Y;
    float _ScrollSpeedAlpha2X;
    float _ScrollSpeedAlpha2Y;
 
    float _RotationSpeed1;
    float _RotationCenter1;
    float _RotationSpeed2;
    float _RotationCenter2;
 
    float _Brightness;
    float _AlphaWeakness;
 
    float _RotationSpeed;
 
    float _Speed;
    float _Freq;
    float _Amp;
    float _OffsetVal;
 
    struct Input {
        float2 uv_TexOne;
        float2 uv_TexTwo;
        float2 uv_AlphaTexOne;
        float2 uv_AlphaTexTwo;
        float2 uv_AlphaTexThree;
    };
 
    void vert(inout appdata_full v) {
        float time = _Time * _Speed;
        float waveValueA = sin(time + v.vertex.x * _Freq) * _Amp;
 
        v.vertex.xyz = float3(v.vertex.x, v.vertex.y + waveValueA, v.vertex.z);
        v.normal = normalize(float3(v.normal.x + waveValueA, v.normal.y, v.normal.z));
    }
 
    // This is the only code you need to touch    
    void surf(Input IN, inout SurfaceOutput o) {
 
        //Rotation
        float sinX, cosX, sinY;
        float2x2 rotationMatrix;
 
        sinX = sin(_RotationSpeed1 * _Time);
        cosX = cos(_RotationSpeed1 * _Time);
        sinY = sin(_RotationSpeed1 * _Time);
        rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
 
        //Center the rotation point and apply rotation
        IN.uv_TexOne.xy -= _RotationCenter1;
        IN.uv_TexOne.xy = mul(IN.uv_TexOne.xy, rotationMatrix);
        IN.uv_TexOne.xy += _RotationCenter1;
 
        sinX = sin(_RotationSpeed2 * _Time);
        cosX = cos(_RotationSpeed2 * _Time);
        sinY = sin(_RotationSpeed2 * _Time);
        rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
 
        //Center the rotation point and apply rotation
        IN.uv_TexTwo.xy -= _RotationCenter2;
        IN.uv_TexTwo.xy = mul(IN.uv_TexTwo.xy, rotationMatrix);
        IN.uv_TexTwo.xy += _RotationCenter2;
 
        //Scrolling
        IN.uv_TexOne.x -= _ScrollSpeed1X * _Time;
        IN.uv_TexOne.y -= _ScrollSpeed1Y * _Time;
 
        IN.uv_TexTwo.x -= _ScrollSpeed2X * _Time;
        IN.uv_TexTwo.y -= _ScrollSpeed2Y * _Time;
 
        IN.uv_AlphaTexOne.x -= _ScrollSpeedAlpha1X * _Time;
        IN.uv_AlphaTexOne.y -= _ScrollSpeedAlpha1Y * _Time;
 
        IN.uv_AlphaTexTwo.x -= _ScrollSpeedAlpha2X * _Time;
        IN.uv_AlphaTexTwo.y -= _ScrollSpeedAlpha2Y * _Time;
 
        //Textures
        fixed4 c1 = tex2D(_TexOne, IN.uv_TexOne) * (_Color * _Brightness); // This is your color texture
        fixed4 c2 = tex2D(_TexTwo, IN.uv_TexTwo) * (_Color * _Brightness); // This is your color texture
 
        //Alphas
        fixed4 a1 = tex2D(_AlphaTexOne, IN.uv_AlphaTexOne); // This is your alpha texture
        fixed4 a2 = tex2D(_AlphaTexTwo, IN.uv_AlphaTexTwo); // This is your alpha texture
        fixed4 a3 = tex2D(_AlphaTexThree, IN.uv_AlphaTexThree); // This is your alpha texture
   
        //Assignment
        o.Albedo = (c1.rgb * c2.rgb * 2); // Setting your color from the one texture
        o.Alpha = ((a1.a * a2.a * 2) * a3.a * 2) *_AlphaWeakness; // Setting your alpha from the other texture
    }
    ENDCG
    }
}