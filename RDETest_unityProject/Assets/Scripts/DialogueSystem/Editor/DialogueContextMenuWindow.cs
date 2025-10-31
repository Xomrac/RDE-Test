using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	public class DialogueContextMenuWindow : ScriptableObject,ISearchWindowProvider
	{
		private DialogueGraphView _graphView;
		private Dialogue _dialogue;
		private DialogueGraphWindow _owner;

		public void Init(DialogueGraphView view, Dialogue dialogue, DialogueGraphWindow owner)
		{
			_graphView = view;
			_dialogue = dialogue;
			_owner = owner;
		}

		public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
		{
			return new List<SearchTreeEntry>
			{
				new SearchTreeGroupEntry(new GUIContent("Create Node")),

				new SearchTreeEntry(new GUIContent("Dialogue Node"))
				{
					level = 1,
					userData = "beat-node"
				}
			};
		}
		public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
		{
			switch (entry.userData)
			{
				case "beat-node":
					// 1) screen → window
					Vector2 windowPos = context.screenMousePosition - _owner.position.position;
					// 2) window (root local) → world
					Vector2 worldPos = _owner.rootVisualElement.LocalToWorld(windowPos);
					// 3) world → graph content local
					Vector2 graphPos = _graphView.contentViewContainer.WorldToLocal(worldPos);
					_graphView.AddBeatNode(_dialogue,graphPos);
					return true;

				default:
					return false;
			}
		}
	}

}