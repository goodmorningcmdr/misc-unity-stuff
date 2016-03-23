using UnityEngine;
using System.Collections;

public class ScaleBounce : MonoBehaviour {
	
	public Vector3 wantedScale = Vector3.one * 1.01f;
	bool direction = true;
	public float speed = 2;
	Vector3 defScale, newScale;
	public float timeBetweenBounces = 0.1f;
	bool wait;

	void Awake() {
		defScale = transform.localScale;
		newScale = wantedScale;
	}
	
	void Update() {		
		transform.localScale = Vector3.Lerp(transform.localScale, newScale, speed * Time.deltaTime);
	}

	void LateUpdate() {
		if ((transform.localScale - newScale).magnitude < 0.005f)
		{
			if (!wait)
			{
				wait = true;
				StartCoroutine("ChangeDirection");
			}
		}
	}

	IEnumerator ChangeDirection() {
		yield return new WaitForSeconds(timeBetweenBounces);
		direction = !direction;
		if (direction) newScale = wantedScale; else newScale = defScale;
		wait = false;
	}
}
