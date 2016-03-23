/// <summary>
/// Author: Bob Berkebile
/// Email: bob@bullyentertainment.com || bobb@pixelplacement.com
/// </summary>

using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour {
	
	#region Events
	public static event System.Action OnFadeBegin;
	public static event System.Action<float> OnFadeUpdate;
	public static event System.Action OnFadeEnd;
	#endregion
	
	#region Public Variables
	public static Color CurrentColor{
		get{
			return _currentColor;	
		}
	}	
	
	public static float CurrentAlpha{
		get{
			return _currentColor.a;	
		}
	}
	
	public static bool IsFadingUp{
		get{
			return _isFadingUp;	
		}
	}
	#endregion
	
	#region Private Variables
	static Texture2D _texture;
	static ScreenFade _instance;
	static Color _baseColor = Color.black;
	static Color _startColor;
	static Color _currentColor;
	static Color _endColor;
	static bool _isFadingUp;
	#endregion
	
	#region Init
	void Awake(){
		useGUILayout = false;	
	}
	
	void OnDestroy(){
		_instance = null;
	}
	#endregion
	
	#region OnGUI
	void OnGUI(){
		if ( _currentColor.a > 0 ) {
			GUI.color = _currentColor;
			GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), _texture );
		}
	}
	#endregion
	
	#region Public Methods
	public static void ChangeColor( Color color, bool retainCurrentAlpha ){
		CheckInstance();
		_baseColor = color;
		if ( retainCurrentAlpha ) {
			_baseColor.a = _currentColor.a;
		}
		_texture.SetPixel( 1, 1, _baseColor );
		_texture.Apply();
	}
	
	public static void Fade( Color color, float startAlpha, float endAlpha, float duration, float delay, bool jumpToStartAlpha, System.Action onComplete = null, System.Action<float> onUpdate = null ){
		
		CheckInstance();
		ChangeColor( color, false );
		
		_startColor = _baseColor;
		_startColor.a = startAlpha;
		
		_endColor = _baseColor;
		_endColor.a = endAlpha;
		
		if ( jumpToStartAlpha ) {
			_currentColor.a = startAlpha;
		}
		
		_instance.StopAllCoroutines();
		_instance.StartCoroutine( _instance.DoFade( duration, delay, onComplete, onUpdate ) );	
	}
	
	public static void Fade( float startAlpha, float endAlpha, float duration, float delay, bool jumpToStartAlpha ){
		Fade( _baseColor, startAlpha, endAlpha, duration, delay, jumpToStartAlpha, null, null );
	}

	public static void Fade( float startAlpha, float endAlpha, float duration, float delay, bool jumpToStartAlpha, System.Action onComplete = null, System.Action<float> onUpdate = null ){
		Fade( _baseColor, startAlpha, endAlpha, duration, delay, jumpToStartAlpha, onComplete, onUpdate );
	}

	public static void FadeUp( float duration, float delay, System.Action onComplete = null, System.Action<float> onUpdate = null ){
		Fade( _baseColor, _currentColor.a, 1, duration, delay, false, onComplete, onUpdate );
	}
	

	public static void FadeDown( float duration, float delay, System.Action onComplete = null, System.Action<float> onUpdate = null ){
		Fade( _baseColor, _currentColor.a, 0, duration, delay, false, onComplete, onUpdate );	
	}
	#endregion
	
	#region Private Methods
	static void CheckInstance(){
		if ( _instance == null ) {
			
			//create singleton:
			GameObject screenFadeGameObject = new GameObject( "Screen Fade" );
			DontDestroyOnLoad( screenFadeGameObject );
			_instance = screenFadeGameObject.AddComponent<ScreenFade>();
			
			//create texture:
			_texture = new Texture2D( 1, 1, TextureFormat.ARGB32, false );
			ChangeColor( _currentColor, false );
		}
	}
	#endregion
	
	#region Coroutines
	IEnumerator DoFade( float duration, float delay, System.Action onComplete, System.Action<float> onUpdate ){
		if ( _startColor == _endColor ) {
			yield break;
		}
		
		if ( delay > 0 ) {
			yield return new WaitForSeconds( delay );
		}
		
		if ( _currentColor.a < _endColor.a ) {
			_isFadingUp = true;
		}else{
			_isFadingUp = false;	
		}
		
		float startTime = Time.realtimeSinceStartup;

		if ( OnFadeBegin != null ) OnFadeBegin();

		while (true) {
			float percentage = ( Time.realtimeSinceStartup - startTime ) / duration;
			if ( OnFadeUpdate != null ) OnFadeUpdate( percentage );
			if ( onUpdate != null ) onUpdate( percentage );
			_currentColor = Color.Lerp( _startColor, _endColor, percentage );
			if ( percentage >= 1 ) {
				_currentColor = _endColor;
				if ( OnFadeEnd != null) OnFadeEnd();
				if ( onComplete != null ) onComplete();
				yield break;
			}
			yield return null;
		}
	}
	#endregion
}
