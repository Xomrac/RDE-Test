using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using XomracCore.DialogueSystem.SerializedData;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	public static class DialogueGraphSaver
	{
		public static void Save(this DialogueGraphView graph)
		{
			if (graph.saveQueued) return;
			graph.saveQueued = true;
			EditorApplication.delayCall += () =>
			{
				graph.schedule.Execute(() =>
				{
					graph.saveQueued = false;
					SaveNodes(graph);
					SaveEdges(graph);
					graph.CurrentDialogue.Save();
				}).ExecuteLater(0);
			};
		}

		private static void SaveNodes(DialogueGraphView graph)
		{
			var tempNodes = new List<NodeData>();
			foreach (ANodeDisplayer node in graph.nodes.OfType<ANodeDisplayer>())
			{
				switch (node)
				{
					case BeatNodeDisplayer beatDisplayer:
						tempNodes.Add(beatDisplayer.AsNodeData());
						break;
					case StartNodeDisplayer startDisplayer:
						var startModel = new NodeData
						{
							guid = startDisplayer.Guid,
							position = new Rect(startDisplayer.GetPosition().position, startDisplayer.GetPosition().size)
						};
						tempNodes.Add(startModel);
						break;
				}
			}
			graph.CurrentDialogue.SetNodes(tempNodes);
		}

		private static BeatNodeData AsNodeData(this BeatNodeDisplayer displayer)
		{
			var data = new BeatNodeData
			{
				guid = displayer.Guid,
				speaker = displayer.Speaker,
				position = displayer.GetPosition(),
				beat = displayer.DisplayedBeat,
				choices = new List<DialogueChoice>(displayer.Choices)
			};

			return data;
		}

		private static void SaveEdges(DialogueGraphView graph)
		{
			var tempEdges = new List<EdgeData>();
			foreach (Edge edge in graph.edges.Where(edge => edge.input != null && edge.output != null))
			{
				if (edge.output.node is not ANodeDisplayer fromNode || edge.input.node is not ANodeDisplayer toNode) continue;

				string choiceValue = edge.output.userData as string;
				Debug.Log(choiceValue);
				var edgeModel = new EdgeData
				{
					outputNodeGuid = fromNode.Guid,
					outputPortDisplayedValue = choiceValue,
					inputNodeGuid = toNode.Guid
				};
				tempEdges.Add(edgeModel);
			}
			graph.CurrentDialogue.SetEdges(tempEdges);
		}
	}

}