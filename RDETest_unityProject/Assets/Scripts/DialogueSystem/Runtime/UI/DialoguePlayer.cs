using System;
using System.Linq;
using UnityEngine;
using XomracCore.DialogueSystem.SerializedData;

namespace XomracCore.DialogueSystem
{

	public class DialoguePlayer
	{
		private Dialogue _dialogue;

		public DialoguePlayer(Dialogue dialogue)
		{
			_dialogue = dialogue;
			foreach (NodeData node in dialogue.Nodes.Where(node => node is not BeatNodeData))
			{
				Debug.Log(node.guid);
				_currentNodeGuid = node.guid;
				break;
			}
		}

		public string _currentNodeGuid;

		public event Action<BeatNodeData> NewNodeReached;
		public event Action DialogueFinished;

		public void ContinueDialogue()
		{
			foreach (EdgeData edge in _dialogue.Edges.Where(edge => edge.outputNodeGuid == _currentNodeGuid))
			{
				_currentNodeGuid = edge.inputNodeGuid;
				BeatNodeData newNode = GetNodeByGuid(_currentNodeGuid);
				if (string.IsNullOrEmpty(_currentNodeGuid) || newNode == null)
				{
					FinishDialogue();
					return;
				}
				NewNodeReached?.Invoke(newNode);
				return;
			}
			FinishDialogue();
		}

		public void ContinueDialogue(string choiceText)
		{
			foreach (EdgeData edge in _dialogue.Edges.Where(edge => edge.outputNodeGuid == _currentNodeGuid && edge.outputPortDisplayedValue == choiceText))
			{
				_currentNodeGuid = edge.inputNodeGuid;
				BeatNodeData newNode = GetNodeByGuid(_currentNodeGuid);
				if (string.IsNullOrEmpty(_currentNodeGuid) || newNode == null)
				{
					FinishDialogue();
					return;
				}
				NewNodeReached?.Invoke(newNode);
				return;
			}
			FinishDialogue();
		}

		private BeatNodeData GetNodeByGuid(string guid)
		{
			foreach (NodeData node in _dialogue.Nodes)
			{
				if (node.guid == guid && node is BeatNodeData beatNode)
				{
					return beatNode;
				}
			}
			return null;
		}

		private void FinishDialogue()
		{
			DialogueFinished?.Invoke();
		}
	}

}