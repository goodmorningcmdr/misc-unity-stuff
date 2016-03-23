using UnityEngine;
using System.Collections;

public class ItemDragger : MonoBehaviour {
	public float spring = 50;
	public float damper = 5;
	public float drag = 10;
	public float angularDrag = 5;
	public float distance = 0.2f;
	public float throwForce = 500;
	public float throwRange = 1000;
	public bool attachToCenter = false;
	private SpringJoint springJoint;
	Camera cam;
	RaycastHit hit;
	float rayDist = 3;

	void Start() {
		holding = false;
		cam = Camera.main;
	}

	void Update() {
		if (holding || FPSController.disabled || Time.timeScale == 0)
		{
			return;
		}

		if (!Input.GetButtonDown("Interact") && Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDist, LayerMaskHelper.EverythingBut(31)))
			return;
		
		if (!hit.rigidbody || hit.rigidbody.isKinematic || hit.transform.tag != "Interactable")
			return;

		if (!springJoint)
		{
			GameObject go = new GameObject("Rigidbody dragger");
			Rigidbody body = go.AddComponent<Rigidbody>();
			springJoint = go.AddComponent<SpringJoint>();
			body.isKinematic = true;
		}
 
		springJoint.transform.position = hit.point;

		if (attachToCenter)
		{
			var anchor = transform.TransformDirection(hit.rigidbody.centerOfMass) + hit.rigidbody.transform.position;
			anchor = springJoint.transform.InverseTransformPoint(anchor);
			springJoint.anchor = anchor;
		}
		else
		{
			springJoint.anchor = Vector3.zero;
		}
 
		springJoint.enableCollision = true;
		springJoint.spring = spring;
		springJoint.damper = damper;
		springJoint.maxDistance = distance;
		springJoint.connectedBody = hit.rigidbody;		
		connectedBooty = springJoint.connectedBody;
		StartCoroutine("DragObject", springDistance);
	}
	public float springDistance = 2.5f;

	Rigidbody connectedBooty;

	void LateUpdate() {
		if (Time.timeScale == 0)
		return;

		if (!ControllerCheck.usingController && !Input.GetButton("Interact") && holding && springJoint != null)
		{
			springJoint.connectedBody.WakeUp();
			holding = false;
		}

		if (ControllerCheck.usingController && Input.GetButtonDown("Interact") && holding && springJoint != null)
		{
			springJoint.connectedBody.WakeUp();
			holding = false;
		}
	}

	public static bool holding = false;

	IEnumerator DragObject(float distance) {
		yield return new WaitForEndOfFrame();
		float oldDrag = connectedBooty.drag;
		float oldAngularDrag = connectedBooty.angularDrag;
		connectedBooty.drag = drag;
		connectedBooty.angularDrag = angularDrag;
		holding = true;

		while (holding && (transform.position - connectedBooty.position).sqrMagnitude < 5 * 5)
        {
			Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            springJoint.transform.position = ray.GetPoint(distance);
 
            if (Input.GetButtonDown("Throw") && Time.timeScale != 0)
			{
				connectedBooty.AddExplosionForce(throwForce, cam.transform.position, throwRange);
				connectedBooty.drag = oldDrag;
				connectedBooty.angularDrag = oldAngularDrag;
				connectedBooty.WakeUp();
				connectedBooty = null;
				holding = false;
				break;
            }

			yield return null;
        }

        if (springJoint.connectedBody)
        {
            springJoint.connectedBody.drag = oldDrag;
            springJoint.connectedBody.angularDrag = oldAngularDrag;
			springJoint.connectedBody.WakeUp();
            springJoint.connectedBody = null;
			holding = false;
        }
    }
}