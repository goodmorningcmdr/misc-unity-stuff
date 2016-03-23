using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour {

    public UnityEvent OnEnter;
    public UnityEvent OnExit;

	public string onlyWithTag = "";

    void OnTriggerEnter(Collider c) {
		OnEnter.Invoke();
	}
    
	void OnTriggerExit(Collider c) {
		OnExit.Invoke();
	}
}