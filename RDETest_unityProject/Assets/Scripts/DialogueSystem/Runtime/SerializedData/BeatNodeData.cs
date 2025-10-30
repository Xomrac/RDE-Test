using System;
using System.Collections.Generic;

namespace XomracCore.DialogueSystem.SerializedData
{

	[Serializable]
	public class BeatNodeData : NodeData
	{
		public string beat;
		public Speaker speaker;
		public List<DialogueChoice> choices;
	}

}