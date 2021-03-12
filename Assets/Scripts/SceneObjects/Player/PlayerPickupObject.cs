using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupObject : MonoBehaviour
{


	public GameObject playerCameraGO;
	public float carryDistance;
	public float smooth;

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
		if (this.isCarrying)
		{
			Carry(carriedObject);
			CheckDrop();
		}
		else
		{
			CheckPickup();
		}
	}

	// IMPLEMENTATION METHODS

	void Carry(GameObject go)
	{
		go.transform.position = Vector3.Lerp(
			go.transform.position,
			this.playerCameraGO.transform.position +
				this.playerCameraGO.transform.forward * this.carryDistance,
			Time.deltaTime * smooth
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
					carriedObject = hit.transform.gameObject;
				}
			}
		}
	}

	void CheckDrop()
	{
		if (Input.GetMouseButtonDown(0))
		{
			DropObject();
		}
	}

	void DropObject()
	{
		this.isCarrying= false;
		carriedObject = null;
	}
}
