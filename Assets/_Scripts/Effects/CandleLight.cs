using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Light))]
public class CandleLight : MonoBehaviour {
	Light _light;

	float min = 0.1f;
	float max = 0.25f;
	public float speed = 20;
	float newIntensity;
	[Range(0, 8)]
	public float minIntensity = 0.75f, maxIntensity = 1;
	
	void Awake() {
		_light = GetComponent<Light>();
	}
	
	IEnumerator Start() {
		while (true)
		{
			newIntensity = Random.Range(minIntensity, maxIntensity);
			yield return new WaitForSeconds(Random.Range(min, max));
		}
	}

	void Update() {
		_light.intensity = Mathf.Lerp(_light.intensity, newIntensity, speed * Time.deltaTime);
	}
}