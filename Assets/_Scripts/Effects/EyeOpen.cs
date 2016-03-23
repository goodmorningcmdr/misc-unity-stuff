using UnityEngine;
using System.Collections;

public class EyeOpen : MonoBehaviour {
	
	private Texture2D face;
	public float speed = 0.5f;
	private float theColor = 1;
	public float delay;
	public bool killOnPause = false;

	void Awake() {
		face = new Texture2D(1, 1);
		face.SetPixel(1, 1, Color.black);
		face.Apply();
	}

	IEnumerator Start() {
		yield return new WaitForSeconds(delay);
		
		while (theColor > 0)
		{
			theColor -= speed * Time.deltaTime;			
			yield return null;	
		}
	}

	void Update() {
		if (killOnPause && Time.timeScale == 0 || theColor <= 0)
		{
			Destroy(this);
		}
	}
	
	void OnGUI() {
		if (Time.timeScale == 0)
			return;

		Color tempColor = Color.black * theColor;
		GUI.color = tempColor;
		GUI.depth = -1000;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), face, ScaleMode.StretchToFill, true, 0);
	}
}