using UnityEngine;
using UnityEngine.Events;

namespace RDE.HotspotSystem
{

	public class GenericHotspot : AHotspot
	{
		[SerializeField] private UnityEvent _triggered;
		
		public override void Interact()
		{
			_triggered?.Invoke();
			_currentInteractions++;
			if (_currentInteractions >= _maxInteractions && _maxInteractions != -1)
			{
				enabled = false;
			}
			LoseFocus();
		}

		public override bool CanInteract()
		{
			return _interactionType == InteractionType.Manual && HasInteractionsLeft && enabled;
		}

		public override void GainFocus()
		{
			if (CanInteract())
			{
				_affordanceIndicator?.SetActive(true);
			}
		}

		public override void LoseFocus()
		{
			_affordanceIndicator?.SetActive(false);
		}

		public override Transform Transform => transform;
	}

}