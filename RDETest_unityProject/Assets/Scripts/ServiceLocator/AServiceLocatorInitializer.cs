namespace XomracCore.Patterns.SL
{
	using UnityEngine;
	using Utils.Extensions;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(ServiceLocator))]
	public abstract class AServiceLocatorInitializer : MonoBehaviour
	{
		private ServiceLocator _target;
		internal ServiceLocator Target => _target.OrNull() ?? (_target = GetComponent<ServiceLocator>());

		private bool _hasBeenBootIntialized;

		void Awake() => InitializeOnDemand();

		public void InitializeOnDemand()
		{
			if (_hasBeenBootIntialized) return;
			_hasBeenBootIntialized = true;
			Intialize();
		}

		protected abstract void Intialize();
	}

}