using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour {

	public string level;
	public float delay;
	public LoadSceneMode loadMode = LoadSceneMode.Single;

	[TextArea(2, 10)]
	public string loadScreenText;

	public Text textDisplay;

	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(textDisplay.transform.root.gameObject);
		Invoke("LoadLevel", delay);

		string[] txt = loadScreenText.Split('\n');
		textDisplay.text = txt[Random.Range(0, txt.Length)];
	}

	void LoadLevel() {
		SceneManager.LoadScene(level, loadMode);
	}

	void OnLevelWasLoaded() {
		Destroy(textDisplay);
		Destroy(this.gameObject);	
	}
}
