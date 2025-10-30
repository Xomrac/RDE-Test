using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies.FSMConditions
{

	public class HasWaitedEnoughTimeCondition : IFSMTransitionCondition
	{
		private float _waitTime;
		private float _elapsedTime;

		public HasWaitedEnoughTimeCondition(float waitTime)
		{
			_waitTime = waitTime;
			_elapsedTime = 0f;
		}

		public bool IsMet()
		{
			_elapsedTime += Time.deltaTime;
			if (_elapsedTime >= _waitTime)
			{
				_elapsedTime = 0f; // reset for next time
				return true;
			}
			return false;
		}
	}

}