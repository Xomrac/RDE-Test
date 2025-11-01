using System;
using UnityEngine;

namespace RDE.HotspotSystem
{

	public abstract class AHotspot : MonoBehaviour,IInteractable
	{
		[SerializeField] protected InteractionType _interactionType;
		[SerializeField] protected GameObject _affordanceIndicator;
		[SerializeField] protected int _maxInteractions = -1; // -1 for infinite
		
		protected int _currentInteractions = 0;
		
		public bool HasInteractionsLeft => _maxInteractions == -1 || _currentInteractions < _maxInteractions;

		private void Awake()
		{
			LoseFocus();
		}

		public abstract void Interact();

		public abstract bool CanInteract();


		public abstract void GainFocus();

		public abstract void LoseFocus();
		
		public abstract Transform Transform { get; }
	}

}