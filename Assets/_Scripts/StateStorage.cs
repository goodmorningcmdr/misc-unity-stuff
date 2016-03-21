using UnityEngine;
using System.Xml.Serialization;
using System.IO;

// usage: StateStorage.LoadData<SomeClass>("SomeClassStats");

public class StateStorage {	
	public static T LoadData<T>(string key) {
		if (PlayerPrefs.HasKey(key))
		{		
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StringReader sr = new StringReader(PlayerPrefs.GetString(key));
			return (T)serializer.Deserialize(sr);
		}
		else
		{
			return (T)System.Activator.CreateInstance(typeof(T));
		}
	}	
	
	public static void SaveData<T>(string key, T source) {
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StringWriter sw = new StringWriter();
		serializer.Serialize(sw, source);
		PlayerPrefs.SetString(key, sw.ToString());
	}
	
	public static void ClearData(string key) {
		PlayerPrefs.DeleteKey(key);
	}
	
	public static void CommitData() {
		PlayerPrefs.Save();	
	}
}