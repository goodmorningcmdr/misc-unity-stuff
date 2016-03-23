using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CreateFindWizard : EditorWindow {
	
	public string componentName = "AudioSource";
	public List<Transform> transformList = new List<Transform>();
	bool fold = false;

	[MenuItem("Tools/List Components In Scene")]
	static void Init() {
		CreateFindWizard window = (CreateFindWizard)EditorWindow.GetWindow(typeof(CreateFindWizard));
		window.titleContent.text = "List Component";
		window.Show();
	}

	void OnGUI() {
		fold = EditorGUILayout.InspectorTitlebar(fold, EditorWindow.focusedWindow);
		
		if (fold)
			return;

		EditorGUILayout.Space();

		if (CheckComponentValid().Length > 0)
		{
			EditorGUILayout.HelpBox(CheckComponentValid(), MessageType.Warning, false);
		}
		else
		{
			EditorGUILayout.HelpBox("Looking for the " + CheckComponentValid() + " component", MessageType.Info, false);
		}
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Component Name: ");
		componentName = EditorGUILayout.TextField(componentName, GUILayout.Width(300));
		EditorGUILayout.EndHorizontal();

		//Transform[] items = GameObject.FindObjectsOfType<Transform>();
		
		//for (int i = 0; i < items.Length; i++)
		//{
		//	if (items[i].GetComponent(componentName))
		//	{
		//		Debug.Log(items[i].name, items[i].transform);
		//		if (!transformList.Contains(items[i])) transformList.Add(items[i]);
		//	}
		//}
	}

	string CheckComponentValid() {
		string pattern = @"[^a-zA-Z0-9]";
		Regex rgx = new Regex(pattern);
		componentName = rgx.Replace(componentName, "");

		if (componentName.Length <= 0)
			return "Enter the name of the components you want to find";

		if (System.Type.GetType(componentName + ", Assembly-CSharp", false) == null && System.Type.GetType("UnityEngine." + componentName + ", UnityEngine", false) == null)
		{
			return "Not found";
		}
		else
		{
			return "";
		}
	}
}