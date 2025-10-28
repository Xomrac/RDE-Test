namespace XomracCore.Patterns.SL
{
	using UnityEngine;
	public class ServiceLocatorGlobalInitializer : AServiceLocatorInitializer
	{
		[SerializeField] private bool _dontDestroyOnLoad = true;
		protected override void Intialize()
		{
			Target.MakeGlobal(_dontDestroyOnLoad);
		}
	}

}