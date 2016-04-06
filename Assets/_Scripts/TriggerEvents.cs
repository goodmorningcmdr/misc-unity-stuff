using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour {

	public UnityEvent OnEnter;
	public UnityEvent OnExit;
	
	public string onlyWithTag = "";
	public bool checkTag = false;
	
	void Awake() {
		if (onlyWithTag.Length <= 0)
			checkTag = false;
	}
	
	void OnTriggerEnter(Collider c) {
		if (c.transform.tag != onlyWithTag && checkTag)
			return;
			
		OnEnter.Invoke();
	}
	
	void OnTriggerExit(Collider c) {
		if (c.transform.tag != onlyWithTag && checkTag)
			return;
			
		OnExit.Invoke();
	}
}
