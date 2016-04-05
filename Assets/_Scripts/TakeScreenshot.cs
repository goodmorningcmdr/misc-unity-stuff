using UnityEngine;
using System.Collections;
[DisallowMultipleComponent]
public class TakeScreenshot : MonoBehaviour {
	
	public static int startNumber = 1;
	
	void SaveScreenshot() {
		string dest = Application.persistentDataPath;

		int number = startNumber;
		string name = "" + number;

		string fileName = dest + "/" + Application.productName + " Screenshot ";

		while (System.IO.File.Exists(fileName + name + ".png"))
		{
			number++;
			name = "" + number;
		}

		startNumber = number + 1;

		Application.CaptureScreenshot(fileName + name + ".png", 1);

		Debug.Log("Saved screenshot: " + fileName + name + ".png");
	}

	void Update() {
        if (Input.GetKeyDown(KeyCode.F12)) SaveScreenshot();	
	}
}