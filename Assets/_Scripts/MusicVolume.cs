using UnityEngine;

public class MusicVolume : MonoBehaviour {
	Options Options;
	AudioSource _audio;
	[Range(0, 1)]
	public float desiredVolume = 1;

	void OnEnable() {
		Options = Options.getInstance();
		_audio = GetComponent<AudioSource>();
		_audio.ignoreListenerVolume = true;
		_audio.volume = desiredVolume * Options.musicVolume;
	}

	void Update() {
		_audio.volume = desiredVolume * Options.musicVolume;
	}
}
