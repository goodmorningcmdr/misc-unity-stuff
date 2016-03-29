using UnityEngine;

public class ShowOptions : MonoBehaviour {
	public Options Options;

	void Awake() {
		Options = Options.getInstance();
	}
}