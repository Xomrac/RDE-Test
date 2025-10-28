namespace RDE.Characters.Base
{

	using XomracCore.Patterns.SL;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Abstract base class for character entities.
	/// Provides functionality to register character's services using a local Service Locator pattern.
	/// </summary>
	public abstract class ACharacter : MonoBehaviour
	{

		[SerializeField] private List<Object> _services;

		private ServiceLocator _localServiceLocator;

		public ServiceLocator LocalServiceLocator => _localServiceLocator;

		private void Awake()
		{
			_localServiceLocator = ServiceLocator.Of(this);

			foreach (Object service in _services)
			{
				_localServiceLocator.AddService(service.GetType(), service);
			}
		}
	}

}