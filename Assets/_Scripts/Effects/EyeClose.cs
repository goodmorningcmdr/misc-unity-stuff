using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EyeClose : MonoBehaviour {
	
	private Texture2D face;
	public float speed = 0.5f;
	private float theColor = 0;
	public string nextLevel = "";
	public bool stop = false;
	public bool waitForInput = false;
	
	void Awake() {
		face = new Texture2D(1, 1);	
		face.SetPixel(1, 1, Color.black);
		face.Apply();
	}
	
	IEnumerator Start() {
		while (theColor < 1) 
		{			
			theColor += speed * Time.deltaTime;
			yield return null;
		}
	}
	
	void Update() {
		if (theColor >= 1 && stop)
			KillMe();
	}

	void KillMe() {
		if (nextLevel.Length > 0)
		{
			if (waitForInput)
			{
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)) 
				{
					Invoke("NextLevel", 0.1f);
				}
			}			
		}
		else
		{
			Destroy(this);
		}
	}

	void NextLevel() {
		SceneManager.LoadScene(nextLevel);
	}

	void OnGUI() {
		if (Time.timeScale == 0)
			return;

		Color tempColor = Color.black * theColor;
		GUI.color = tempColor;
		GUI.depth = -1000;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), face, ScaleMode.StretchToFill, true, 0.0f);
	}
}