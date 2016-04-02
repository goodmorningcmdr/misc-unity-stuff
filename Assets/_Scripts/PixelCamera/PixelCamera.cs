using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Pixel Camera")]
public class PixelCamera : MonoBehaviour {
	
	public bool calculatePosition = true;
	
	Transform content;
	Transform cachedTransform;
	Camera cachedCamera;
	int previousScreenWidth;
	int previousScreenHeight;
	
	void Awake() {
		cachedTransform = transform;
		cachedCamera = GetComponent<Camera>();	
		cachedCamera.orthographic = true;
		cachedCamera.nearClipPlane = 0;
		
		content = cachedTransform.Find("Content");

		if (content == null)
		{
			content = (Transform)new GameObject("Content").GetComponent<Transform>();
		}
		
		content.parent = cachedTransform;
		Calculate();
	}
	
	void Update() {
		//if (Time.frameCount % 10 != 0) 
			//return;

		Calculate();
	}	
	
	void Calculate() {
		float orthographicSize = Screen.height / 2;
		cachedCamera.orthographicSize = orthographicSize;

		//Vector3 originOffset = new Vector3(orthographicSize * cachedCamera.aspect, -orthographicSize, -1);
		//if (calculatePosition)
		//{
			//cachedTransform.position = originOffset;
			//content.position = cachedTransform.position - originOffset;
		//}
	}
}
