
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================ PROTECTED METHODS ================================
		
		// -------------------------------------------------------------------------------
		// PlayerSetDeleted
		// Sets the player to deleted (1) or undeletes it (0)
		// -------------------------------------------------------------------------------
		protected void PlayerSetDeleted(string playername, int action=1)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET deleted=? WHERE playername=?", action, playername);
		}
		
		// -------------------------------------------------------------------------------
		// PlayerSetBanned
		// Bans (1) or unbans (0) the user
		// -------------------------------------------------------------------------------
		protected void PlayerSetBanned(string playername, int action=1)
		{
			Execute("UPDATE "+nameof(TablePlayer)+" SET banned=? WHERE playername=?", action, playername);
		}
		
		// -------------------------------------------------------------------------------
		// DeleteDataPlayer
		// Permanently deletes the player and all of its data (hard delete)
		// -------------------------------------------------------------------------------
		protected void DeleteDataPlayer(string playername)
		{			
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), playername); //HOOK
		}
		
		// -------------------------------------------------------------------------------
		// PlayerValid
		// -------------------------------------------------------------------------------
		public bool PlayerValid(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=? AND banned=0 AND deleted=0", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// PlayerExists
		// -------------------------------------------------------------------------------
		public bool PlayerExists(string playername, string username)
		{
			return FindWithQuery<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE playername=? AND username=?", playername, username) != null;
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================