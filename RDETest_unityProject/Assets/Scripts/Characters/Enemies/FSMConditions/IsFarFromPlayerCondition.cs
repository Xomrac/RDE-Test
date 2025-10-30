using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies.FSMConditions
{

	public class IsFarFromPlayerCondition : IFSMTransitionCondition
	{
		private float _range;
		private	Transform _player;
		private Transform _transform;
		
		public IsFarFromPlayerCondition(float range, Transform player, Transform transform)
		{
			_range = range;
			_player = player;
			_transform = transform;
		}

		public bool IsMet()
		{
			// this could be called often, so optimize for performance
			// Use squared distance to avoid the expensive sqrt.
			// range is also squared for comparison.
			float rangeSquared = _range * _range;
			Vector3 distance = _player.position - _transform.position;
			return distance.sqrMagnitude > rangeSquared;
		}
	}

}