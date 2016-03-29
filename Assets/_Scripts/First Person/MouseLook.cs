using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MouseLook : MonoBehaviour {
	Options Options;
	public float sensitivity = 4f;
	float invertFlag = 1f;

	float minimumY = -90f;
	float maximumY = 90f;

	float rotationX, rotationY = 0f;

	Quaternion originalRotation, originalPlayerRotation;
	Quaternion xQuaternion, yQuaternion;

	Transform playerTransform;

	private List<float> rotArrayX = new List<float>();
	float rotAverageX = 0f;
	private List<float> rotArrayY = new List<float>();
	float rotAverageY = 0f;
	public float smoothingFrames = 8;
	
	Transform camTransform;	
	
	void Start() {
		Options = Options.getInstance();
		playerTransform = transform;		
		originalPlayerRotation = playerTransform.localRotation;
		camTransform = Camera.main.transform;
		if (camTransform.parent != this.transform) camTransform.SetParent(transform, false);
		originalRotation = camTransform.localRotation;
	}

	void Update() {
		if (Time.timeScale <= 0 || FPSController.disabled)
		{
			rotArrayX = new List<float>();
			rotArrayY = new List<float>();
			return;
		}

		rotAverageX = 0f;
		rotAverageY = 0f;

		invertFlag = Options.invertYAxis ? -1f : 1f;
		
		sensitivity = Options.lookSensitivity;
		
		rotationX += Input.GetAxisRaw("Mouse X") * sensitivity;

		if (Options.smoothMouse && !ControllerCheck.usingController)
		{
			rotArrayX.Add(rotationX);
			if (rotArrayX.Count >= smoothingFrames)
			{
				rotArrayX.RemoveAt(0);
			}
			for (int i = 0 ; i < rotArrayX.Count ; i++)
			{
				rotAverageX += rotArrayX[i];
			}
			rotAverageX /= rotArrayX.Count;
			xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
		}
		else
		{
			xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
		}

		playerTransform.localRotation = originalPlayerRotation * xQuaternion;

		rotationY += Input.GetAxisRaw("Mouse Y") * sensitivity * invertFlag;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

		if (Options.smoothMouse && !ControllerCheck.usingController)
		{
			rotArrayY.Add(rotationY);
			if (rotArrayY.Count >= smoothingFrames)
			{
				rotArrayY.RemoveAt(0);
			}
			for (int j = 0 ; j < rotArrayY.Count ; j++)
			{
				rotAverageY += rotArrayY[j];
			}
			rotAverageY /= rotArrayY.Count;
			yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
		}
		else
		{
			yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
		}

		camTransform.localRotation = originalRotation * yQuaternion;
	}
}