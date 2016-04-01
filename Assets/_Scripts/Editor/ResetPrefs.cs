using UnityEngine;
using UnityEditor;

public class ResetPrefs : Editor {
    [MenuItem("Tools/Other/Reset PlayerPrefs")]
    static void ClearPlayerPrefs() {
		Debug.Log("Clearing PlayerPrefs");
		PlayerPrefs.DeleteAll();
    }
}