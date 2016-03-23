using UnityEngine;

public static class SmootherTime {
    public static float deltaTime() {
		return (1 - Mathf.Exp( -20 * Time.deltaTime));
	}
	
	public static float unscaledDeltaTime() {
		return (1 - Mathf.Exp( -20 * Time.unscaledDeltaTime));
	}
}