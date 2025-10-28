namespace RDE.Characters.Base
{
	using UnityEngine;
	
	public abstract class ACharacterAnimator : MonoBehaviour
	{
		protected Animator _animator;
		public Animator Animator => _animator;

		protected virtual void Awake()
		{
			_animator = GetComponent<Animator>();
			if (_animator == null)
			{
				Debug.LogWarning("ACharacterAnimator requires an Animator component on the same GameObject to function properly.");
				enabled = false;
			}
		}

		protected virtual void Start() { }

		public void PlayAnimation(string stateName)
		{
			_animator.Play(stateName);
		}
	}

}