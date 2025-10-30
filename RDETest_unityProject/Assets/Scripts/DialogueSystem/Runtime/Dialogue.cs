using UnityEngine;

namespace XomracCore.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private string _dialogueName;
        public string DialogueName => _dialogueName;
    }

}
