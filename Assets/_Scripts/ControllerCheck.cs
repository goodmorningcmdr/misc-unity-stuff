using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class ControllerCheck : MonoBehaviour {

	public static bool usingController;

	private static ControllerCheck _instance;

	private bool hasController;
	
	public int sceneIndex = 1;
	
	void Awake() {
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			if (this != _instance) Destroy(this);
		}		
	}
	
	void Start() {
		hasController = (Input.GetJoystickNames().Length > 0) ? true : false;
	}

	void Update() {
		if (Application.isEditor) return;
		
		if (Time.timeScale == 1 && SceneManager.GetActiveScene().buildIndex > sceneIndex)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}		
		else
		{
			if (!usingController)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		if (!hasController) return;
		
		if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0 || Input.inputString.Length > 0)
		{
			usingController = false;
		}

		if (!usingController)
		{
			if (Cursor.visible)
			{
				for (int i = 0; i < 11; i++)
				{
					if (Mathf.Abs(Input.GetAxis("Joy1 Axis " + i)) > 0.1f)
					{				
						usingController = true;
						break;
					}
				}
				
				for (int i = 0; i < 20; i++)
				{
					if (Input.GetKey((KeyCode)Enum.Parse(typeof(KeyCode), ("Joystick1Button" + i))))
					{				
						usingController = true;
						break;
					}
				}
			}
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}