using UnityEngine;

[DisallowMultipleComponent]
public class ShowLocalAxis : MonoBehaviour {
    [HideInInspector]
    public bool destroyWhenSafe = false;

    [Range(1,20)]
    public float handleLength = 1;

    public void OnDrawGizmos() {
		handleLength = (float)Mathf.Round(handleLength);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Vector3.zero, Vector3.forward * handleLength);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero, Vector3.right * handleLength);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(Vector3.zero, Vector3.up* handleLength);
    }
}