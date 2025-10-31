using System;
using UnityEngine;

namespace XomracCore.DialogueSystem
{

	public class DialogueManager : MonoBehaviour
	{
		[SerializeField] private DialogueDisplayer _displayer;
		[SerializeField] private Dialogue _testDialogue;
		
		private void Start()
		{
			var player = new DialoguePlayer(_testDialogue);
			_displayer.ShowDialogue(player);
			player.ContinueDialogue();
		}
	}

}