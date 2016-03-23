using UnityEngine;
using UnityEngine.Events;

public class ExitRenderDisable : MonoBehaviour {
	
	Transform player;
	public bool distanceCheck;
	public float maxDistance = 50;
	[Range(0.001f, 60)]
	public float interval = 1f;
	
	public UnityEvent OnVisible;
	public UnityEvent OnInvisible;

	void Awake() {
		player = Camera.main.transform;
		if (distanceCheck) InvokeRepeating("DistanceCheck", 1, interval);
	}
	
	void DistanceCheck() {
		float dist = (transform.position - player.position).sqrMagnitude;
		
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
