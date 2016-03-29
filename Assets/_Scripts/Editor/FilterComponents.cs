using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Events;

public class FilterComponents : EditorWindow {
	
	public string componentName = "AudioSource";
	public List<Transform> transformList = new List<Transform>();
	bool fold = false;
	string compName;
	public List<Component> compList;
	Vector2 scrollPos;
	public UnityEvent DoThing;

	[MenuItem("Tools/Component Filter")]
	static void Init() {
		FilterComponents window = (FilterComponents)EditorWindow.GetWindow(typeof(FilterComponents), false, "Filter Components");
		window.Show();
	}

	void Thang() {
		Debug.Log("thing");
	}

	void OnGUI() {
		if (DoThing == null) 
		{
			DoThing = new UnityEvent();
			DoThing.AddListener(Thang);
		}

		fold = EditorGUILayout.InspectorTitlebar(fold, EditorWindow.focusedWindow);
		
		if (fold)
			return;

		GUILayout.Space(20);

		EditorGUILayout.BeginHorizontal();

		GUILayout.Space(20);
		EditorGUILayout.PrefixLabel("Component: ");
		GUILayout.FlexibleSpace();

		componentName = EditorGUILayout.TextField(componentName);

		GUILayout.Space(20);
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(20);
		
		if (!fold)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(20);

			EditorGUILayout.BeginVertical();

			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
			EditorGUILayout.Space();
			List<Component> compList = ExtensionMethods.FindAllObjectsOfType<Component>().OrderBy(go => go.name).ToList();
			bool found = false;

			foreach (Component t in compList)
			{
				if (t.GetType().Name.ToLower().Contains(componentName.ToLower().Replace(" ", "")))
				{
					EditorGUIUtility.labelWidth = 250;
					EditorGUILayout.ObjectField(t.GetType().Name.SeperateCamelCase(), t, t.GetType(), true);
					found = true;
				}
			}

			if (!found)
			{
				DoThing.RemoveAllListeners();			
				EditorGUILayout.HelpBox("No components found.", MessageType.Warning);
			}
			else
			{
				GUILayout.Space(20);
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				
				if (GUILayout.Button("Do Thing", GUILayout.Width(200))) 
				{
					DoThing.Invoke();
					DoThing = null;
				}

				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndVertical();

			GUILayout.Space(20);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20);
		}

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
}