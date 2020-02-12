﻿using System;
using OpenMMO;
using OpenMMO.Network;
using OpenMMO.UI;
using OpenMMO.Chat;
using UnityEngine;
using UnityEngine.UI;

namespace OpenMMO.UI
{

	// ===================================================================================
	// UIWindowChat
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class UIWindowChat : UIRoot
	{
		
		[Header("Window")]
		public GameObject windowRoot;
		
		[Header("Prefab")]
		public UIChatSlot slotPrefab;
		
		[Header("Content")]
		public Transform contentViewport;
		public ScrollRect scrollRect;
		
		[Header("Buttons")]
		public Image toggleButtonImage;
		public Button sendButton;
		
		[Header("Send Input Field")]
		public InputField sendInputField;
		
		[Header("Channel Buttons")]
		public Button publicChannelButton;
		public Button privateChannelButton;
		public Button guildChannelButton;
		public Button partyChannelButton;
		public Button infoChannelButton;
		
		[Header("Channel Id")]
		public string channelIdPublic 	= "public";
		public string channelIdPrivate 	= "private";
		public string channelIdGuild 	= "guild";
		public string channelIdParty 	= "party";
		public string channelIdInfo		= "info";
		
		[Header("Used Images")]
		public Sprite maximizedImage;
		public Sprite minimizedImage;
		
		[Header("Enter Keys")]
		public KeyCode[] enterKeys = {KeyCode.Return, KeyCode.KeypadEnter};
		
		public int maxMessages = 100;
		
		public static UIWindowChat singleton;
		
		protected string channelId = "public";
		
		protected bool inputActive;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// Show
		// -------------------------------------------------------------------------------
		public override void Show()
		{
			base.Show();
		}
		
		// -------------------------------------------------------------------------------
		// Update
		// -------------------------------------------------------------------------------
		protected override void Update()
		{
			
			// -- check for 'Enter' pressed while Input has focus
			
			foreach (KeyCode enterKey in enterKeys)
				if (Input.GetKeyDown(enterKey) && inputActive)
					OnClickSendMessage();
			
			base.Update();
		
		}
		
		// -------------------------------------------------------------------------------
		// ThrottledUpdate
		// -------------------------------------------------------------------------------
		protected override void ThrottledUpdate()
		{
		
			sendButton.interactable = !String.IsNullOrWhiteSpace(sendInputField.text);
			sendButton.onClick.SetListener(() 				=> { OnClickSendMessage(); });
			
			publicChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelPublic(); });
			privateChannelButton.onClick.SetListener(() 	=> { OnClickSwitchChannelPrivate(); });
			guildChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelGuild(); });
			partyChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelParty(); });
			infoChannelButton.onClick.SetListener(() 		=> { OnClickSwitchChannelInfo(); });
			
		}
						
		// =============================== BUTTON HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnClickSendMessage
		// -------------------------------------------------------------------------------
		public void OnClickSendMessage()
		{	
			
			if (String.IsNullOrWhiteSpace(sendInputField.text))
				return;
			
			ChatManager.singleton.ClientChatSend(channelId, sendInputField.text);
			
			sendInputField.text = String.Empty;
			inputActive = false;
			
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelPublic
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelPublic()
		{	
			channelId = channelIdPublic;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelPrivate
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelPrivate()
		{	
			channelId = channelIdPrivate;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelGuild
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelGuild()
		{	
			channelId = channelIdGuild;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelParty
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelParty()
		{	
			channelId = channelIdParty;
			sendButton.interactable = true;
		}
		
		// -------------------------------------------------------------------------------
		// OnClickSwitchChannelInfo
		// -------------------------------------------------------------------------------
		public void OnClickSwitchChannelInfo()
		{	
			channelId = channelIdInfo;
			sendButton.interactable = false;
		}
		
		// -------------------------------------------------------------------------------
		// OnInputFieldChange
		// -------------------------------------------------------------------------------
		public void OnInputFieldChange()
		{
			inputActive = true;
		}	
		
		// =============================== UPDATE HANDLERS ===============================
		
		// -------------------------------------------------------------------------------
		// OnReceiveChatMessage
		// -------------------------------------------------------------------------------
		public void OnReceiveChatMessage(ChatMessage message)
		{
			
			if (contentViewport.childCount >= maxMessages)
			{
				for (int i = 0; i < maxMessages / 2; ++i)
                	Destroy(contentViewport.GetChild(i).gameObject);
			}
			
			GameObject go = Instantiate(slotPrefab.gameObject, contentViewport.transform, true);
			
			go.GetComponent<UIChatSlot>().Init(message);
		
			Canvas.ForceUpdateCanvases();
       	 	scrollRect.verticalNormalizedPosition = 0;
        
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================