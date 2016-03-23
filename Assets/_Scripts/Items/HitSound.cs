using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HitSound : MonoBehaviour {
	
	public AudioClip[] hitSounds;
	public float minPitch, maxPitch = 1;
	public float threshold = 3;
	public bool playOnce = false;
	AudioSource _audio;
	
	void Awake() {
		_audio = GetComponent<AudioSource>();
	}
	
	void OnCollisionEnter(Collision other) {
		if (other.relativeVelocity.magnitude > threshold)
		{
			_audio.clip = hitSounds[Random.Range(0, hitSounds.Length)];
			_audio.pitch = Random.Range(minPitch, maxPitch);
			_audio.Play();
			if (playOnce) Destroy(this);
		}
	}
}
