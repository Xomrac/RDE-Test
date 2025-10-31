using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XomracCore.DialogueSystem.DialogueSystem
{

	public class DialogueGraphWindow : EditorWindow
	{
		private static readonly Vector2 WINDOW_MIN_SIZE = new(400, 200);
		private static Dialogue _currentDialogue;
		private DialogueGraphView _graphView;
		
		public static void ShowWindow(Dialogue dialogueToEdit)
		{
			_currentDialogue = dialogueToEdit;
			var window = GetWindow<DialogueGraphWindow>();
			window.titleContent = new GUIContent("Editing Dialogue: " + dialogueToEdit.DialogueName);
			window.minSize = WINDOW_MIN_SIZE;
			window.Show();
		}

		private void OnEnable()
		{
			CreateGraphView();
		}

		private void OnDisable()
		{
			if (_graphView == null) return;
			rootVisualElement.Remove(_graphView);
			
		}

		void CreateGraphView()
		{
			rootVisualElement.Clear();
			_graphView = new DialogueGraphView(_currentDialogue);
			_graphView.StretchToParentSize();
			_graphView.SetupContextMenu(this);
			rootVisualElement.Add(_graphView);
		}

		
	}

}