using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.Events;

public class FilterComponents : EditorWindow {
	
	public string componentName = "";
	public List<Transform> transformList = new List<Transform>();
	bool fold = false;
	string compName;
	public List<Component> compList;
	Vector2 scrollPos;
	public UnityEvent DoThing;

	[MenuItem("Tools/Component Finder")]
	static void Init() {
		FilterComponents window = (FilterComponents)EditorWindow.GetWindow(typeof(FilterComponents), true, "Find Components", true);
		window.autoRepaintOnSceneChange = true;
	}

	[MenuItem("CONTEXT/FilterComponents/Exit")]
	static void Thang(MenuCommand command) {
		FilterComponents body = (FilterComponents)command.context;
		body.Close();
	}

	static void Thang() {
		EditorWindow.focusedWindow.Close();
	}

	void OnGUI() {
		if (DoThing == null)
		{
			DoThing = new UnityEvent();
			DoThing.AddListener(Thang);
		}

		GUILayout.Space(20);

		EditorGUILayout.BeginHorizontal();

		GUILayout.Space(20);
		EditorGUILayout.PrefixLabel(" Component: ");
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

			List<Component> compList = ExtensionMethods.FindAllObjectsOfType<Component>().OrderBy(go => go.GetType().ToString()).ToList();
	
			bool found = false;

			foreach (Component t in compList)
			{
				if (t.GetType().Name.ToLower().Contains(componentName.ToLower().Replace(" ", "")))
				{
					EditorGUIUtility.labelWidth = 250;
					EditorGUILayout.ObjectField(t.GetType().Name.SeperateCamelCase(), t, t.GetType(), false);
					found = true;
				}
			}

			if (!found)
			{
				DoThing.RemoveAllListeners();			
				EditorGUILayout.HelpBox("No components found", MessageType.Warning);
			}
			else
			{
				GUILayout.Space(20);
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				
				//if (GUILayout.Button("Do Thing", GUILayout.Width(200))) 
				//{
				//	DoThing.Invoke();
				//	DoThing = null;
				//}

				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.EndScrollView();

			EditorGUILayout.EndVertical();

			GUILayout.Space(20);
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(10);
		}
	}
}