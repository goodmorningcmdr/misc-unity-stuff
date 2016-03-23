using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemInteraction : MonoBehaviour {
	Options Options;
	Camera cam;
	RaycastHit hit;
	float rayDist = 1;
	public Text interactText;
	public static bool looking = false;
	public Image crosshair, interactCrosshair;
	public Text highlightedText;

	Texture2D highlightTexture;

	Item currentItem;

	void Start () {
		Options = Options.getInstance();
		cam = Camera.main;
		highlightTexture = new Texture2D(1, 1);
		highlightTexture.SetPixel(1, 1, Color.white);
		highlightTexture.Apply();

		interactCrosshair.enabled = false;
		interactText.text = "";
		highlightedText.text = "";
	}
	
	float timer;

	void Update () {
		if (Time.timeScale == 0 || FPSController.disabled)
		{
			interactCrosshair.enabled = false;
			crosshair.enabled = false;
			interactText.text = "";
			highlightedText.text = "";
			return;
		}

		Ray ray = new Ray(cam.transform.position, cam.transform.forward);

		if (Physics.Raycast(ray, out hit, rayDist, LayerMaskHelper.EverythingBut(2, 30, 31)))
		{
			if (hit.transform.tag == "Interactable")
			{			
				if (Options.hints)
				{
					crosshair.enabled = false;
					interactCrosshair.enabled = true;
					DoInteractionHint(hit.transform.gameObject);
					if (hit.transform.GetComponent<Item>()) currentItem = hit.transform.GetComponent<Item>();

					if (currentItem && currentItem.highlightText.Length > 0)
					{
						highlightedText.text = currentItem.highlightText;
					}
				}
				else
				{
					DisableCrosshair();
				}

				looking = true;
			}
			else
			{
				DisableCrosshair();
			}
		}
		else
		{
			DisableCrosshair();
		}

		if (Input.GetButtonDown("Interact"))
		{
			ray = new Ray(cam.transform.position, cam.transform.forward);

			if (Physics.Raycast(ray, out hit, rayDist, LayerMaskHelper.EverythingBut(2, 30, 31)))
			{
				if (hit.transform.tag == "Interactable")
				{
					DisableCrosshair();

					if (hit.transform.GetComponent<Item>()) currentItem = hit.transform.GetComponent<Item>();

					if (currentItem && currentItem.interactedText.Length > 0)
					{
						CancelInvoke("ClearText");
						interactText.text = currentItem.interactedText;
						Invoke("ClearText", currentItem.displayTextTime);
					}
					else
					{
						if (interactedItem) RemoveInteractionHint(interactedItem);
						ClearText();
					}

					if (currentItem)
					{
						currentItem.InteractEvent();
					}
				}
			}
		}
	}

	void ClearText() {
		interactText.text = "";
	}

	void DisableCrosshair() {
		currentItem = null;

		crosshair.enabled = Options.crosshair ? true : false;

		if (interactedItem) RemoveInteractionHint(interactedItem);

		interactCrosshair.enabled = false;
		highlightedText.text = "";
		looking = false;
	}

	GameObject interactedItem;

	void DoInteractionHint(GameObject item) {
		interactedItem = item;
		foreach (Material mat in item.GetComponent<Renderer>().materials)
		{
			mat.EnableKeyword("_DETAIL_MULX2");
			mat.SetTexture("_DetailAlbedoMap", highlightTexture);
		}
	}

	void RemoveInteractionHint(GameObject item) {
		foreach (Material mat in item.GetComponent<Renderer>().materials)
		{
			mat.EnableKeyword("_DETAIL_MULX2");
			mat.SetTexture("_DetailAlbedoMap", null);
		}
		interactedItem = null;
	}
}
