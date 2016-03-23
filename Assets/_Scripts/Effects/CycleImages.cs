using UnityEngine;
using System.Collections;

public class CycleImages : MonoBehaviour {
	
	public Texture2D[] frames;
	public float framesPerSecond = 30;
	public int materialIndex = 0;
	Renderer _renderer;
	
	void Awake() {
		_renderer = GetComponent<Renderer>();
	}

	void Update() {		
		float index = Time.time * framesPerSecond;
		index = index % frames.Length;

		_renderer.materials[materialIndex].mainTexture = frames[(int)index];
	}
}