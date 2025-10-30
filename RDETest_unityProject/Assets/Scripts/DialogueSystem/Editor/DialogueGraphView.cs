using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	using UnityEditor.Experimental.GraphView;

	public class DialogueGraphView : GraphView
	{
		private readonly Dialogue _currentDialogue;
		public Dialogue CurrentDialogue => _currentDialogue;

		public DialogueGraphView(Dialogue data)
		{
			_currentDialogue = data;
			style.flexGrow = 1;
			SetupInteractions();
		}
		
		

		private void SetupInteractions()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}

	}

}