using System;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace RDE.Characters.Base
{

	public abstract class A2DCharacterMover : ACharacterMover
	{
		protected Vector2 _movementDirection = Vector2.zero;
		public Vector2 MovementDirection => _movementDirection;

		protected Vector2 _previousDirection = Vector2.zero;
		public Vector2 PreviousDirection => _previousDirection;

		protected Rigidbody2D _rigidbody;
		public Rigidbody2D Rigidbody => _rigidbody;

		public Action<Vector2> CharacterMoved;
		public Action<Vector2> CharacterStopped;

		protected override void Start()
		{
			base.Start();
			_rigidbody = ServiceLocator.Of(this).GetService<Rigidbody2D>();
			if (!_rigidbody)
			{
				Debug.Log("A2DCharacterMover requires a Rigidbody2D component to function properly.");
				enabled = false;
			}
		}

		public void ChangeDirection(Vector2 newDirection)
		{
			_movementDirection = newDirection.normalized;
			if (newDirection != Vector2.zero)
			{
				_previousDirection = _movementDirection;
			}
		}
	}
}