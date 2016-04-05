using UnityEngine;

[AddComponentMenu("Image Effects/Letterbox")]
[ExecuteInEditMode, RequireComponent(typeof(Camera))]
public class Letterbox : MonoBehaviour {
	public float Aspect = 21f / 9f;
	
	public Shader shader;
	
	Material material;

	void OnEnable() {
		if (!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}

		shader = Shader.Find("Hidden/Letterbox");
		
		if (!shader || !shader.isSupported)
		{
			enabled = false;
		}
	}

	void Apply(Texture source, RenderTexture destination) {
		if (source is RenderTexture)
		{
			OnRenderImage(source as RenderTexture, destination);
			return;
		}

		RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
		Graphics.Blit(source, rt);
		OnRenderImage(rt, destination);
		RenderTexture.ReleaseTemporary(rt);
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		if (material == null)
		{
			shader.hideFlags = HideFlags.NotEditable;
			material = new Material(shader);
			material.hideFlags = HideFlags.HideAndDontSave;
		}

		float w = (float)source.width;
		float h = (float)source.height;
		float currentAspect = w / h;
		float offset = 0;
		int pass = 0;

		if (currentAspect < Aspect)
		{
			offset = (h - w / Aspect) * 0.5f / h;
		}
		else if (currentAspect > Aspect)
		{
			offset = (w - h * Aspect) * 0.5f / w;
			pass = 1;
		}
		else
		{
			Graphics.Blit(source, destination);
			return;
		}

		material.SetVector("_Offsets", new Vector2(offset, 1 - offset));
		Graphics.Blit(source, destination, material, pass);
	}
}