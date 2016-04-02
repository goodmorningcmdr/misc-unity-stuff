using UnityEngine;

[System.Serializable]
public class Options {
	[Header("Camera/Mouse Settings")]
	[Range(0.1f, 20)]
	public float lookSensitivity = 4;
	public bool smoothMouse = false;
	public bool invertYAxis = false;
	[Range(75, 120)]
	public int fieldOfView = 90;
	public bool viewBob = true;

	[Header("Misc Settings")]
	public bool holdToRun = true;
	public bool holdToCrouch = true;
	public enum diff { Easy, Normal, Hard }
	public diff difficulty = diff.Normal;

	[Header("Volume Settings")]
	[Range(0, 1)]
	public float volume = 1;
	[Range(0, 1)]
	public float musicVolume = 1;
	[Range(0, 1)]
	public float speechVolume = 1;
	
	[Header("UI Settings")]
	public bool subtitles = true;
	public bool hints = true;
	public bool crosshair = true;	

	[Header("Screen/Graphics Settings")]
	public bool isFullscreen = true;
	public int screenResolution;
	public bool vsync = true;
	public int extraBrightness = 0;
	[Range(0, 3)]
	public int ambientOcclusion = 0, screenReflections = 0, antiAliasing = 0, bloom = 1, sharpenFilter = 0, shadows = 3;
	public bool lensAbberation = true;
	public bool noise = true;
	public bool depthOfField = true;
	public bool motionBlur = true;
	
	void Load(){}
	void Save(){}
}

