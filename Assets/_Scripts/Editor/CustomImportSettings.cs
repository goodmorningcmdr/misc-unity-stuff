using UnityEngine;
using UnityEditor;

class CustomImportSettings : AssetPostprocessor {

	[MenuItem("CONTEXT/Asset/Reset")]
	static void Reset() {
		Debug.Log("reset");
	}

	void OnPreprocessTexture() {
		if (assetPath.Contains("Resources")) return; //handle resources manually

		TextureImporter importer = assetImporter as TextureImporter;

		importer.anisoLevel = 16;
		importer.mipMapBias = -0.5f;
		importer.mipmapFilter = TextureImporterMipFilter.KaiserFilter;
		importer.maxTextureSize = 4096;

		if (importer.filterMode != FilterMode.Point)
		{
			importer.filterMode = FilterMode.Trilinear;
		}

		if (assetPath.StartsWith("Assets/Textures/UI") || assetPath.StartsWith("Assets/Textures/Sprites")) //UI textures and sprites
		{
			importer.textureType = TextureImporterType.Sprite;
			importer.textureFormat = TextureImporterFormat.ARGB32;
		}

		if (assetPath.StartsWith("Assets/Textures/Colors")) //single color textures
		{
			importer.textureFormat = TextureImporterFormat.AutomaticCrunched;
			importer.maxTextureSize = 32;
			importer.anisoLevel = 0;
			importer.mipmapEnabled = false;
		}

		if (assetPath.ToLower().Contains("lut"))
		{
			importer.isReadable = true;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.linearTexture = true;
			importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			importer.mipmapEnabled = false;
		}
	}
}