using XomracCore.Patterns.SL;

namespace RDE.Characters.PlayerCharacter
{

	using UnityEngine;
	using Base;

	public class PlayerAnimator : A2DCharacterAnimator
	{
		private static readonly int Moving = Animator.StringToHash("Moving");
		private static readonly int Y = Animator.StringToHash("Y");
		private static readonly int X = Animator.StringToHash("X");
		private PlayerMover _playerMover;
		protected override void Start()
		{
			base.Start();
			_playerMover = ServiceLocator.Of(this).GetService<PlayerMover>();
			if (!_playerMover) return;
			
			_playerMover.CharacterMoved += OnCharacterMoved;
			_playerMover.CharacterStopped += OnCharacterStopped;
		}

		private void OnCharacterStopped(Vector2 previousDirection)
		{
            _animator.SetBool(Moving, false);
			PlayDirectionalAnimation(previousDirection);
		}

		private void OnCharacterMoved(Vector2 movementDirection)
		{
			_animator.SetBool(Moving, true);
			PlayDirectionalAnimation(movementDirection);
		}
		

		public override void PlayDirectionalAnimation(Vector2 direction)
		{
			_animator.SetFloat(X, direction.x);
			_animator.SetFloat(Y, direction.y);
		}
	}

}