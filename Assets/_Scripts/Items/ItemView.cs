using UnityEngine;
using System.Collections;

public class ItemView : MonoBehaviour {
	
	public float speed = 50;
	Vector3 oldPos, oldRot, newPos, newRot;
	public static bool holding = false;
	bool returning;
	public float distanceToCamera = 0.4f;
	public bool rotateHorizontal = true, rotateVertical = false;
	Options Options;

	void Awake() {
		Options = Options.getInstance();
		holding = false;
	}

	void Start() {
		oldPos = transform.position;
		oldRot = transform.eulerAngles;
	}

	void Update() {
		if (holding && Time.timeScale != 0 && !returning)
		{
			if (Input.GetButtonDown("Throw"))
			{
				transform.LookAt(Camera.main.transform.position);
			}

			float lh = Input.GetAxis("Mouse X");
			float ly = Input.GetAxis("Mouse Y");

			if (rotateHorizontal) transform.Rotate(Vector3.down * lh * speed * Time.deltaTime, Space.Self);

			if (rotateVertical)
			{
				if (!Options.invertYAxis)
					transform.Rotate(Vector3.left * ly * speed * Time.deltaTime, Space.Self);
				else
					transform.Rotate(Vector3.right * ly * speed * Time.deltaTime, Space.Self);
			}

			newRot = transform.eulerAngles;

			if (Input.GetButtonDown("Interact")) StartCoroutine("PutDown");
		}
	}
	
	public void ViewThisItem() {
		FPSController.disabled = true;
		StartCoroutine("ViewMe");
	}
	
	IEnumerator ViewMe() {
		Transform here = Camera.main.transform.FindChild("ItemHere");

		if (here == null) 
		{
			GameObject th = new GameObject();
			th.name = "ItemHere";
			th.transform.parent = Camera.main.transform;
			th.transform.localPosition = Vector3.zero;
			here = th.transform;
		}
		
		here.localPosition = new Vector3(0, 0, distanceToCamera);
		here.LookAt(Camera.main.transform);
		newPos = here.position;
		newRot = here.eulerAngles;

		while ((transform.position - newPos).magnitude >= .005f)
		{
			transform.position = Vector3.MoveTowards(transform.position, newPos, 5 * Time.deltaTime);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(newRot), 200 * Time.deltaTime);
			yield return null;
		}

		transform.position = newPos;
		transform.rotation = Quaternion.Euler(newRot);
		
		newRot = transform.eulerAngles;

		transform.tag = "Untagged";
		
		yield return new WaitForEndOfFrame();
		returning = false;
		holding = true;
	}

	IEnumerator PutDown() {
		returning = true;		

		float t = Time.time;
		while (t + 0.25f > Time.time)
		{
			transform.position = Vector3.Lerp(transform.position, oldPos, 10 * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(oldRot), 10 * Time.deltaTime);
			yield return null;
		}

		transform.position = oldPos;
		transform.eulerAngles = oldRot;
		newRot = Vector3.zero;
		returning = false;
		holding = false;
		yield return new WaitForEndOfFrame();		
		transform.tag = "Interactable";
		FPSController.disabled = false;
	}
}
