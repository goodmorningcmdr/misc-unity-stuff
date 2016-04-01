using UnityEngine;
using UnityEditor;
using System.Collections;

public class GenerateCubemaps : MonoBehaviour {
	
    public static void GenerateCubemapss (bool select,int resolution,Color reflectcolor, LayerMask layera)
    {
        if (!System.IO.Directory.Exists("Assets/Textures/Cubemaps/"))
        {
            System.IO.Directory.CreateDirectory(Application.dataPath.Substring(0, Application.dataPath.Length - 6) + "Assets/Textures/Cubemaps/");
        }
	    
		GameObject[] gos;
		
		if (Selection.gameObjects.Length > 0)
		{
			gos = Selection.gameObjects;
		}
		else 
		{
			gos = null;
			EditorGUILayout.HelpBox("Nothing is selected", MessageType.Warning);
			return;
		}
		
	    EditorUtility.DisplayProgressBar("Initilizing Cubemaps", "Prepering...", 0.0f);
        
	    int j=0;
	    int all= gos.Length;
	    foreach (GameObject g in gos) { 
            if (g.GetComponent<Renderer>()!=null)
			{
                foreach (Material item in g.GetComponent<Renderer>().sharedMaterials)
                {
					if (item.name.Contains("Default-Material")) continue;

					item.shader = Shader.Find("Legacy Shaders/Reflective/Diffuse");

                    if (item.HasProperty("_Cube"))
                    {
                        EditorUtility.DisplayProgressBar("Initilizing Cubemaps", "Processing object " + g.name, 1.0f * j / all);
						
						item.SetColor("_ReflectColor", reflectcolor);
                        g.GetComponent<Renderer>().enabled = false;
                        GameObject go = new GameObject("CubemapCamera", typeof(Camera));
                        go.GetComponent<Camera>().transform.position = g.GetComponent<Renderer>().bounds.center;
                        go.GetComponent<Camera>().transform.rotation = Quaternion.identity;
                        go.GetComponent<Camera>().cullingMask = layera;
                        go.GetComponent<Camera>().nearClipPlane = 0.01f;
                        Cubemap cubemap = new Cubemap(resolution, TextureFormat.ARGB32, false);
                        for (int i = 0; i < resolution; i++)
                        {
                            for (int k = 0; k < 6; k++)
                            {
                                cubemap.SetPixel((CubemapFace) k, i, j, Color.white);    
                            }
                        }
                        
                        go.GetComponent<Camera>().RenderToCubemap(cubemap);
						
                        GameObject.DestroyImmediate(go);
                        AssetDatabase.CreateAsset(cubemap, "Assets/Textures/Cubemaps/" + g.name + "_" + item.name + ".cubemap");
                        g.GetComponent<Renderer>().enabled = true;
						AssetDatabase.Refresh();
                        Cubemap lm = (Cubemap)AssetDatabase.LoadAssetAtPath("Assets/Textures/Cubemaps/" + g.name + "_" + item.name + ".cubemap", typeof(Object));

                        item.SetTexture("_Cube", lm);
                    }
                }
		    }
		    j++;
	    }
	    EditorUtility.ClearProgressBar();
    }
}
