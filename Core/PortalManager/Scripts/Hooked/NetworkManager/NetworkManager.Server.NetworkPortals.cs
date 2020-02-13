using OpenMMO;
using OpenMMO.Network;
using OpenMMO.Portals;
using OpenMMO.Database;
using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace OpenMMO.Network
{
	
	// ===================================================================================
	// NetworkManager
	// ===================================================================================
	[RequireComponent(typeof(PortalManager))]
	public partial class NetworkManager
	{
   		
   		// -----------------------------------------------------------------------------------
		// OnStartServer_NetworkPortals
		// @Server
		// -----------------------------------------------------------------------------------
		[DevExtMethods("OnStartServer")]
		void OnStartServer_NetworkPortals()
		{
			
			NetworkServer.RegisterHandler<ClientMessageRequestPlayerSwitchServer>(OnClientMessageRequestPlayerSwitchServer);
            NetworkServer.RegisterHandler<ClientMessageRequestPlayerAutoLogin>(OnClientMessageRequestPlayerAutoLogin);
            
            GetComponent<PortalManager>().SpawnSubZones();
		}
   		
		// -------------------------------------------------------------------------------
		// LoginPlayer_NetworkPortals
		// @Server
		// -------------------------------------------------------------------------------
		[DevExtMethods("LoginPlayer")]
		void LoginPlayer_NetworkPortals(NetworkConnection conn, GameObject player, GameObject prefab, string userName, string playerName)
		{
			
			PlayerComponent pc 				= player.GetComponent<PlayerComponent>();
			string zoneName 				= pc.tablePlayerZones.zonename;
			NetworkZoneTemplate currentZone = pc.currentZone;
			
			if (!String.IsNullOrWhiteSpace(zoneName) && zoneName != currentZone.name)
			{
				string anchorName = pc.tablePlayerZones.anchorname;
				
				// -- refresh security token
				pc.RefreshToken();
				
				// -- issue warp
				pc.WarpRemote(anchorName, zoneName, pc.GetToken);
				
			}
		
		}
		
		// -------------------------------------------------------------------------------
		// SwitchServerPlayer
		// @Server -> @Client
		// -------------------------------------------------------------------------------
		public void SwitchServerPlayer(NetworkConnection conn, string playername, string anchorName, string zoneName, int _token)
		{

			ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				playername			= playername,
				anchorname 			= anchorName,
				zonename 			= zoneName,
				token				= _token,
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(playername, anchorName, zoneName, _token))
			{
				message.text = systemText.playerSwitchServerSuccess;
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
				message.success = false;
			}
			
        	conn.Send(message);
		
		}
		
		// ======================== MESSAGE HANDLERS - PLAYER ============================
        
        // -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerAutoLogin
        // Direction: @Client -> @Server
        // Execution: @Server
		// -------------------------------------------------------------------------------     
        void OnClientMessageRequestPlayerAutoLogin(NetworkConnection conn, ClientMessageRequestPlayerAutoLogin msg)
		{
			
			ServerMessageResponsePlayerAutoLogin message = new ServerMessageResponsePlayerAutoLogin
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
			
			if (DatabaseManager.singleton.TryPlayerAutoLogin(msg.playername, msg.username))
			{
				AutoLoginPlayer(conn, msg.username, msg.playername);
				message.text = systemText.playerLoginSuccess;
			}
			else
			{
				message.text = systemText.playerLoginFailure;
				message.success = false;
			}
					
			conn.Send(message);
			
		}
		
		// -------------------------------------------------------------------------------
        // OnClientMessageRequestPlayerSwitchServer
        // Direction: @Client -> @Server
        // Execution: @Server
        // -------------------------------------------------------------------------------    
        void OnClientMessageRequestPlayerSwitchServer(NetworkConnection conn, ClientMessageRequestPlayerSwitchServer msg)
        {
        	
        	ServerMessageResponsePlayerSwitchServer message = new ServerMessageResponsePlayerSwitchServer
			{
				success 			= true,
				text			 	= "",
				causesDisconnect 	= false
			};
        	
        	if (DatabaseManager.singleton.TryPlayerSwitchServer(msg.playername, msg.anchorname, msg.zonename, msg.token))
			{
				message.text = systemText.playerSwitchServerSuccess;
			}
			else
			{
				message.text = systemText.playerSwitchServerFailure;
				message.success = false;
			}
			
        	conn.Send(message);
        	
        }
        
        // ============================== MAJOR ACTIONS ==================================
        
        // -------------------------------------------------------------------------------
		// AutoLoginPlayer
		// @Server
		// -------------------------------------------------------------------------------
		protected void AutoLoginPlayer(NetworkConnection conn, string username, string playername)
		{
			if (!UserLoggedIn(username))
			{
			
				string prefabname = DatabaseManager.singleton.GetPlayerPrefabName(playername);
				GameObject prefab = GetPlayerPrefab(prefabname);
				
				GameObject player = DatabaseManager.singleton.LoadDataPlayer(prefab, playername);
				
				// -- warp to anchor location
				PlayerComponent pc = player.GetComponent<PlayerComponent>();
				
				// -- update zone
    			pc.tablePlayerZones.zonename = pc.currentZone.name;
				
				string anchorName = pc.tablePlayerZones.anchorname;
				pc.WarpLocal(anchorName);
				
				ValidatePlayerPosition(player);
				
				NetworkServer.AddPlayerForConnection(conn, player);
				
				onlinePlayers[player.name] = player;
				state = NetworkState.Game;
				
				this.InvokeInstanceDevExtMethods(nameof(AutoLoginPlayer), conn, player, prefab, username, playername);
				eventListeners.OnLoginPlayer.Invoke(conn);

			}
			else
				ServerSendError(conn, systemText.userAlreadyOnline, true);
		}
		
                
		// -------------------------------------------------------------------------------
	
	}

}

// =======================================================================================