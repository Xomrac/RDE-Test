namespace RDE.Characters.PlayerCharacter
{
	using Base;
	using UnityEngine;
	using UnityEngine.InputSystem;

	/// <summary>
	/// Handles player movement, including input processing, movement, and rotation.
	/// </summary>
	public class PlayerMover : A2DCharacterMover
	{
		[SerializeField] private InputActionReference _moveAction;


		protected override void Awake()
		{
			base.Awake();
			BindControls();
			ToggleControls(true);
		}

		private void FixedUpdate()
		{
			Move();
			UpdateMovementSpeed(Time.fixedDeltaTime);
		}
		
		private void BindControls()
		{
			_moveAction.action.performed += OnMovementPerformed;
			_moveAction.action.canceled += OnMovementCanceled;
		}
		
		private void OnMovementPerformed(InputAction.CallbackContext context)
		{
			var inputDirection = context.ReadValue<Vector2>();
			ChangeDirection(inputDirection);
		}
		
		private void OnMovementCanceled(InputAction.CallbackContext _)
		{
			Stop();
		}
		
		protected override void Move()
		{
			if (_movementDirection == Vector2.zero)
			{
				return;
			}
			base.Move();
			Vector2 movementDelta = new Vector2(_movementDirection.x, _movementDirection.y) * (_currentSpeed * Time.fixedDeltaTime);
			Debug.Log(_currentSpeed);
			Debug.Log(_currentSpeed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(_rigidbody.position + movementDelta);
			CharacterMoved?.Invoke(_movementDirection);
		}

		protected override void Stop()
		{
			base.Stop();
			ChangeDirection(Vector2.zero);
			_rigidbody.linearVelocity = Vector3.zero;
			CharacterStopped?.Invoke(_previousDirection);
		}
		
		private void ToggleControls(bool enable)
		{
			if (enable)
			{
				_moveAction.action.Enable();
			}
			else
			{
				_moveAction.action.Disable();
			}
		}
		
		
	}

}