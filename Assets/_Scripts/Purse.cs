using UnityEngine;

[DisallowMultipleComponent]
public class Purse : MonoBehaviour {
	public static Purse instance;

	public Options options;
	[HideInInspector]
	public TakeScreenshot screenshot;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	void Init() {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else if (instance != this)
		{
			Destroy(this);
			return;
		}
		screenshot = gameObject.AddComponent<TakeScreenshot>();
		options = new Options();
		LoadOptions(System.IO.File.ReadAllText(fileName()));
	}
	
	void Awake() {
		if (instance == null)
		{
			Init();
		}
	}

	void Update() {
		HandleSaving();
	}

	string savedData;

	string fileName() {
		string _fileName = Application.persistentDataPath;

		_fileName += "/" + Application.productName.ToLower() + "_settings.gmc";

		if (!System.IO.File.Exists(_fileName))
		{
			System.IO.FileStream.Null.Close();
			System.IO.File.CreateText(_fileName);
		}
		return _fileName;
	}

	void HandleSaving() {
		if (Input.GetKeyDown(KeyCode.U))
		{
			SaveOptions();
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			LoadOptions(System.IO.File.ReadAllText(fileName()));
		}
	}

	public void SaveOptions() {
		savedData = JsonUtility.ToJson(options, true);

		System.IO.File.WriteAllText(fileName(), savedData);
	}

	public void LoadOptions(string savedData) {
		if (!System.IO.File.Exists(fileName())) return;

		JsonUtility.FromJsonOverwrite(savedData, options);
	}
}