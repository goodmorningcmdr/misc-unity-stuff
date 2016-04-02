using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class TakeScreenshotEditor : Editor {
	public static int startNumber = 1;
	
    [MenuItem ("Tools/Save Screenshot", false, 1)]
    static void TakeScreenshot() {
		System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
		Type type = assembly.GetType("UnityEditor.GameView");
		EditorWindow gameview = EditorWindow.GetWindow(type);
		gameview.Repaint();
		int number = startNumber;
		string fileName = @"\" + Application.productName + " Screenshot " + number;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		while (System.IO.File.Exists(path + fileName + ".png"))
		{
			number++;
			fileName = @"\" + Application.productName + " Screenshot " + number;
		}
		Application.CaptureScreenshot(path + fileName + ".png", 1);
		Debug.Log("Saved screenshot " + path + fileName + ".png");
    }

	[MenuItem("Tools/Other/Save High Resolution Screenshot", false, 1)]
	static void TakeScreenshotHigh() {
		System.Reflection.Assembly assembly = typeof(UnityEditor.EditorWindow).Assembly;
		Type type = assembly.GetType("UnityEditor.GameView");
		EditorWindow gameview = EditorWindow.GetWindow(type);
		gameview.Repaint();
		int number = startNumber;
		string fileName = @"\" + Application.productName + " Screenshot " + number;
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		
		while (System.IO.File.Exists(path + fileName + ".png"))
        {
            number++;
			fileName = @"\" + Application.productName + " Screenshot " + number;
        }
		Application.CaptureScreenshot(path + fileName + ".png", 4);
		Debug.Log("Saved screenshot " + path + fileName + ".png");
	}
}