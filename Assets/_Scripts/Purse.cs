using UnityEngine;

[DisallowMultipleComponent]
public class Purse : MonoBehaviour {
	public static Purse instance;

	public Options options;
	public TakeScreenshot screenshot;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	void Init() {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
			return;
		}
	}
	
	void Awake() {
		if (instance == null)
		{
			//Debug.LogError("This shit is fucked", this.transform);
			Init();
		}
		options = new Options();
		LoadOptions(System.IO.File.ReadAllText(fileName()));
	}

	void Update() {
		HandleSaving();
	}

	string savedData;
	string fileName() {
		string _fileName = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/" + Application.productName;

		try { if (!System.IO.Directory.Exists(_fileName)) { System.IO.Directory.CreateDirectory(_fileName); } }
		catch (System.IO.IOException ex) { System.Console.WriteLine(ex.Message); }

		_fileName += "/" + Application.productName.ToLower() + "_settings.gmc";
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