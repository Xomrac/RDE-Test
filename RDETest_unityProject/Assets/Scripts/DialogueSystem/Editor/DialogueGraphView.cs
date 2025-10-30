namespace XomracCore.DialogueSystem.DialogueSystem
{

	using UnityEngine.UIElements;
	using UnityEditor.Experimental.GraphView;

	public class DialogueGraphView : GraphView
	{
		private readonly Dialogue _currentDialogue;
		public Dialogue CurrentDialogue => _currentDialogue;
		
		private bool _hasStartNode = false;

		public DialogueGraphView(Dialogue data)
		{
			_currentDialogue = data;
			style.flexGrow = 1;
			SetupInteractions();
			AddGrid();
		}
		
		public void AddBeatNode(Dialogue dialogue)
		{
			ANodeDisplayer node = new BeatNodeDisplayer()
				.WithTitle("New Beat")
				.WithGuid(System.Guid.NewGuid().ToString())
				.WithView(this);
			(node as BeatNodeDisplayer).WithSpeaker(dialogue.DefaultSpeaker);
			AddElement(node);
		}
		
		public void AddStartNode()
		{
			if (_hasStartNode) return;
			ANodeDisplayer node = new StartNodeDisplayer()
				.WithTitle("Start Node")
				.WithGuid(System.Guid.NewGuid().ToString())
				.WithView(this);
			AddElement(node);
			_hasStartNode = true;
		}

		private void SetupInteractions()
		{
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());
		}
		
		private void AddGrid()
		{
			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
		}
	}

}