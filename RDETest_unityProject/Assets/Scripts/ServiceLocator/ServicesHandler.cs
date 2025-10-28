namespace XomracCore.Patterns.SL
{

	using System;
	using System.Collections.Generic;
	using UnityEngine;

	public class ServicesHandler
	{
		private readonly Dictionary<Type, object> services = new();
		public IEnumerable<object> Available => services.Values;

		public bool TryGet<T>(out T service) where T : class
		{
			Type type = typeof(T);
			if (services.TryGetValue(type, out object obj))
			{
				service = obj as T;
				return true;
			}

			service = null;
			return false;
		}

		public T Get<T>() where T : class
		{
			Type type = typeof(T);
			if (services.TryGetValue(type, out object obj))
			{
				return obj as T;
			}

			throw new ArgumentException($"Service of type {type.FullName} not registered");
		}

		public void Add<T>(T service)
		{
			Type type = typeof(T);

			if (!services.TryAdd(type, service))
			{
				Debug.LogError($"Service of type {type.FullName} already registered");
			}
		}

		public void Add(Type type, object service)
		{
			if (!type.IsInstanceOfType(service))
			{
				throw new ArgumentException("Type of service does not match type of service", nameof(service));
			}

			if (!services.TryAdd(type, service))
			{
				Debug.LogError($"Service of type {type.FullName} already registered");
			}
		}
	}
}