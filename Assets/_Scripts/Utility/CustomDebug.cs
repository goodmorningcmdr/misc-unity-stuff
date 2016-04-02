using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class CustomDebug : MonoBehaviour {
	private ILogger logger = Debug.logger;
	public LogType buildFilter = LogType.Warning;

	void Start() {
		logger.filterLogType = buildFilter;
		logger.Log("This log will be displayed only in debug build");
		logger.LogError("This log will be displayed in debug and release build", this.transform);
	}
}