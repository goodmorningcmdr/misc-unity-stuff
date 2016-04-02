using UnityEngine;

public class FreeCamera : MonoBehaviour {
	public bool enabledOnPause = true, editorOnly = true;
	float rotationY, rotationX;
	public float lookSensitivity = 5;
	public float moveSpeed = 5;
	public float speedModifier = 2;
	float _moveSpeed;
	public string lookHorizontal = "Mouse X", lookVertical = "Mouse Y";
	public KeyCode moveForward = KeyCode.W, moveBack = KeyCode.S, moveLeft = KeyCode.A, moveRight = KeyCode.D, moveFaster = KeyCode.LeftShift;
	
	void OnEnable() {
		if (!Application.isEditor && editorOnly) Destroy(this);
	}
	void Start() {
		_moveSpeed = moveSpeed;
	}
	
	void Update() {
		if (!enabledOnPause && Time.timeScale == 0) return;
		rotationX = transform.localEulerAngles.y + Input.GetAxisRaw(lookHorizontal) * lookSensitivity;
		rotationY += Input.GetAxisRaw(lookVertical) * lookSensitivity; rotationY = Mathf.Clamp(rotationY, -90, 90);
		
		moveSpeed = (Input.GetKey(moveFaster)) ? _moveSpeed * speedModifier : _moveSpeed;
		
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		if (Input.GetKey(moveForward)) transform.position += transform.forward * moveSpeed * Time.unscaledDeltaTime;
		if (Input.GetKey(moveBack)) transform.position -= transform.forward * moveSpeed * Time.unscaledDeltaTime;
		if (Input.GetKey(moveLeft)) transform.position -= transform.right * moveSpeed * Time.unscaledDeltaTime;
		if (Input.GetKey(moveRight)) transform.position += transform.right * moveSpeed * Time.unscaledDeltaTime;
	}
}
