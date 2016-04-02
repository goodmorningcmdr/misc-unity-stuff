using UnityEngine;
using System.Collections;
[DisallowMultipleComponent]
public class TakeScreenshot : MonoBehaviour {
	
	public static int startNumber = 1;
	
	void SaveScreenshot() {
		string dest = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		dest += "/" + Application.productName;

		try
		{
			if (!System.IO.Directory.Exists(dest))
			{
				System.IO.Directory.CreateDirectory(dest);
			}
		}
		catch (System.IO.IOException ex)
		{
			System.Console.WriteLine(ex.Message);
		}

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
        if (Input.GetKeyDown(KeyCode.Print)) SaveScreenshot();	
	}
}