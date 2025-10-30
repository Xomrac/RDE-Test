using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	public static class DialogueAssetHandler
	{
		// Priority 1 to make it runs before default handler
		[OnOpenAsset(1)] 
		public static bool OnOpenAsset(int instanceID, int line)
		{
			var asset = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
			if (asset != null)
			{
				DialogueGraphWindow.ShowWindow(asset);
				// stop the default behavior
				return true;
			}
			return false;
		}
	}

}