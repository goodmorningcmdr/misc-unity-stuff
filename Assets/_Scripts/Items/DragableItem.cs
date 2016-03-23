using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DragableItem : MonoBehaviour {	
	void Start() {
		transform.tag = "Interactable";
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().useGravity = true;
		GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}
}
