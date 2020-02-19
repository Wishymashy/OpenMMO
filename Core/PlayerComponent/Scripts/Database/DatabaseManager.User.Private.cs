
using OpenMMO;
using OpenMMO.Database;
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace OpenMMO.Database
{

	// ===================================================================================
	// DatabaseManager
	// ===================================================================================
	public partial class DatabaseManager
	{
		
		// ============================= PRIVATE METHODS =================================
		
		// -------------------------------------------------------------------------------
		// Init_User
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(Init))]
		void Init_User()
		{
	   		CreateTable<TableUser>();
		}
		
		// -------------------------------------------------------------------------------
		// LoadDataWithPriority_User
		// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayerPriority))]
		void LoadDataPlayerPriority_User(GameObject player)
		{
			/*
				users do not load priority data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}
		
	   	// -------------------------------------------------------------------------------
	   	// LoadDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(LoadDataPlayer))]
		void LoadDataPlayer_User(GameObject player)
		{
	   		/*
				users do not load any data, feel free to add your own
				
				instead, user data is saved/loaded as part of the register/login process
			*/
		}

		// -------------------------------------------------------------------------------
	   	// SaveDataPlayer_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataPlayer))]
		void SaveDataPlayer_User(GameObject player)
		{
			string userName = player.GetComponent<PlayerComponent>().tablePlayer.username;
			// TODO:
			// saving the time here locks us out
			//Execute("UPDATE "+nameof(TableUser)+" SET lastlogin=? WHERE username=?", DateTime.UtcNow, userName);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginPlayer_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginPlayer))]
	   	void LoginPlayer_User(string playername, string username)
	   	{
	   		// -- we update lastlogin of user only when a player character logs in (otherwise we lock ourselves out)
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastlogin=? WHERE username=?", DateTime.UtcNow, username);
	   	}
		
	   	// -------------------------------------------------------------------------------
	   	// SaveDataUser_User
	   	// -------------------------------------------------------------------------------
		[DevExtMethods(nameof(SaveDataUser))]
		void SaveDataUser_User(string username)
		{
			// -- we update lastaved and lastlogin in this case to update the login timeout check
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastsaved=? WHERE username=?", DateTime.UtcNow, username);
		}
		
		// -------------------------------------------------------------------------------
	   	// LoginUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LoginUser))]
	   	void LoginUser_User(string username)
	   	{
	   		// -- Note: We do NOT set the lastlogin time here as it would lock us out!
	   	}
		
		// -------------------------------------------------------------------------------
	   	// LogoutUser_User
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(LogoutUser))]
	   	void LogoutUser_User(string username)
	   	{
	   		// -- this resets lastlogin to allow immediate re-login
	   		Execute("UPDATE "+nameof(TableUser)+" SET lastlogin=? WHERE username=?", DateTime.MinValue, username);
	   	}
		
		// -------------------------------------------------------------------------------
	   	// DeleteDataUser_User
	   	// Note: This one is not called "DeleteDataPlayer" because its the user, not a player
	   	// -------------------------------------------------------------------------------
	   	[DevExtMethods(nameof(DeleteDataUser))]
	   	void DeleteDataUser_User(string name)
	   	{
	   		Execute("DELETE FROM "+nameof(TableUser)+" WHERE username=?", name);
	   	}
	   	
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================