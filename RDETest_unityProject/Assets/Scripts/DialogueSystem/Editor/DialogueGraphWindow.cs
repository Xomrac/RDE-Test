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
			GenerateToolbar();
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
			rootVisualElement.Add(_graphView);
		}

		void GenerateToolbar()
		{
			var toolbar = new Toolbar();

			var addDialogueButton = new Button(() =>
			{
				_graphView.AddBeatNode(_currentDialogue);
			})
			{
				text = "Add Dialogue Node"
			};
		
			var addStartButton = new Button(() =>
			{
				_graphView.AddStartNode();
			})
			{
				text = "Add Start Node"
			};
			addStartButton.clicked += () =>
			{
				// Disable the button after adding the start node
				addStartButton.SetEnabled(false);
			};
			
			var saveButton = new Button(() =>
			{
				_graphView.SaveDialogue();
			})
			{
				text = "Save Dialogue"
			};
			toolbar.Add(addDialogueButton);
			toolbar.Add(addStartButton);
			toolbar.Add(saveButton);
			rootVisualElement.Add(toolbar);
		}
	}

}