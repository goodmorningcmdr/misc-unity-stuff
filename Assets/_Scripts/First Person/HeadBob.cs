using UnityEngine;
using System.Collections;

public class HeadBob : MonoBehaviour {
	Options Options;
	public float bobbingSpeed = 10f;
	float runSpeed, walkSpeed;
	public float bobbingAmount = 0.05f;
	public float midpoint = 1.25f;
	float _midpoint;
	float crouchMidpoint = 0.4f;

	private float timer = 0.0f;
	CharacterController playerController;
	FPSController player;
	Transform camTransform;

	void Start() {
		Options = Options.getInstance();
		walkSpeed = bobbingSpeed;
		runSpeed = walkSpeed + 2;
		playerController = transform.GetComponent<CharacterController>();
		player = transform.GetComponent<FPSController>();
		camTransform = Camera.main.transform;
		_midpoint = midpoint;
	}

	void Update() {
		if (!playerController.isGrounded || Time.timeScale == 0 || FPSController.disabled)
		{
			return;
		}

		if (player.isCrouching)
		{
			midpoint = Mathf.Lerp(midpoint, crouchMidpoint, 5 * Time.deltaTime);
		}
		else
		{
			if (player.CanGetUp()) midpoint = Mathf.Lerp(midpoint, _midpoint, 5 * Time.deltaTime);
		}

		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		bobbingSpeed = player.isRunning ? runSpeed : walkSpeed;
		bobbingAmount = player.isRunning ? 0.08f : 0.06f;

		if (!Options.viewBob || playerController.velocity.sqrMagnitude <= 0f)
		{
			bobbingAmount = bobbingSpeed = 0;
		}

		if (player.isCrouching)
		{
			bobbingAmount -= 0.02f;
			bobbingSpeed -= 2;
		}

		if (Mathf.Abs(horizontal) == 0f && Mathf.Abs(vertical) == 0f)
		{
			timer = 0.0f;
		}
		else
		{
			waveslice = Mathf.Sin(timer);
			timer = timer + bobbingSpeed * Time.deltaTime;
			if (timer > Mathf.PI * 2f)
			{
				timer = timer - (Mathf.PI * 2f);
			}
		}

		if (waveslice != 0f)
		{
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;

			Vector3 localPos = camTransform.localPosition;
			localPos.y = midpoint + translateChange * Time.timeScale;
			camTransform.localPosition = localPos;
		}
		else
		{
			Vector3 localPos = camTransform.localPosition;
			localPos.y = midpoint;
			camTransform.localPosition = localPos;
		}
	}
}
