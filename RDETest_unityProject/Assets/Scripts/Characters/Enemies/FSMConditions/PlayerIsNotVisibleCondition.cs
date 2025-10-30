using System;
using RDE.Characters.PlayerCharacter;
using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies
{

	internal class PlayerIsNotVisibleCondition : IFSMTransitionCondition
	{
		private float _range;
		private Player _player;
		private Transform _transform;
		private LayerMask _raycastMask;
		
		public PlayerIsNotVisibleCondition(float range, Player player, Transform transform, LayerMask raycastMask)
		{
			_range = range;
			_player = player;
			_transform = transform;
			_raycastMask = raycastMask;
		}

		public bool IsMet()
		{
			// this could be called often, so optimize for performance
			// Use squared distance to avoid the expensive sqrt.
			// range is also squared for comparison.
			float rangeSquared = _range * _range;
			Vector3 distance = _player.transform.position - _transform.position;
			bool isNear = distance.sqrMagnitude <= rangeSquared;
			
			// if not near, no need to raycast at all, it would be waste of performance
			if (!isNear) return true;
			
			RaycastHit2D hit = Physics2D.Raycast(_transform.position, distance.normalized, _raycastMask);
			return hit.collider == null || hit.collider.gameObject != _player.gameObject;
		}
	}

}