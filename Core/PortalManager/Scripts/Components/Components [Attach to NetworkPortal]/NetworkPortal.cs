﻿
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Database;
using OpenMMO.UI;
using OpenMMO.DebugManager;
using OpenMMO.Portals;

namespace OpenMMO.Portals
{

	// ===================================================================================
	// NetworkPortal
	// ===================================================================================
	[DisallowMultipleComponent]
	public class NetworkPortal : BasePortal
	{
	
		[Header("Teleportation")]
		[Tooltip("Target Network Zone to teleport to (optional)")]
		public NetworkZoneTemplate targetZone;
		[Tooltip("Anchor name in the target scene to teleport to")]
		public string targetAnchor;
		
		// -------------------------------------------------------------------------------
		// OnTriggerEnter
		// @Client / @Server
		// -------------------------------------------------------------------------------
		public override void OnTriggerEnter(Collider co)
		{

			PlayerComponent pc = co.GetComponentInParent<PlayerComponent>();
			
			if (pc == null || !pc.IsLocalPlayer)
				return;
			
			if (!triggerOnEnter)
			{
			
				if (pc.CheckCooldown)
					UIPopupPrompt.singleton.Init(String.Format(popupEnter, targetZone.title), OnClickConfirm);
				else
					UIPopupNotify.singleton.Init(String.Format(popupWait, pc.GetCooldownTimeRemaining().ToString("F0")));
					
			}
			else
				OnClickConfirm();
				
		}
		
		// -------------------------------------------------------------------------------
		// OnClickConfirm
		// @Client
		// -------------------------------------------------------------------------------
		public override void OnClickConfirm()
		{
		
			GameObject player = PlayerComponent.localPlayer;
			
			if (!player)
				return;
				
			if (player && targetZone != null && !String.IsNullOrWhiteSpace(targetAnchor))
				player.GetComponent<PlayerComponent>().Cmd_WarpRemote(targetAnchor, targetZone.name);
			
			base.OnClickConfirm();
			
		}
		
    	// -------------------------------------------------------------------------------
    	
	}

}

// =======================================================================================