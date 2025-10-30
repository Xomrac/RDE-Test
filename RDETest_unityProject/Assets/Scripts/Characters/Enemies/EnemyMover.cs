using RDE.Characters.Base;
using UnityEngine;

namespace RDETest.Characters.Characters.Enemies
{

	public class EnemyMover : A2DCharacterMover
	{

		[SerializeField] private Transform _target;

		
		public override void Move()
		{
			if (_movementDirection == Vector2.zero)
			{
				return;
			}
			base.Move();
			UpdateMovementSpeed(Time.fixedDeltaTime);
			Vector2 movementDelta = new Vector2(_movementDirection.x, _movementDirection.y) * (_currentSpeed * Time.fixedDeltaTime);
			_rigidbody.MovePosition(_rigidbody.position + movementDelta);
			CharacterMoved?.Invoke(_movementDirection);
		}
		
		public override void Stop()
		{
			base.Stop();
			ChangeDirection(Vector2.zero);
			_rigidbody.linearVelocity = Vector3.zero;
			CharacterStopped?.Invoke(_previousDirection);
		}
	}

}