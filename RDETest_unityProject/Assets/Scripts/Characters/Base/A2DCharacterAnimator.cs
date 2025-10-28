using UnityEngine;

namespace RDE.Characters.Base
{

	public abstract class A2DCharacterAnimator : ACharacterAnimator
	{
		public abstract void PlayDirectionalAnimation(Vector2 direction);
		
	}

}