using UnityEngine;

public class AnimationHelper : MonoBehaviour {

	public Animation anim;
	public string[] animations;
	public float[] newSpeed;
	public WrapMode[] wrapMode;
	public bool playAutomatically;
	
	void Awake() {
		if (!anim) 
		{
			anim = GetComponent<Animation>();
		}

		for (int i = 0; i < animations.Length; i++)
		{
			if (newSpeed.Length <= i) anim[animations[i]].speed = newSpeed[i];
			if (wrapMode.Length <= i) anim[animations[i]].wrapMode = wrapMode[i];		
			anim.playAutomatically = playAutomatically;
		}
		
		if (playAutomatically) anim.Play();
	}
}
