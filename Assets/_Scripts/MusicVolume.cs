using UnityEngine;

public class MusicVolume : MonoBehaviour {
	AudioSource _audio;
	[Range(0, 1)]
	public float desiredVolume = 1;

	void Start() {
		_audio = GetComponent<AudioSource>();
		_audio.ignoreListenerVolume = true;
		_audio.volume = desiredVolume * Purse.instance.options.musicVolume;
	}

	void Update() {
		_audio.volume = desiredVolume * Purse.instance.options.musicVolume;
	}
}
