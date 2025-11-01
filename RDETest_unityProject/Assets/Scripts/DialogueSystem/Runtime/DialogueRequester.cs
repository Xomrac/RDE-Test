using UnityEngine;
using UnityEngine.Events;

namespace XomracCore.DialogueSystem
{

	public class DialogueRequester : MonoBehaviour
	{
		[SerializeField] private Dialogue _dialogue;
		public Dialogue Dialogue => _dialogue;

		[SerializeField] private UnityEvent _dialogueFinshed;

		[SerializeField] private bool _requestOnStart = false;

		private void Start()
		{
			if (_requestOnStart)
			{
				StartDialogue();
			}
		}
		

		public void StartDialogue()
		{
			DialoguePlayer player = DialogueManager.StartDialogue(_dialogue);
			if (player != null)
			{
				player.DialogueFinished += OnDialogueFinished;
				return;
			}
			OnDialogueFinished();
		}

		private void OnDialogueFinished()
		{
			_dialogueFinshed?.Invoke();
		}

	}

}