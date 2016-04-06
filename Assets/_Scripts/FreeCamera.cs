using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class FreeCamera : MonoBehaviour {
	public bool enabledOnPause = true, editorOnly = true;

	[Space(20)]
	public float lookSensitivity = 5;
	public float moveSpeed = 5;
	public float speedModifier = 2;

	[Space(20)]
	public string lookHorizontal = "Mouse X";
	public string lookVertical = "Mouse Y";
	public KeyCode moveForward = KeyCode.W, moveBack = KeyCode.S, moveLeft = KeyCode.A, moveRight = KeyCode.D, moveFaster = KeyCode.LeftShift, flyToggle = KeyCode.F8;
	
	[Space(20)]
	public UnityEvent FlyEnabled, FlyDisabled;

	float rotationY, rotationX;
	float _moveSpeed;
	bool _on = false;

	public bool on {
		get
		{
			return _on;
		}
		set
		{	
			_on = value;
		
			if (_on)
			{
				FlyEnabled.Invoke();
			}
			else
			{
				FlyDisabled.Invoke();
			}
		}
	}

	void OnEnable() {
		if (!Application.isEditor && editorOnly) Destroy(this);
	}
	
	void Start() {
		_moveSpeed = moveSpeed;
	}

	void Update() {
		if (Input.GetKeyDown(flyToggle))
			on = !on;

		if (!enabledOnPause && Time.timeScale == 0 || !on) return;

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
