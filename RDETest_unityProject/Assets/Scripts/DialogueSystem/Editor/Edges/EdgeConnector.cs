namespace UnityEditor.Experimental.GraphView
{

	using UnityEngine.UIElements;

	
	// Manipulator to handle edge creation and preview during drag-and-drop operations
	// makes possible to create edges by dragging from an output port to an input port
	// shows a preview edge during the drag operation
	// notifies a listener when the edge is dropped either on a valid input port or outside
	public class EdgeConnector : MouseManipulator
	{

		private readonly IEdgeConnectorListener _listener;
		private Edge _edgeCandidate;
		private GraphView _graphView;

		public EdgeConnector(IEdgeConnectorListener listener)
		{
			_listener = listener;
			activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
		}

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<MouseDownEvent>(OnMouseDown);
			target.RegisterCallback<MouseUpEvent>(OnMouseUp);
			target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
		}

		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
			target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
			target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
		}

		private void OnMouseDown(MouseDownEvent evt)
		{
			_graphView = target.GetFirstAncestorOfType<GraphView>();
			if (_graphView == null) return;
			_edgeCandidate = new Edge
			{
				output = target as Port
			};
			// create a new preview edge starting from the output port
			// make it invisible for now, it will be made visible once an input port is assigned during the drag
			// if made visible before that, it would look like a dangling edge which is not desired
			_edgeCandidate.output.Connect(_edgeCandidate);
			_graphView.AddElement(_edgeCandidate);
			_edgeCandidate.style.display = DisplayStyle.None;
		}

		private void OnMouseMove(MouseMoveEvent evt)
		{
			// make the preview visible if an input port has been assigned during the drag
			if (_edgeCandidate != null && _edgeCandidate.input != null)
			{
				_edgeCandidate.style.display = DisplayStyle.Flex;
			}
		}

		private void OnMouseUp(MouseUpEvent evt)
		{
			if (_graphView != null && _edgeCandidate != null)
			{
				
				if (_edgeCandidate.input == null)
				{
					// if you release the mouse and no input port has been assigned, it means the edge was dropped outside a port
					// remove the edge and notify the listener
					_graphView.RemoveElement(_edgeCandidate);
					_listener.OnDropOutsidePort(_edgeCandidate, evt.mousePosition);
				}
				else
				{
					_listener.OnDrop(_graphView, _edgeCandidate);
				}

				_edgeCandidate = null;
			}
		}
	}

}