using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupObject : MonoBehaviour
{


	public GameObject playerCameraGO;
	public float distance;
	public float smooth;

	private Camera playerCamera;
	private bool carrying = false;
	private GameObject carriedObject;


	// UNITY HOOKS

    void Start()
	{
		this.playerCamera = playerCameraGO.GetComponent<Camera>();
	}

	void Update()
	{
		if (carrying)
		{
			Carry(carriedObject);
			CheckDrop();
			//RotateObject();
		}
		else
		{
			Pickup();
		}
	}

	// IMPLEMENTATION METHODS

	void RotateObject()
	{
		carriedObject.transform.Rotate(5, 10, 15);
	}

	void Carry(GameObject o)
	{
		o.transform.position = Vector3.Lerp(
			o.transform.position,
			this.playerCameraGO.transform.position + this.playerCameraGO.transform.forward * distance,
			Time.deltaTime * smooth
		);
	}

	void Pickup()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//Debug.Log("Checking pickup");
			RaycastHit hit;
			Ray ray = this.playerCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			if (Physics.Raycast(ray, out hit))
			{
				//Debug.Log("Hit: " + hit.transform.gameObject.name);
				bool pickupable = (hit.transform.gameObject.GetComponent<PickupableScript>() != null);
				if (pickupable)
				{
					//Debug.Log("Picking up pickupable");
					carrying = true;
					carriedObject = hit.transform.gameObject;
                    //hit.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
		carrying = false;
		//carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		carriedObject = null;
	}
}
