namespace XomracCore.DialogueSystem
{

	using UnityEditor.Experimental.GraphView;

	public class StartNodeDisplayer : ANodeDisplayer
	{
		public StartNodeDisplayer()
		{
			outputContainer.Add(CreateOutputPort());
			RefreshExpandedState();
			RefreshPorts();
			// make the start node not deletable
			capabilities &= ~Capabilities.Deletable;
		}
	}

}