using UnityEngine;

namespace UnityEditor.Experimental.GraphView
{

	// class that does the connection logic when creating edges in the GraphView
	public class EdgeConnectorListener : IEdgeConnectorListener
	{

		public void OnDropOutsidePort(Edge edge, Vector2 position)
		{
			// Remove the edge if dropped outside a port
			edge.parent?.Remove(edge);
		}

		public void OnDrop(GraphView graphView, Edge edge)
		{
			graphView.AddElement(edge);
		}
	}

}