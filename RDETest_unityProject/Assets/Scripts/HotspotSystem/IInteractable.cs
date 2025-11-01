using UnityEngine;

namespace RDE.HotspotSystem
{

	public interface IInteractable
	{
		void Interact();
		
		bool CanInteract();

		void GainFocus();

		void LoseFocus();
		
		Transform Transform { get; }
	}

}