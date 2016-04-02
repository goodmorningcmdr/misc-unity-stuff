using UnityEngine;
using System.Collections;

public enum StretchMethod{ Both, Width, Height, Fill, AspectFit, AspectFill }

[ExecuteInEditMode]
[AddComponentMenu("Scripts/Pixel Camera/Pixel Stretch" )]
public class PixelStretch : MonoBehaviour {
	
	public StretchMethod stretchMethod;
	public Vector2 imageSize = new Vector2( 640, 480 );
	
	Transform cachedTransform;
	Vector3 cachedScale;
	
	void Awake() {
		cachedTransform = transform;	
		cachedScale = cachedTransform.localScale;
		Calculate();
	}
	
	void Update () {
		Calculate();
	}
	
	void Calculate(){
		float imageRatio = 0;
		float screenRatio = 0; 
		Vector2 scaled = Vector2.one;
		
		switch (stretchMethod)
		{
			case StretchMethod.AspectFit:
			imageRatio = imageSize.x / imageSize.y;
			screenRatio = (float)Screen.width / (float)Screen.height;
			
			scaled = screenRatio > imageRatio ? new Vector2(imageSize.x * (float)Screen.height / imageSize.y, (float)Screen.height) : new Vector2((float)Screen.width, imageSize.y * (float)Screen.width / imageSize.x);
			
			cachedTransform.localScale = new Vector3(scaled.x, scaled.y, cachedScale.z);
			break;
			
			case StretchMethod.AspectFill:
				imageRatio = imageSize.x / imageSize.y;
				screenRatio = (float)Screen.width / (float)Screen.height;

				scaled = screenRatio < imageRatio ? new Vector2(imageSize.x * (float)Screen.height / imageSize.y, (float)Screen.height) : new Vector2((float)Screen.width, imageSize.y * (float)Screen.width / imageSize.x);

				cachedTransform.localScale = new Vector3(scaled.x, scaled.y, cachedScale.z);
			break;
			
			case StretchMethod.Width:
				cachedTransform.localScale = new Vector3(scaled.x, scaled.y, cachedScale.z);
			break;
			
			case StretchMethod.Height:
				cachedTransform.localScale = new Vector3(scaled.x, scaled.y, cachedScale.z);
			break;
			
			case StretchMethod.Both:
				cachedTransform.localScale = new Vector3(Screen.width, Screen.height, cachedScale.z);
			break;
			
			case StretchMethod.Fill:
				Vector3 size;
				size.z = cachedScale.z;
				float aspect = Screen.width / Screen.height;

				if (aspect <= 1)
				{
					size.x = Screen.width;
					size.y = Screen.width * aspect;
				}
				else
				{
					size.x = Screen.width * (Screen.height / Screen.width);
					size.y = Screen.height;
				}

				cachedTransform.localScale = size;
			break;
		}
	}
}
