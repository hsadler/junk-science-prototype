using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{


	public GameObject playerCameraGO;
	public float carryDistance;
	public float carrySmooth;


	private Camera playerCamera;
	private bool isCarrying = false;
	private GameObject carriedObject;


	// UNITY HOOKS

	void Start()
    {
		this.playerCamera = playerCameraGO.GetComponent<Camera>();
	}

    void Update()
    {
        if (LabSceneManager.instance.playerActive)
        {
            this.CheckAimInteractions();
        }
		if (this.isCarrying)
		{
			this.Carry(carriedObject);
			this.CheckDrop();
		}
		else
		{
			this.CheckPickup();
		}

	}

    // IMPLEMENTATION METHODS

    private void CheckAimInteractions()
    {
		RaycastHit hit;
		Ray ray = this.playerCamera.ViewportPointToRay(
			new Vector3(0.5F, 0.5F, 0)
		);
		if (Physics.Raycast(ray, out hit))
		{
			GameObject objectHit = hit.transform.gameObject;
			var playerInteractionMessage = new PlayerInteractionMessage(
				gameObject,
				hit
			);
			// send standard ray hit message
			objectHit.SendMessageUpwards(
				"PlayerRayHitInteraction",
				playerInteractionMessage,
				SendMessageOptions.DontRequireReceiver
			);
			if (Input.GetMouseButtonDown(0))
			{
				objectHit.SendMessageUpwards(
					"PlayerLeftClickInteraction",
					playerInteractionMessage,
					SendMessageOptions.DontRequireReceiver
				);
			}
			else if (Input.GetMouseButtonDown(1))
			{
				objectHit.SendMessageUpwards(
					"PlayerRightClickInteraction",
					playerInteractionMessage,
					SendMessageOptions.DontRequireReceiver
				);
			}
			else if (Input.GetKeyDown(KeyCode.F))
			{
				objectHit.SendMessageUpwards(
					"PlayerFKeyInteraction",
					playerInteractionMessage,
					SendMessageOptions.DontRequireReceiver
				);
			}
		}
	}

	void Carry(GameObject go)
	{
		go.transform.position = Vector3.Lerp(
			go.transform.position,
			this.playerCameraGO.transform.position + this.playerCameraGO.transform.forward * this.carryDistance,
			Time.deltaTime * this.carrySmooth
		);
	}

	void CheckPickup()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = this.playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			if (Physics.Raycast(ray, out hit))
			{
				bool pickupable = (hit.transform.gameObject.GetComponent<PickupableScript>() != null);
				if (pickupable)
				{
					this.isCarrying = true;
					this.carriedObject = hit.transform.gameObject;
				}
			}
		}
	}

	void CheckDrop()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.DropObject();
		}
	}

	void DropObject()
	{
		this.isCarrying = false;
		this.carriedObject = null;
	}


}
