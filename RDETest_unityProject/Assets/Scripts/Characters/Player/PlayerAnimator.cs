using XomracCore.Patterns.SL;

namespace RDE.Characters.PlayerCharacter
{

	using UnityEngine;
	using Base;

	public class PlayerAnimator : A2DCharacterAnimator
	{
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
		}

		private void OnCharacterMoved(Vector2 movementDirection)
		{
			PlayDirectionalAnimation(movementDirection);
		}
		

		public override void PlayDirectionalAnimation(Vector2 direction)
		{
			_animator.SetFloat("x", direction.x);
			_animator.SetFloat("y", direction.y);
		}

		private void PlayIdleAnimation(Vector2 direction)
		{
			
		}
	}

}