using UnityEditor;
using UnityEngine;

class CubemapWindow: ScriptableWizard {
    public Color color = Color.white;
	public enum res
	{
		_16x16,
		_32x32,
		_64x64,
		_128x128,
		_256x256,
		_512x512,
		_1024x1024,
		_2048x2048
	}
	public res resolution = res._1024x1024; 
	public LayerMask reflect =~0;
	
    void OnWizardUpdate () {
        helpString = "Please set the resolution.";
    }   
}