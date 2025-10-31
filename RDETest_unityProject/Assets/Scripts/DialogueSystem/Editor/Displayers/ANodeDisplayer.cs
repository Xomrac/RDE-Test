using UnityEngine.UIElements;

namespace XomracCore.DialogueSystem
{

	using UnityEngine;
	using DialogueSystem;
	using UnityEditor.Experimental.GraphView;

	public class ANodeDisplayer : Node
	{
		protected const int DEFAULT_WIDTH = 250;
		protected const int DEFAULT_HEIGHT = 250;

		protected const int DEFAULT_X_POSITION = 200;
		protected const int DEFAULT_Y_POSITION = 200;

		private string _guid;
		public string Guid => _guid;

		private DialogueGraphView _view;
		public DialogueGraphView View => _view;
		
		private readonly EdgeConnectorListener _listener = new();
		public EdgeConnectorListener Listener => _listener;

		#region Builder Methods
			
		public ANodeDisplayer()
		{
			SetPosition(new Rect(DEFAULT_X_POSITION, DEFAULT_Y_POSITION, DEFAULT_WIDTH, DEFAULT_HEIGHT));
		}

		public ANodeDisplayer WithTitle(string newTitle)
		{
			title = newTitle;
			return this;
		}

		public ANodeDisplayer WithGuid(string newGuid)
		{
			_guid = newGuid;
			return this;
		}

		public ANodeDisplayer AtPosition(Vector2 newPosition)
		{
			var rect = GetPosition();
			rect.x = newPosition.x;
			rect.y = newPosition.y;
			SetPosition(rect);
			return this;
		}

		public ANodeDisplayer WithSize(Vector2 newSize)
		{
			Rect rect = GetPosition();
			rect.width = newSize.x;
			rect.height = newSize.y;
			SetPosition(rect);
			return this;
		}

		public ANodeDisplayer WithView(DialogueGraphView newView)
		{
			_view = newView;
			return this;
		}

		#endregion
			
		protected virtual Port CreateInputPort(string portName = "Input")
		{
			var inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(DialoguePort));
			inputPort.portName = portName;
			return inputPort;
		}
		protected virtual Port CreateOutputPort(string portName = "Next", object data = null)
		{
			Port outputPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(DialoguePort));
			outputPort.portName = portName;
			outputPort.userData = data;
			outputPort.AddManipulator(new CustomEdgeConnector(Listener));
			return outputPort;
		}
	}

}