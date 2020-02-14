﻿// =======================================================================================
// Attached to a UI element that stretches across the entire screen in order to hide
// all other UI elements underneath it. This is used as a background for all kinds of
// popup windows. It blocks ray-casting, so the user may only interact with the popup
// instead of clicking on other UI elements.
//
// =======================================================================================

using UnityEngine;
using System.Collections;
using OpenMMO;
using OpenMMO.UI;

namespace OpenMMO.UI
{
	
	// ===================================================================================
	// UIBackgroundLayer
	// ===================================================================================
	public partial class UIBackgroundLayer : UIRoot 
	{

		[SerializeField] protected Animator animator;
		[SerializeField] protected string fadeInTriggerName = "fadeIn";
		[SerializeField] protected string fadeOutTriggerName = "fadeOut";
		
		public static UIBackgroundLayer singleton;
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected override void Awake()
		{
			singleton = this;
			base.Awake();
		}
		
		// -------------------------------------------------------------------------------
		// FadeIn
		// -------------------------------------------------------------------------------
		public void FadeIn(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeInDelayed), delay);
			else
				FadeInDelayed();
		}
		
		// -------------------------------------------------------------------------------
		// FadeInDelayed
		// -------------------------------------------------------------------------------
		protected void FadeInDelayed()
		{
			animator.SetTrigger(fadeInTriggerName);
			Show();
		}
		
		// -------------------------------------------------------------------------------
		// FadeOut
		// -------------------------------------------------------------------------------
		public void FadeOut(float delay=0f)
		{
			if (delay > 0)
				Invoke(nameof(FadeOutDelayed), delay);
			else
				FadeOutDelayed();
		}
		
		// -------------------------------------------------------------------------------
		// FadeOutDelayed
		// -------------------------------------------------------------------------------
		protected void FadeOutDelayed()
		{
			animator.SetTrigger(fadeOutTriggerName);
			StartCoroutine(nameof(DeactivateWindow));
		}
		
		// -------------------------------------------------------------------------------
		protected IEnumerator DeactivateWindow()
		{
			yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
			Hide();
		}
		
		// -------------------------------------------------------------------------------
		
	}

}

// =======================================================================================