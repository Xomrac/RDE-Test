using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XomracCore.DialogueSystem
{

	public static class DialogueManager
	{
		private static DialogueDisplayer _displayer;
		public static event Action DialogueFinished;
		
		public static DialoguePlayer StartDialogue(Dialogue dialogue)
		{
			if (dialogue == null)
			{
				Debug.LogError("Dialogue is null. Cannot start dialogue.");
				DialogueFinished?.Invoke();
				return null;
			}
			var player = new DialoguePlayer(dialogue);
			_displayer = Object.FindFirstObjectByType<DialogueDisplayer>();
			if (_displayer == null)
			{
				Debug.LogError("No DialogueDisplayer found in the scene. Please add one to display dialogues.");
				DialogueFinished?.Invoke();
				return null;
			}
			_displayer.ShowDialogue(player);
			player.ContinueDialogue();
			player.DialogueFinished += () =>
			{
				DialogueFinished?.Invoke();
			};
			return player;
		}
		
		
	}

}