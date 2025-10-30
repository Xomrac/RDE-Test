using System.Collections.Generic;
using XomracCore.DialogueSystem.SerializedData;

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

		// automatically called by Unity to determine which ports can connect when you create/drag a connection
		public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter _)
		{
			var compatiblePorts = new List<Port>();

			ports.ForEach(port =>
			{
				if (startPort != port && startPort.node != port.node)
				{
					if (startPort.direction != port.direction)
					{
						compatiblePorts.Add(port);
					}
				}
			});

			return compatiblePorts;
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

		public void AddBeatNode(BeatNodeData nodeData)
		{
			BeatNodeDisplayer node = new();
			node.WithTitle("Beat Node")
				.WithGuid(nodeData.guid)
				.WithView(this);
			node.WithSpeaker(nodeData.speaker)
				.WithChoices(nodeData.choices)
				.WithBeat(nodeData.beat);
			node.SetPosition(nodeData.position);
			AddElement(node);
		}

		public void AddNode(NodeData data)
		{
			ANodeDisplayer node = new StartNodeDisplayer()
				.WithTitle("Start Node")
				.WithGuid(data.guid)
				.WithView(this);
			node.SetPosition(data.position);
			AddElement(node);
			_hasStartNode = true;
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

		public void SaveDialogue()
		{
			this.Save();
		}

		public void LoadDialogue()
		{
			this.Load();
		}

	}

}