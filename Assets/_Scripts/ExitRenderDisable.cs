using UnityEngine;
using UnityEngine.Events;

public class ExitRenderDisable : MonoBehaviour {

	public Transform target;
	public bool distanceCheck;
	public float maxDistance = 50;
	[Range(0.001f, 60)]
	public float interval = 1f;

	public UnityEvent OnVisible;
	public UnityEvent OnInvisible;

	void Awake() {
		if (target == null && Camera.main) target = Camera.main.transform;
		if (distanceCheck && target) InvokeRepeating("DistanceCheck", 1, interval);
	}

	void DistanceCheck() {
		if (!target)
		{
			distanceCheck = false;
			CancelInvoke();
			OnVisible.Invoke();
			return;
		}

		float dist = (transform.position - target.position).sqrMagnitude;

		if (dist > maxDistance * maxDistance)
		{
			OnInvisible.Invoke();
		}
		else
		{
			OnVisible.Invoke();
		}
	}

	void OnBecameInvisible() {
		OnInvisible.Invoke();
	}

	void OnBecameVisible() {
		OnVisible.Invoke();
	}
}
