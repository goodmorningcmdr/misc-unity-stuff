using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

[InitializeOnLoad]
[ExecuteInEditMode] 
public class ReorderComponents : EditorWindow {
   
    int activeButton;
    int tempButtonIndex;
    int lastButtonIndex;
    bool mouseDown;
    int[] newIndexes;
   
    [MenuItem("Tools/Reorder Components", false, 0)]
    private static void ShowWindow () {
		ReorderComponents window = (ReorderComponents)EditorWindow.GetWindow(typeof(ReorderComponents), true, "Reorder Components");
		window.autoRepaintOnSceneChange = true;
    }
   
    void OnInspectorUpdate() {
        Repaint();
    }

	float styleHeight = 32;
   
    void OnGUI () {
        Transform currentTransform = Selection.activeTransform;
		if (currentTransform == null)
		{
			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("Select a game object", MessageType.Info);
		}
        if (Selection.GetFiltered(typeof(Transform), SelectionMode.Unfiltered).Length > 0)
        {
            currentTransform = (Transform) Selection.GetFiltered(typeof(Transform), SelectionMode.Unfiltered)[0];
        }
 
        if (currentTransform != null) 
		{
			EditorWindow windowHndle = EditorWindow.GetWindow(typeof(ReorderComponents));

			Component[] comps = currentTransform.GetComponents<Component>();
			int marginSize = 8;
			float styleWidth = Screen.width - windowHndle.position.size.x;

			Texture2D buttonActiveBackground = new Texture2D(1, 1);
			buttonActiveBackground.SetPixel(0, 0, Color.gray);
			buttonActiveBackground.Apply();
           
			GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
			buttonStyle.alignment = TextAnchor.MiddleCenter;
			buttonStyle.fixedHeight = styleHeight;
			buttonStyle.fixedWidth = styleWidth;
			buttonStyle.margin = new RectOffset(marginSize * 2, marginSize * 2, marginSize, marginSize);
			
			GUIStyle buttonDisabledStyle = new GUIStyle(GUI.skin.button);
			buttonDisabledStyle.alignment = TextAnchor.MiddleCenter;
			buttonDisabledStyle.fixedHeight = styleHeight;
			buttonDisabledStyle.fixedWidth = styleWidth;
			buttonDisabledStyle.margin = new RectOffset(marginSize * 2, marginSize * 2, marginSize, marginSize);
			buttonDisabledStyle.normal.textColor = Color.gray;
           
			GUIStyle buttonActiveStyle = new GUIStyle();
			buttonActiveStyle.alignment = TextAnchor.MiddleCenter;
			buttonActiveStyle.fixedHeight = styleHeight;
			buttonActiveStyle.fixedWidth = styleWidth;
			buttonDisabledStyle.margin = new RectOffset(marginSize * 2, marginSize * 2, marginSize, marginSize);
			buttonActiveStyle.normal.background = buttonActiveBackground;
			buttonActiveStyle.normal.textColor = Color.gray;

            // Check if mouse button gets pressed down and see which button is below mouse
            if (!mouseDown && Event.current.type == EventType.MouseDown) {
				activeButton = Mathf.FloorToInt((Event.current.mousePosition.y) / ((marginSize + styleHeight)));
 
                if (activeButton < comps.Length)
                {
					if (comps[activeButton].GetType().ToString() == "UnityEngine.Transform" || comps[activeButton].GetType().ToString() == "UnityEngine.RectTransform")
					{
                        mouseDown = false;
                    } else {
                        mouseDown = true;
                        lastButtonIndex = activeButton;
                       
                        newIndexes = new int[comps.Length];
                        for (int i=0; i<comps.Length; i++) {
                            newIndexes[i] = i;
                        }
                    }
                }
            }
           
            // Mouse button released
            if (mouseDown && Event.current.type == EventType.MouseUp) {
                mouseDown = false;
               
                // Reorder components
                int positionsToMove = Mathf.RoundToInt(Mathf.Abs(tempButtonIndex - activeButton));
               
                Undo.RecordObject(currentTransform.gameObject, "Undo component reorder.");
 
                if (positionsToMove > 0) {
                    int direction = (tempButtonIndex - activeButton) / Mathf.Abs(tempButtonIndex - activeButton);
               
                    for (int i=0; i<positionsToMove; i++) {
                        if (direction > 0) {
                            UnityEditorInternal.ComponentUtility.MoveComponentDown(comps[activeButton + i]);
                        }
                        if (direction < 0) {
                            UnityEditorInternal.ComponentUtility.MoveComponentUp(comps[activeButton - i]);
                        }
                       
                        comps = currentTransform.GetComponents<Component>();
                    }
                }
            }
           
            // Draw buttons
            for (int i=0; i<comps.Length; i++) {
                int j = i;
               
                if (mouseDown) {
                    j = newIndexes[i];
                }
           
                string componentName = comps[j].GetType().ToString();
           
                GUIStyle style = buttonStyle;
               
                if (mouseDown && i == tempButtonIndex) { style = buttonActiveStyle; }
				if (componentName == "UnityEngine.Transform" || componentName == "UnityEngine.RectTransform") { style = buttonDisabledStyle; }
 
                SerializedObject serialized_object = new SerializedObject(comps[j]);
 
                SerializedProperty prop = serialized_object.GetIterator();
 
                int attempts = 0;
 
                string property_name = "";

                prop.NextVisible(true);
				
				if (componentName.Contains("CanvasRenderer") || !prop.hasChildren) attempts = 0;

				while (attempts > 0 && prop.NextVisible(true))
                {
                    if (prop.propertyType == SerializedPropertyType.String)
                    {
                        property_name = prop.displayName + ": " + prop.stringValue;
                        break;
                    }
                    else if (prop.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        if (prop.objectReferenceValue == null)
                        {
                            property_name = prop.displayName + ": null";
                        }
                        else
                        {
                            property_name = prop.displayName + ": " + prop.objectReferenceValue.name;
                        }
                        break;
                    }
                    else if (prop.propertyType == SerializedPropertyType.Integer)
                    {
                        property_name = prop.displayName + ": " + prop.intValue;
                        break;
                    }
                    else if (prop.propertyType == SerializedPropertyType.Float)
                    {
						if (componentName == "UnityEngine.Transform" || componentName == "UnityEngine.RectTransform")
						{
							prop = serialized_object.FindProperty("m_LocalPosition");
							property_name = prop.displayName + ": " + prop.vector3Value;
						}
						else
							property_name = prop.displayName + ": " + prop.floatValue;

                        break;
                    }
                    else if (prop.propertyType == SerializedPropertyType.Boolean)
                    {
                        property_name = prop.displayName + ": " + prop.boolValue;
                        break;
                    }
					else if (prop.propertyType == SerializedPropertyType.Vector3)
					{
						property_name = prop.displayName + ": " + prop.vector3Value;
						break;
					}
                    attempts --;
                }
				
                if (property_name != "")
                {
                    componentName += "\n" + property_name.Substring(0, Mathf.Min(64, property_name.Length));
                }

				//string compName = componentName.Replace("UnityEngine.", ""); compName = compName.Replace("UnityStandardAssets.","");
				string compName = comps[j].GetType().Name;

				GUILayout.Button(compName.SeperateCamelCase(), style);
            }
           
            // If mouse button is down, draw the extra button
            if (mouseDown) {
                Rect buttonPosition = new Rect(Event.current.mousePosition.x - Screen.width/2 - (styleWidth + marginSize), Event.current.mousePosition.y - 12.5f, Screen.width - 10, 25);
                GUI.Button(buttonPosition, comps[activeButton].GetType().Name.SeperateCamelCase(), buttonStyle);
            }
           
            // Get index for the new temp button
            if (mouseDown) {
                int tmp = Mathf.FloorToInt(Event.current.mousePosition.y / (styleHeight + marginSize));
               
                if (tmp > comps.Length - 1) {
                    tmp = comps.Length - 1;
                }
               
                if (tmp < 0) {
                    tmp = 0;
                }

				if (comps[tmp].GetType().ToString() != "UnityEngine.Transform" && comps[tmp].GetType().ToString() != "UnityEngine.RectTransform")
				{
                    tempButtonIndex = tmp;
                }
                               
                if (tempButtonIndex != lastButtonIndex) {
                    int temp = newIndexes[lastButtonIndex];
                    newIndexes[lastButtonIndex] = newIndexes[tempButtonIndex];
                    newIndexes[tempButtonIndex] = temp;
               
                    lastButtonIndex = tempButtonIndex;
                }
            }
        }
    }
}