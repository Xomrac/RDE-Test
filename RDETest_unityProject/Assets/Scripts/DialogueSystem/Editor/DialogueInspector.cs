using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	[CustomEditor(typeof(Dialogue))]
	public class DialogueInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Edit Dialogue"))
			{
				DialogueGraphWindow.ShowWindow((Dialogue)target);
			}
		}
	}

}