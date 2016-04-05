using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour {

	NavMeshAgent nav;
	Vector3 toPos;
	RaycastHit hit;
	public bool stopAndLook = false;
	Transform agent;
	Transform cameraToLookAt;
	public Transform neck;
	public int state = 0;
	public Vector3 defaultPos;
	public float scareTime = 20;
	public int prefState = 0;
	public string prefAnim = "Idle";
	bool dead = false;
	public GameObject blood;

	void Awake() {
		nav = GetComponent<NavMeshAgent>();

		if (defaultPos == Vector3.zero)
		{
			defaultPos = transform.position;
		}
	}

	void Start() {
		agent = Camera.main.transform;

		cameraToLookAt = agent.transform;
	}
	
	void Update() {
		if (dead)
			return;

		DoAnimations();

		switch (state)
		{
		case 0:
			DoNormal();
			break;
		case 1:
			DoScared();
			break;
		case 2:
			break;
		}
	}

	void DoAnimations() {
		if (nav.velocity.magnitude <= 0.5f)
		{
			GetComponent<Animation>().Stop();
			GetComponent<Animation>().Play(prefAnim);
		}
		else
		{
			if (!GetComponent<Animation>().IsPlaying("Walk"))
			{
				GetComponent<Animation>().Play("Walk");
			}
		}
	}

	void DoNormal() {
		if (stopAndLook)
		{
			GetComponent<Animation>().Play("Idle");
			
			Vector3 v = cameraToLookAt.position - transform.position;
			v.x = v.z = 0.0f;	
			transform.LookAt(cameraToLookAt.position - v);
		}
		
		if (!stopAndLook)
		{
			nav.speed = 2;
		}
		else if (stopAndLook)
		{
			nav.speed = 0;
		}
		
		if (!stopAndLook)
			nav.SetDestination(defaultPos);
	}

	void DoScared() {
		stopAndLook = false;

		if (nav.velocity.magnitude <= 0.5f)
		{
			toPos = agent.transform.position + transform.position  + (Random.insideUnitSphere * 25);
			toPos.y = transform.position.y;
		}

		nav.speed = 4;

		nav.SetDestination(toPos);
	}

	IEnumerator StopAndLook() {
		stopAndLook = true;
		yield return new WaitForSeconds(4);
		stopAndLook = false;
	}

	IEnumerator GetScared() {
		state = 1;
		yield return new WaitForSeconds(scareTime);
		state = prefState;
	}

	void BodyRotate() {
		Ray ray = new Ray (transform.position, Vector3.down);
		if (Physics.Raycast(ray, out hit, 1000, LayerMaskHelper.EverythingBut(gameObject.layer)))
		{
			transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		}
	}

	void DoDie() {
		if (dead)
			return;

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 25, LayerMaskHelper.OnlyIncluding(31));
		foreach (Collider col in hitColliders) 
		{
			if (col.name == "NPC")
			{
				col.GetComponent<NPC>().StopCoroutine("GetScared");
				col.SendMessage("GetScared",SendMessageOptions.DontRequireReceiver);
			}
		}

		dead = true;
		transform.root.GetComponent<NavMeshAgent>().enabled = false;
		transform.root.GetComponent<NPC>().enabled = false;
		transform.root.GetComponent<Animation>().Stop();

		Rigidbody[] rigids = transform.root.GetComponentsInChildren<Rigidbody>();
		
		foreach(Rigidbody rigid in rigids)
		{
			rigid.isKinematic = false;
		}
	}
}