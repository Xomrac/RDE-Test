using UnityEditor;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	using System.Collections.Generic;
	using UnityEngine;
	using SerializedData;
	using UnityEngine.UIElements;
	using UnityEditor.Experimental.GraphView;

	public class DialogueGraphView : GraphView
	{
		private readonly Dialogue _currentDialogue;
		public Dialogue CurrentDialogue => _currentDialogue;

		private bool _hasStartNode = false;
		private bool _isLoading = false;
		private DialogueContextMenuWindow _contextMenuWindow;
		public bool saveQueued;
		public IVisualElementScheduledItem saveScheduler;
		
		

		public DialogueGraphView(Dialogue data)
		{
			_currentDialogue = data;
			style.flexGrow = 1;
			SetupInteractions();
			AddGrid();
			_isLoading = true;
			this.Load();
			_isLoading = false;
			TryAddStartNode();
			graphViewChanged += OnGraphViewChanged;
		}

		private GraphViewChange OnGraphViewChanged(GraphViewChange changes)
		{
			if (_isLoading) return changes;
			
			if (changes.elementsToRemove != null && changes.elementsToRemove.Count > 0)
			{
				var edgesToAlsoRemove = new HashSet<Edge>();
				foreach (var el in changes.elementsToRemove)
				{
					if (el is Node node)
					{
						// input ports
						for (int i = 0; i < node.inputContainer.childCount; i++)
						{
							if (node.inputContainer[i] is Port p)
							{
								foreach (var e in p.connections)
									edgesToAlsoRemove.Add(e);
							}
						}

						// output ports
						for (int i = 0; i < node.outputContainer.childCount; i++)
						{
							if (node.outputContainer[i] is Port p)
							{
								foreach (var e in p.connections)
									edgesToAlsoRemove.Add(e);
							}
						}
					}
				}

				// Aggiunge gli edge mancanti alla rimozione
				foreach (var edge in edgesToAlsoRemove)
				{
					if (!changes.elementsToRemove.Contains(edge))
						changes.elementsToRemove.Add(edge);
				}
			}

			// save automatically when there are changes
			// edges created, elements removed, or elements moved
			if (changes.edgesToCreate is {Count: > 0 } ||
				changes.elementsToRemove is {Count: > 0 } ||
				changes.movedElements is {Count: > 0 })
			{
				EditorApplication.delayCall += () => this.Save();
			}

			return changes;
		}

		private void TryAddStartNode()
		{
			if (_hasStartNode) return;
			AddStartNode();
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

		public void AddBeatNode(Dialogue dialogue, Vector2 position)
		{
			ANodeDisplayer node = new BeatNodeDisplayer()
				.WithTitle("New Beat")
				.WithGuid(System.Guid.NewGuid().ToString())
				.WithView(this);
			node.SetPosition(new Rect(position, node.GetPosition().size));
			(node as BeatNodeDisplayer).WithSpeaker(dialogue.DefaultSpeaker);
			AddElement(node);
			this.Save();
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
			this.Save();
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
		
		public void SaveGraph()
		{
			this.Save();
		}
		
		public void SetupContextMenu(DialogueGraphWindow parentWindow)
		{
			_contextMenuWindow = ScriptableObject.CreateInstance<DialogueContextMenuWindow>();
			_contextMenuWindow.hideFlags = HideFlags.HideAndDontSave;
			_contextMenuWindow.Init(this, _currentDialogue,parentWindow);


			// open when pressing Space
			nodeCreationRequest = ctx =>
			{
				SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), _contextMenuWindow);
			};

			//TODO: add right click context menu
			
		}
	}

}