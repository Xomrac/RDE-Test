namespace RDE.HotspotSystem
{
	using System.Collections;
	using UnityEngine;
	using UnityEngine.InputSystem;
	
	public class Interactor : MonoBehaviour
	{

		[SerializeField] private float _checkRate = 0.3f;
		[SerializeField] private float _checkRadius = 0.5f;
		[SerializeField] private LayerMask _interactableLayer;
		[SerializeField] private InputActionReference _interactInputAction;

		private IInteractable _currentInteractable;
		private Coroutine _searchCoroutine;

		private void Awake()
		{
			_interactInputAction.action.performed += OnInteract;
			StartSearchCoroutine();
		}

		private void OnEnable()
		{
			StartSearchCoroutine();
			_interactInputAction.action.Enable();
			_currentInteractable?.GainFocus();
		}
		
		private void OnDisable()
		{
			StopSearchCoroutine();
			_interactInputAction.action.Disable();
			_currentInteractable?.LoseFocus();
		}

		private void OnInteract(InputAction.CallbackContext _)
		{
			_currentInteractable?.Interact();
		}
		
		private void StartSearchCoroutine()
		{
			if (_searchCoroutine != null) StopCoroutine(_searchCoroutine);
			_searchCoroutine = StartCoroutine(SearchInteractables());
		}

		private void StopSearchCoroutine()
		{
			if (_searchCoroutine != null)
			{
				StopCoroutine(_searchCoroutine);
				_searchCoroutine = null;
			}
		}

		// Continuously searches for interactable objects within a specified radius at defined intervals.
		// It's costly and useless to do it every frame.
		private IEnumerator SearchInteractables()
		{
			while (true)
			{
				CheckForInteractables();
				yield return new WaitForSeconds(_checkRate);
			}
		}

		private void CheckForInteractables()
		{
			Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _checkRadius, _interactableLayer);
			IInteractable nearest = null;
			float nearestDistance = float.MaxValue;

			foreach (Collider2D hit in hits)
			{
				if (hit.TryGetComponent(out IInteractable interactable))
				{
					if (!interactable.CanInteract()) continue;
					float distance = (transform.position - interactable.Transform.position).sqrMagnitude;
					if (distance < nearestDistance)
					{
						nearest = interactable;
						nearestDistance = distance;
					}
				}
			}

			if (nearest != _currentInteractable)
			{
				_currentInteractable?.LoseFocus();
				_currentInteractable = nearest;
				_currentInteractable?.GainFocus();
			}
			else if (nearest == null)
			{
				_currentInteractable?.LoseFocus();
				_currentInteractable = null;
			}
		}
	}
}
