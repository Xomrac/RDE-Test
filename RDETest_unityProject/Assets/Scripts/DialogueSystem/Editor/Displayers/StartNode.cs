namespace XomracCore.DialogueSystem
{

	public class StartNodeDisplayer : ANodeDisplayer
	{
		public StartNodeDisplayer()
		{
			outputContainer.Add(CreateOutputPort());
			RefreshExpandedState();
			RefreshPorts();
		}
	}

}