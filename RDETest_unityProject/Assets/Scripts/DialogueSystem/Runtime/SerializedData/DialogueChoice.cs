using System;

namespace XomracCore.DialogueSystem.SerializedData
{

	[Serializable]
	public class DialogueChoice
	{
		public string portGuid = Guid.NewGuid().ToString();
		public string displayedValue = "Choice";
	}

}