using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour {
	public float zoomSpeed = 10;
	private float baseFOV = 60;
	Camera _camera;
	public int zoomLevel = 2;

	void Start() {
		_camera = Camera.main;
		_camera.fieldOfView = baseFOV;
	}

	void Update() {
		float radAngle = Purse.instance.options.fieldOfView * Mathf.Deg2Rad;
		float radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * Screen.height / Screen.width);
		float baseFOV = Mathf.Rad2Deg * radHFOV;

		float targetFOV = Input.GetMouseButton(1) ? (baseFOV / zoomLevel) : baseFOV;

		_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, zoomSpeed * Time.deltaTime);
	}
}