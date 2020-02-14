
using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using OpenMMO;
using OpenMMO.Database;
using UnityEngine.AI;

namespace OpenMMO {
	
	// ===================================================================================
	// PlayerComponent
	// ===================================================================================
	[DisallowMultipleComponent]
	[System.Serializable]
	public partial class PlayerComponent : EntityComponent
	{
	
		// holds exact replica of table data as in database
		// no need to sync, can be done individually if required
		public TablePlayer tablePlayer 				= new TablePlayer();
		public TablePlayerZones tablePlayerZones 	= new TablePlayerZones();
		
        //DEPRECIATED - Just use Camera.main for now, we can cache later in the Camera Dolly if it becomes a performance issue later.
		//Camera mainCamera { get { return Camera.main; } } //TODO: This is never used and Camera.main does the same thing...consider removing
		
		// -------------------------------------------------------------------------------
		// Start
		// -------------------------------------------------------------------------------
		[ServerCallback]
		protected override void Start()
    	{
        	base.Start(); // required
		}
		
		// -------------------------------------------------------------------------------
		// OnStartLocalPlayer
		// -------------------------------------------------------------------------------
		public override void OnStartLocalPlayer()
    	{
    		base.OnStartLocalPlayer();
    		
            //DEPRECIATED - Cameras now find their own targets :)
        	//mainCamera = Camera.main;
        	//mainCamera.GetComponent<CameraOpenMMO>().target = this.transform;
        	//mainCamera.GetComponent<CameraOpenMMO>().enabled = true;
		}
		
		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		void OnDestroy()
    	{
    	
        }
		
		// -------------------------------------------------------------------------------
		// UpdateServer
		// -------------------------------------------------------------------------------
		[Server]
		protected override void UpdateServer()
		{
			base.UpdateServer();
			this.InvokeInstanceDevExtMethods(nameof(UpdateServer));
		}
		
		// -------------------------------------------------------------------------------
		// UpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void UpdateClient()
		{
			base.UpdateClient();
			this.InvokeInstanceDevExtMethods(nameof(UpdateClient));
		}
		
		// -------------------------------------------------------------------------------
		// LateUpdateClient
		// -------------------------------------------------------------------------------
		[Client]
		protected override void LateUpdateClient()
		{
			this.InvokeInstanceDevExtMethods(nameof(LateUpdateClient));
		}
		
		// -------------------------------------------------------------------------------
		// FixedClient
		// @Client
		// -------------------------------------------------------------------------------
		[Client]
		protected override void FixedUpdateClient()
		{
			
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================