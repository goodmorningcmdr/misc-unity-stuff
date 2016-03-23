using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Letterbox))]
public class LetterboxEditor : Editor {
	SerializedProperty p_Aspect;

	static Dictionary<string, GUIContent> m_GUIContentCache;

	protected static GUIContent GetContent(string textAndTooltip)
	{
		if (string.IsNullOrEmpty(textAndTooltip))
			return GUIContent.none;

		if (m_GUIContentCache == null)
			m_GUIContentCache = new Dictionary<string, GUIContent>();

		GUIContent content = null;

		if (!m_GUIContentCache.TryGetValue(textAndTooltip, out content))
		{
			string[] s = textAndTooltip.Split('|');
			content = new GUIContent(s[0]);

			if (s.Length > 1 && !string.IsNullOrEmpty(s[1]))
				content.tooltip = s[1];

			m_GUIContentCache.Add(textAndTooltip, content);
		}

		return content;
	}
	static GUIContent[] presets = 
	{
		new GUIContent("Choose a preset"),
		new GUIContent("6:5"),
		new GUIContent("5:4"),
		new GUIContent("4:3"),
		new GUIContent("3:2"),
		new GUIContent("16:10"),
		new GUIContent("16:9"),
		new GUIContent("2:1"),
		new GUIContent("11:5"),
		new GUIContent("21:9"),
		new GUIContent("11:4")
	};

	static float[] presetsData = { 6f/5f, 5f/4f, 4f/3f, 3f/2f, 16f/10f, 16f/9f, 2f/1f, 11f/5f, 21f/9f, 2.76f/1f };

	void OnEnable() {
		p_Aspect = serializedObject.FindProperty("Aspect");
	}

	public override void OnInspectorGUI() {
		serializedObject.Update();

		EditorGUILayout.PropertyField(p_Aspect, GetContent("Aspect Ratio"));

		EditorGUI.BeginChangeCheck();
		int selectedPreset = EditorGUILayout.Popup(GetContent("Preset"), 0, presets);

		if (EditorGUI.EndChangeCheck() && selectedPreset > 0)
		{
			selectedPreset--;
			p_Aspect.floatValue = presetsData[selectedPreset];
		}
		EditorGUI.BeginDisabledGroup(true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("shader"));
		EditorGUI.EndDisabledGroup();
		serializedObject.ApplyModifiedProperties();
	}
}