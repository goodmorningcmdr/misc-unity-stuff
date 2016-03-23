using UnityEngine;

public class AutoRotate : MonoBehaviour {

	public Vector3 axis = Vector3.up;
	public float speed = 100;
	public Space space = Space.Self;
	public bool randomAxis = false;

	void OnEnable() {
		if (randomAxis) axis = Random.onUnitSphere;
	}

	void Update() {
		transform.Rotate(axis, speed * Time.deltaTime, space);
	}
}
