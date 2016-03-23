using UnityEngine;
using System.Collections;

public class RecalculateNormals : MonoBehaviour {
	
	public int angle = 50;
	
	void OnEnable() {
		foreach (MeshFilter meshFilter in GetComponentsInChildren<MeshFilter>())
		{
			meshFilter.mesh.RecalculateNormals(angle);
		}
	}
}
