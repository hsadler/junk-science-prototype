using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{


	public GameObject playerCameraGO;
	public float maxPickupDistance = 100f;
	public float maxCarrySpeedPerSecond = 100f;
	public float carrySmooth = 1f;
	public float rotateSpeed;
	public float rotateSmooth;

	private Camera playerCamera;
	private bool isCarrying = false;
	private float carryDistance;
	private GameObject carriedObject;

	private Quaternion lastRotation = Quaternion.identity;


	// UNITY HOOKS

	void Start()
    {
		this.playerCamera = playerCameraGO.GetComponent<Camera>();
	}

    void Update()
    {
		// aim and clicks
        if (LabSceneManager.instance.playerActive)
        {
            this.CheckAimInteractions();
        }
		// carrying objects
		if (this.isCarrying)
		{
			this.CarryObject(carriedObject);
			this.CheckAndTurnObject(carriedObject);
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
		Vector3 cameraPos = this.playerCamera.transform.position;
		Vector3 direction = this.playerCamera.transform.forward;
		float distance = Mathf.Infinity;
		int playerInteractableMask = 1 << 8;
		if (Physics.Raycast(cameraPos, direction, out hit, distance, playerInteractableMask))
		{
			GameObject objectHit = hit.transform.gameObject;
			var playerInteractionMessage = new PlayerInteractionMessage(
				gameObject,
				hit
			);
			// send standard aim hit message
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

	private void CheckPickup()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Vector3 cameraPos = this.playerCamera.transform.position;
			Vector3 direction = this.playerCamera.transform.forward;
			float distance = Mathf.Infinity;
			int playerInteractableMask = 1 << 8;
			if (Physics.Raycast(cameraPos, direction, out hit, distance, playerInteractableMask))
			{
				bool pickupable = (hit.transform.gameObject.GetComponent<PickupableScript>() != null);
				if (pickupable && hit.distance < this.maxPickupDistance)
				{
					this.isCarrying = true;
					this.carryDistance = hit.distance;
					this.carriedObject = hit.transform.gameObject;
					this.carriedObject.GetComponent<Rigidbody>().isKinematic = true;
				}
			}
		}
	}

	private void CheckDrop()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.DropObject();
		}
	}

	private void CarryObject(GameObject go)
	{
        Vector3 newPos = Vector3.Lerp(
            go.transform.position,
            this.playerCameraGO.transform.position + this.playerCameraGO.transform.forward * this.carryDistance,
            Time.deltaTime * this.carrySmooth
        );
		// ALT IMPLEMENTATION
		//Vector3 newPos = Vector3.MoveTowards(
		//	go.transform.position,
		//	this.playerCameraGO.transform.position + this.playerCameraGO.transform.forward * this.carryDistance,
		//	this.maxCarrySpeedPerSecond * Time.deltaTime
		//);
		go.transform.position = newPos;	
	}

	private void CheckAndTurnObject(GameObject go)
	{
		float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		bool isMouseWheelUp = (mouseWheel > 0);
		bool isMouseWheelDown = (mouseWheel < 0);
		Vector3 rotVector = Vector3.zero;
		if (isMouseWheelUp)
		{
			rotVector = Vector3.forward;
		}
		else if (isMouseWheelDown)
		{
			rotVector = Vector3.back;
		}
		if (rotVector != Vector3.zero)
		{
			Quaternion toRotation = go.transform.localRotation *
				Quaternion.AngleAxis(this.rotateSpeed, rotVector);
			go.transform.rotation = Quaternion.Lerp(
				go.transform.rotation,
				toRotation,
				Time.deltaTime * this.rotateSmooth
			);
			lastRotation = toRotation;
		}
		else
		{
			if (lastRotation != Quaternion.identity) {
				go.transform.rotation = Quaternion.Lerp(
					go.transform.rotation,
					lastRotation,
					Time.deltaTime * this.rotateSmooth
				);
			}
		}
	}

	private void DropObject()
	{
		this.carriedObject.GetComponent<Rigidbody>().isKinematic = false;
		this.isCarrying = false;
		this.carriedObject = null;
		this.lastRotation = Quaternion.identity;
	}


}
