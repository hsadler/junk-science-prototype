using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{


	public Camera playerCamera;


    // UNITY HOOKS

    void Start()
    {
        
    }

    void Update()
    {
        if (LabSceneManager.instance.playerActive)
        {
            this.CheckAimInteractions();
        }

    }

    // IMPLEMENTATION METHODS

    private void CheckAimInteractions()
    {
		RaycastHit hit;
		Ray ray = playerCamera.ViewportPointToRay(
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


}
