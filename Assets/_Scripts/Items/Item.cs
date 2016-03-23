using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Item : MonoBehaviour {

	[TextArea(2, 10)]
	public string interactedText;
	public string highlightText;

	public UnityEvent TriggerEvent;
	public UnityEvent OneTriggerEvent;
	bool doOnce = true;

	public float displayTextTime = 5;

	void Start() {
		transform.tag = "Interactable";
	}

	public void InteractEvent() {
		TriggerEvent.Invoke();

		if (doOnce)
		{
			doOnce = false;
			OneTriggerEvent.Invoke();
		}
	}
}