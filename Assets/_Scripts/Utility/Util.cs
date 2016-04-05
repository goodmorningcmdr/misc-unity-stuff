using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class Util {
	public static List<T> FindAllObjectsOfType<T>() where T : Component {
		List<T> list = new List<T>();
		foreach (T instance in Resources.FindObjectsOfTypeAll<T>())
		{
			if (instance == null || instance.gameObject == null)
			{
				continue;
			}
			if (instance.gameObject && instance.gameObject.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene())
				list.Add(instance);
		}
		return list;
	}

	public static Quaternion randomRotation(float range) {
		return Quaternion.Euler(Random.insideUnitSphere * range);
	}

	public static bool IsComponentValid(string componentName) {
		string pattern = @"[^a-zA-Z0-9]";
		Regex rgx = new Regex(pattern);
		componentName = rgx.Replace(componentName, "");

		if (componentName.Length <= 0)
			return false;

		if (System.Type.GetType(componentName + ", Assembly-CSharp", false, true) == null && 
		System.Type.GetType("UnityEngine.UI." + componentName + ", UnityEngine.UI", false, true) == null
		&& 
		System.Type.GetType("UnityEngine." + componentName + ", UnityEngine", false, true) == null)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
