using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FramesCounter : MonoBehaviour {
	const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0}";
	public bool on;

    void Start() {
		m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }

    void Update() {
		if (Input.GetKeyDown(KeyCode.F1)) on = !on;		

		m_FpsAccumulator++;
		if (Time.realtimeSinceStartup > m_FpsNextPeriod)
		{
			m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
			m_FpsAccumulator = 0;
			m_FpsNextPeriod += fpsMeasurePeriod;
		}
    }
	
	void OnGUI() {
		if (!on || Time.timeScale <= 0)
			return;
		
		GUI.skin.label.fontSize = 25;
		GUI.skin.label.normal.textColor = Color.yellow;
		GUI.Label(new Rect(Screen.width - 50, 10, 100, 50), string.Format(display, m_CurrentFps));
	}
}