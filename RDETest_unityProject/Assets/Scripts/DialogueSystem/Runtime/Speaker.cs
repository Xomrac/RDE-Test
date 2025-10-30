using UnityEngine;

namespace XomracCore.DialogueSystem
{

	[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue System/New Speaker")]
	public class Speaker : ScriptableObject
	{
		[SerializeField] private string _name;
		public string Name => _name;
        
		[SerializeField] private Sprite _portrait;
		public Sprite Portrait => _portrait;
	}

}