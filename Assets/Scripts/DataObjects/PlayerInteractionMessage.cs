using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionMessage
{

	public RaycastHit hit;
	public GameObject player;

	public PlayerInteractionMessage(GameObject player, RaycastHit hit)
	{
		this.player = player;
		this.hit = hit;
	}

}