using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour {
	public string level;
	public float delay;
	public LoadSceneMode loadMode = LoadSceneMode.Single;
	public bool destroyAll = false;

	[TextArea(2, 10)]
	public string loadScreenText;

	public Text textDisplay;
	
	bool changingLevels = false;
	int fromIndex;

	void Awake() {
		fromIndex = SceneManager.GetActiveScene().buildIndex;

		if (textDisplay == null)
		{
			var go = Instantiate(Resources.Load("LoadScreenText")) as GameObject;
			textDisplay = go.GetComponentInChildren<Text>();
		}
		
		string[] txt = loadScreenText.Split('\n');
		textDisplay.text = txt[Random.Range(0, txt.Length)];

		DontDestroyOnLoad(this.gameObject);
		DontDestroyOnLoad(textDisplay.transform.root.gameObject);

		Invoke("LoadLevel", delay);
	}

	void LoadLevel() {
		changingLevels = true;
		if (destroyAll) DestroyAll();
		SceneManager.LoadScene(level, loadMode);
	}

	void DestroyAll() {
		foreach (Transform t in Util.FindAllObjectsOfType<Transform>())
		{
			Destroy(t.gameObject);
		}
	}

	void Update() {
		if (loadMode == LoadSceneMode.Additive && changingLevels)
		{
			Destroy(textDisplay.transform.root.gameObject);
			Destroy(this.gameObject);
		}
	}

	void OnLevelWasLoaded(int index) {
		if (changingLevels && index != fromIndex)
		{
			Destroy(textDisplay.transform.root.gameObject);
			Destroy(this.gameObject);
		}
	}
}
