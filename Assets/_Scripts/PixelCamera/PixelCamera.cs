/*---------------------------------------------------------------------------------
Configures a camera for easy 1:1 pixel management and placement. A convenient
"content" gameobject is attached to the camera as a place to attach objects to the
camera while still allowing logical 1:1 pixel positioning coordinates. If you
intend to use the content gameobject you should turn off calculatePosition so
you can move the camera around as desired. Note that y position values should be
negative as needed for pixel placement.

Author:	Bob Berkebile
Email:	bobb@pixelplacement.com
---------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]
[ AddComponentMenu ( "Image Effects/Pixel Camera" ) ]
public class PixelCamera : MonoBehaviour {
	
	//-----------------------------------------------------------------------------
	// Public Variables
	//-----------------------------------------------------------------------------
	
	public bool calculatePosition = true;
	
	//-----------------------------------------------------------------------------
	// Private Variables
	//-----------------------------------------------------------------------------
	
	Transform content;
	Transform cachedTransform;
	Camera cachedCamera;
	int previousScreenWidth;
	int previousScreenHeight;
	
	//-----------------------------------------------------------------------------
	// Init
	//-----------------------------------------------------------------------------
	
	void Awake(){
		cachedTransform = transform;
		cachedCamera = GetComponent<Camera>();	
		cachedCamera.orthographic = true;
		cachedCamera.nearClipPlane = 0;
		
		//set up child content holder ( if there was already one from a previous life of the PixelCamera script lets use that instead of creating a new one ):
		content = cachedTransform.Find( "Content" );
		if ( content == null ) {
			content = (Transform)new GameObject( "Content" ).GetComponent<Transform>();
		}
		content.parent = cachedTransform;
		Calculate();
	}
	
	//-----------------------------------------------------------------------------
	// Update
	//-----------------------------------------------------------------------------
		
	void Update () {
		if ( Time.frameCount % 10 != 0 ) {
			return;
		}
		//Calculate();
	}	
	
	//-----------------------------------------------------------------------------
	// Private Methods
	//-----------------------------------------------------------------------------
	
	void Calculate(){
		float orthographicSize = Screen.height / 2;
		cachedCamera.orthographicSize = orthographicSize;
		Vector3 originOffset = new Vector3( orthographicSize * cachedCamera.aspect, -orthographicSize, -1 );
		if ( calculatePosition ) {
			cachedTransform.position = originOffset;
		}
		content.position = cachedTransform.position - originOffset;
	}
}
