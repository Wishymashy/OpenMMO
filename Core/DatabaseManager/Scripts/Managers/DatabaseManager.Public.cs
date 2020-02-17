
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using Mirror;

namespace OpenMMO.Database
{
	
	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================= PUBLIC METHODS ==================================
		
    	// -------------------------------------------------------------------------------
		// CreateDefaultDataPlayer
		// called when a new player is registered, the hook is executed on all modules and
		// used to parse default data onto the player (like starting Equipment etc.).
		// -------------------------------------------------------------------------------
		public void CreateDefaultDataPlayer(GameObject player)
		{
			this.InvokeInstanceDevExtMethods(nameof(CreateDefaultDataPlayer), player); //HOOK
		}

        // -------------------------------------------------------------------------------
        // LoadDataPlayerPriority
        // Called at the start of LoadDataPlayer, before the rest of the method is called
        // -------------------------------------------------------------------------------
        public virtual void LoadDataPlayerPriority(GameObject prefab, GameObject player)
        {
			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayerPriority), player); //HOOK
        }
		
		// -------------------------------------------------------------------------------
		// LoadDataPlayer
		// called when a player is loaded from the database, the hooks are executed on
		// all modules and used to load additional player data.
		// -------------------------------------------------------------------------------
		public GameObject LoadDataPlayer(GameObject prefab, string _name)
		{
			GameObject player = Instantiate(prefab);
			player.name = _name;

            LoadDataPlayerPriority(prefab, player);

			this.InvokeInstanceDevExtMethods(nameof(LoadDataPlayer), player); //HOOK
			return player;
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataUser
		// called when a user is saved to the database, the hook is executed on all
		// modules and used to save additional data.
		// -------------------------------------------------------------------------------
		public void SaveDataUser(string username, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataUser), username); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		// -------------------------------------------------------------------------------
		// SaveDataPlayer
		// called when a player is saved to the database, the hook is executed on all
		// modules and used to save additional data.
		// -------------------------------------------------------------------------------
		public void SaveDataPlayer(GameObject player, bool useTransaction = true)
		{
			if (useTransaction)
				databaseLayer.BeginTransaction();
			
			this.InvokeInstanceDevExtMethods(nameof(SaveDataPlayer), player); //HOOK
			
			if (useTransaction)
				databaseLayer.Commit();
		}
		
		// -------------------------------------------------------------------------------
		// LoginUser
		// @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginUser(string name)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginUser), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LogoutUser
		// @NetworkManager
		// -------------------------------------------------------------------------------
		public void LogoutUser(string name)
		{
			SaveDataUser(name, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutUser), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LoginPlayer
		// @NetworkManager
		// -------------------------------------------------------------------------------
		public void LoginPlayer(string name)
		{
			this.InvokeInstanceDevExtMethods(nameof(LoginPlayer), name); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// LogoutPlayer
		// @NetworkManager
		// -------------------------------------------------------------------------------
		public void LogoutPlayer(GameObject player)
		{
			SaveDataPlayer(player, false);
			this.InvokeInstanceDevExtMethods(nameof(LogoutPlayer), player); //HOOK
        }
		
		// -------------------------------------------------------------------------------

	}

}

// =======================================================================================