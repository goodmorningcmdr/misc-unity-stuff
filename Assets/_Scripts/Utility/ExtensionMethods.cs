using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class ExtensionMethods {
    public static Transform GetGrandChild(this Transform trans, string childName) {
		Transform[] items = trans.gameObject.GetComponentsInChildren<Transform>(true);

		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].name == childName) return items[i];
		}
		return null;
	}

	public static Transform GetGrandChild(this Transform trans, string childName, bool closeEnough) {
		Transform[] items = trans.gameObject.GetComponentsInChildren<Transform>(true);
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].name == childName || closeEnough && items[i].name.Contains(childName))
			{
				return items[i];
			}
		}
		return null;
	}

	public static void DestroyChildren(this Transform trans) {
		foreach (Transform child in trans.GetComponentsInChildren<Transform>(true))
		{
			if (child != trans) GameObject.Destroy(trans.gameObject);
		}
	}
	
	public static void SetLayerToChildren(this Transform trans, int wantedLayer) {
		foreach (Transform child in trans.GetComponentsInChildren<Transform>(true))
		{
			child.gameObject.layer = wantedLayer;
		}
	}

	public static void SetTagToChildren(this Transform trans, string tagName) {
		foreach (Transform child in trans.GetComponentsInChildren<Transform>(true))
		{
			child.gameObject.tag = tagName;
		}
	}

	public static void SetShadows(this Transform trans, bool off) {
		foreach (MeshRenderer tmesh in trans.GetComponentsInChildren<MeshRenderer>(true))
		{
			if (off) tmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			else tmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}

		foreach (MeshRenderer tsmesh in trans.GetComponentsInChildren<MeshRenderer>(true))
		{
			if (off) tsmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			else tsmesh.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}
	
	public static void ResetTransform(this Transform trans) {
		trans.localRotation = Quaternion.identity;
		trans.localPosition = Vector3.zero;
		trans.localScale = Vector3.one;
	}

	public static Color ChangeAlpha(this Color color, float alpha) {
		return new Color(color.r, color.g, color.b, alpha);
	}

	public static T GetComponentInParents<T>(this GameObject gameObject) where T : Component {
		for (Transform t = gameObject.transform; t != null; t = t.parent)
		{
			T result = t.GetComponent<T>();
			if (result != null)	return result;
		}
		return null;
	}

	public static T[] GetComponentsInParents<T>(this GameObject gameObject)	where T : Component {
		List<T> results = new List<T>();
		for (Transform t = gameObject.transform; t != null; t = t.parent)
		{
			T result = t.GetComponent<T>();
			if (result != null)	results.Add(result);
		}
		return results.ToArray();
	}

	public static string SeperateCamelCase(this string value) {
		return Regex.Replace(value, "((?<=[a-z])[A-Z]|(?<!^)[A-Z](?=[a-z]))", " $1"); 
	}
}