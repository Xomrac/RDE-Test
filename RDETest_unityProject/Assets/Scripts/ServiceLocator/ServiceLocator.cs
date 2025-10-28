namespace XomracCore.Patterns.SL
{
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	using Utils.Extensions;

	public class ServiceLocator : MonoBehaviour
	{
		static ServiceLocator _instance;

		private readonly ServicesHandler servicesHandler = new();
		public IEnumerable<object> AvailableServices => servicesHandler.Available;
		
		

		internal void MakeGlobal(bool dontDestroyOnLoad)
		{
			if (_instance == this)
			{
				Debug.LogWarning("Already global", this);
			}
			else if (_instance != null)
			{
				Debug.LogError("Another ServiceLocator is already global", this);
			}
			else
			{
				Debug.Log("Making ServiceLocator global", this);
				_instance = this;
				if (dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
			}
		}


		/// <summary>
		/// Gets the global ServiceLocator instance. Creates new if none exists.
		/// </summary>        
		public static ServiceLocator Global
		{
			get{
				if (_instance != null) return _instance;

				var globalServiceLocator = FindFirstObjectByType<ServiceLocatorGlobalInitializer>();
				if (globalServiceLocator != null)
				{
					globalServiceLocator.InitializeOnDemand();
					return _instance;
				}
				Debug.LogWarning("No global ServiceLocator found, creating new one. Check if you really want this.");
				var container = new GameObject("===GLOBAL SERVICE LOCATOR===", typeof(ServiceLocator));
				container.AddComponent<ServiceLocatorGlobalInitializer>().InitializeOnDemand();

				return _instance;
			}
		}

		
		/// <summary>
		/// Gets the closest ServiceLocator instance to the provided 
		/// MonoBehaviour in hierarch or the global ServiceLocator.
		/// </summary>
		public static ServiceLocator Of(MonoBehaviour mb)
		{
			return mb.GetComponentInParent<ServiceLocator>().OrNull() ?? Global;
		}

		/// <summary>
		/// Registers a service to the ServiceLocator using the service's type.
		/// </summary>
		/// <param name="service">The service to register.</param>  
		/// <typeparam name="T">Class type of the service to be registered.</typeparam>
		/// <returns>The ServiceLocator instance after registering the service.</returns>
		public ServiceLocator AddService<T>(T service)
		{
			servicesHandler.Add(service);
			return this;
		}
		
		/// <summary>
		/// Registers a service to the ServiceLocator using a specific type.
		/// </summary>
		/// <param name="type">The type to use for registration.</param>
		/// <param name="service">The service to register.</param>  
		/// <returns>The ServiceLocator instance after registering the service.</returns>
		public ServiceLocator AddService(Type type, object service) {
			servicesHandler.Add(type, service);
			return this;
		}
		

		/// <summary>
		/// Tries to get a service of a specific type. Returns whether or not the process is successful.
		/// </summary>
		/// <param name="service">Service of type T to get.</param>  
		/// <typeparam name="T">Class type of the service to be retrieved.</typeparam>
		/// <returns>True if the service retrieval was successful, false otherwise.</returns>
		public bool TryGetService<T>(out T service) where T : class
		{
			service = null;

			return servicesHandler.TryGet(out service);
		}
		
		public T GetService<T>() where T : class
		{
			return servicesHandler.TryGet(out T service) ? service : null;
		}
		
		
		private void OnDestroy()
		{
			_instance = null;
		}

		// https://docs.unity3d.com/ScriptReference/RuntimeInitializeOnLoadMethodAttribute.html
		// maybe overkill but should ensure static vars are reset when entering playmode in the editor
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			_instance = null;
		}
	}
}