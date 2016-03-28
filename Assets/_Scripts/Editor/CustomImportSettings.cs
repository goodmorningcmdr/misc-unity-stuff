using UnityEngine;
using UnityEditor;

class CustomImportSettings : AssetPostprocessor {
	void OnPreprocessTexture() {
		if (assetPath.Contains("Resources")) return; //handle resource textures manually
		TextureImporter importer = assetImporter as TextureImporter;

		if (assetPath.StartsWith("Assets/Textures/UI") || assetPath.StartsWith("Assets/Textures/Sprites")) //UI textures and sprites
		{
			importer.textureType = TextureImporterType.Sprite;
			importer.maxTextureSize = 4096;
			importer.textureFormat = TextureImporterFormat.ARGB32;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.mipmapEnabled = false;
		}
		else if (assetPath.StartsWith("Assets/Textures/Colors")) //single color textures
		{
			importer.textureFormat = TextureImporterFormat.AutomaticCrunched;
			importer.maxTextureSize = 32;
			importer.anisoLevel = 0;
			importer.mipmapEnabled = false;
		}
		else if (assetPath.StartsWith("Assets/Textures") || assetPath.StartsWith("Assets/Models"))
		{		
			importer.anisoLevel = 16;
			importer.isReadable = false;
			importer.filterMode = FilterMode.Bilinear;
			importer.maxTextureSize = 2048;
			if (!importer.normalmap) 
			{
				importer.textureType = TextureImporterType.Image | TextureImporterType.Advanced; 
				if (assetPath.ToLower().Contains("bump") || assetPath.ToLower().Contains("normal")) {importer.textureType = TextureImporterType.Bump;}
			}
		}
	}

	void OnPreprocessModel() {
		ModelImporter importer = assetImporter as ModelImporter;
		if (importer.defaultClipAnimations.Length == 0)
		{
			importer.importAnimation = false; 
			importer.animationType = ModelImporterAnimationType.None;
		}
	}
}