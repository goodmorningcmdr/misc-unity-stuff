using UnityEngine;
using System.Collections;
[DisallowMultipleComponent]
public class TakeScreenshot : MonoBehaviour {
	
	public static int startNumber = 1;
	
	void SaveScreenshot() {
		string dest = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
		dest += "/" + Application.productName + "/Screenshots/";

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
	}

	void OnGUI() {
		Event e = Event.current;
        if (e.isKey && e.keyCode.ToString() == "SysReq") SaveScreenshot();	
	}
}