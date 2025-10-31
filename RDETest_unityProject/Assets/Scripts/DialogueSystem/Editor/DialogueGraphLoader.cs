using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using XomracCore.DialogueSystem.SerializedData;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	public static class DialogueGraphLoader
	{
		public static void Load(this DialogueGraphView graph)
		{
			if (graph.CurrentDialogue == null) return;

			LoadNodes(graph);
			LoadEdges(graph);
		}

		private static void LoadNodes(DialogueGraphView graph)
		{
			foreach (NodeData node in graph.CurrentDialogue.Nodes)
			{
				// debug the node type
				Debug.Log("Loading node of type: " + node.GetType().Name);
				if (node is BeatNodeData beatData)
				{
					graph.AddBeatNode(beatData);
				}
				else
				{
					graph.AddNode(node);
				}
			}
		}

		private static void LoadEdges(DialogueGraphView graph)
		{
			foreach (EdgeData edgeData in graph.CurrentDialogue.Edges)
			{
				var outputNode = graph.nodes.ToList().Find(node => (node as ANodeDisplayer).Guid == edgeData.outputNodeGuid) as ANodeDisplayer;
				var inputNode = graph.nodes.ToList().Find(node => (node as ANodeDisplayer).Guid == edgeData.inputNodeGuid) as ANodeDisplayer;

				if (outputNode == null || inputNode == null) continue;

				Port outputPort;
				if (string.IsNullOrEmpty(edgeData.outputPortDisplayedValue))
				{
					outputPort = outputNode.outputContainer.Q<Port>();
				}
				else
				{
					outputPort = outputNode.outputContainer.Query<Port>().Where(port => (string)port.userData == edgeData.outputPortDisplayedValue).First();
				}
				var inputPort = inputNode.inputContainer.Q<Port>();

				if (outputPort == null || inputPort == null) continue;

				Edge edge = outputPort.ConnectTo(inputPort);
				graph.AddElement(edge);
			}
		}

	}

}