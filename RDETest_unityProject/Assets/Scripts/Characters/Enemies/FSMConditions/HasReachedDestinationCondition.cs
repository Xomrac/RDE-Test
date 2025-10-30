using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies.FSMConditions
{

	public class HasReachedDestinationCondition : IFSMTransitionCondition
	{
		private Enemy _enemy;
		private float _tolerance;

		public HasReachedDestinationCondition(Enemy enemy, float tolerance)
		{
			_enemy = enemy;
			_tolerance = tolerance;
		}

		public bool IsMet()
		{
			// this could be called often, so optimize for performance
			// Use squared distance to avoid the expensive sqrt.
			// range is also squared for comparison.
			float rangeSquared = _tolerance * _tolerance;
			Vector3 distance = _enemy.PatrolDestination.position - _enemy.transform.position;
			return distance.sqrMagnitude <= rangeSquared;
		}
	}

}