
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;
using UnityEngine.AI;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================ PROTECTED METHODS ================================
		
		// -------------------------------------------------------------------------------
		// UserValid
		// -------------------------------------------------------------------------------
		protected bool UserValid(string username, string password)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=? AND password=? AND banned=0 AND deleted=0", username, password) != null;
		}
		
		// -------------------------------------------------------------------------------
		// UserExists
		// -------------------------------------------------------------------------------
		protected bool UserExists(string username)
		{
			return FindWithQuery<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE username=?", username) != null;
		}
		
		// -------------------------------------------------------------------------------
		// UserRegister
		// -------------------------------------------------------------------------------
		protected void UserRegister(string userName, string userPassword, string userEmail, string userDeviceid)
		{
			Insert(new TableUser{ username=userName, password=userPassword, email=userEmail, deviceid=userDeviceid, created=DateTime.UtcNow, lastlogin=DateTime.Now});
		}
		
		// -------------------------------------------------------------------------------
		// UserChangePassword
		// -------------------------------------------------------------------------------
		protected void UserChangePassword(string username, string oldpassword, string newpassword)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET password=? WHERE username=? AND password=?", newpassword, username, oldpassword);
		}
		
		// -------------------------------------------------------------------------------
		// UserSetDeleted
		// Sets the user to deleted (1) or undeletes it (0)
		// -------------------------------------------------------------------------------
		protected void UserSetDeleted(string username, int action=1)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET deleted=? WHERE username=?", action, username);
		}
		
		// -------------------------------------------------------------------------------
		// UserSetBanned
		// Bans (1) or unbans (0) the user
		// -------------------------------------------------------------------------------
		protected void UserSetBanned(string username, int action=1)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET banned=? WHERE username=?", action, username);
		}

        // -------------------------------------------------------------------------------
        // DeleteDataUser
        // Called when User Data has been deleted.
        // -------------------------------------------------------------------------------
        protected void DeleteDataUser(string username)
        {
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataUser), username); //HOOK
        }

		// -------------------------------------------------------------------------------
		// DeleteUser
		// Permanently deletes the user and all of its data (hard delete)
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(DeleteUser))]
		protected void DeleteUser(string username)
		{			
			this.InvokeInstanceDevExtMethods(nameof(DeleteDataPlayer), username); //HOOK 		// delete player data too

            DeleteDataUser(username);
		}
		
		// -------------------------------------------------------------------------------
		// UserSetConfirmed
		// Sets the user to confirmed (1) or unconfirms it (0)
		// -------------------------------------------------------------------------------
		protected void UserSetConfirmed(string username, int action=1)
		{
			Execute("UPDATE "+nameof(TableUser)+" SET confirmed=? WHERE username=?", action, username);
		}
		
		// -------------------------------------------------------------------------------
		// GetPlayerCount
		// returns the number of players registered on this user account
		// -------------------------------------------------------------------------------
		protected int GetPlayerCount(string username)
		{
			List<TablePlayer> result =  Query<TablePlayer>("SELECT * FROM "+nameof(TablePlayer)+" WHERE username=? AND deleted=0", username);
			
			if (result == null)
				return 0;
			else
				return result.Count;
		}
		
		// -------------------------------------------------------------------------------
		// GetUserCount
		// returns the number of user accounts registered on this device-id and/or email
		// -------------------------------------------------------------------------------
		protected int GetUserCount(string deviceId, string eMail)
		{

			List<TableUser> result = Query<TableUser>("SELECT * FROM "+nameof(TableUser)+" WHERE deviceid=? AND email=? AND deleted=0", deviceId, eMail);
			
			if (result == null)
				return 0;
			else
				return result.Count;

		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================