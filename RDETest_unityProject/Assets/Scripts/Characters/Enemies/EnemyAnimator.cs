using RDE.Characters.Base;
using RDE.Characters.PlayerCharacter;
using UnityEngine;
using XomracCore.Patterns.SL;

namespace RDETest.Characters.Characters.Enemies
{

	public class EnemyAnimator : A2DCharacterAnimator
	{

		private static readonly int Moving = Animator.StringToHash("Moving");
		private static readonly int Y = Animator.StringToHash("Y");
		private static readonly int X = Animator.StringToHash("X");
		private EnemyMover _mover;
		protected override void Start()
		{
			base.Start();
			_mover = ServiceLocator.Of(this).GetService<EnemyMover>();
			if (!_mover) return;
			
			_mover.CharacterMoved += OnCharacterMoved;
			_mover.CharacterStopped += OnCharacterStopped;
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
			_animator.speed = Mathf.Clamp(_mover.SpeedProportion, 0.1f, 1f);
			_animator.SetFloat(X, direction.x);
			_animator.SetFloat(Y, direction.y);
		}
	}

}