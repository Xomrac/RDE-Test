using RDE.Characters.PlayerCharacter;
using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies.FSMConditions
{

	public class PlayerIsVisibleCondition : IFSMTransitionCondition
	{
		private float _range;
		private Player _player;
		private Transform _transform;
		private LayerMask _raycastMask;
		private bool _debug;

		public PlayerIsVisibleCondition(float range, Player player, Transform transform, LayerMask raycastMask, bool debug = false)
		{
			_range = range;
			_player = player;
			_transform = transform;
			_raycastMask = raycastMask;
			_debug = debug;
		}

		public bool IsMet()
		{
			// ottimizzazione: confronto con distanza al quadrato
			float rangeSquared = _range * _range;
			Vector3 toPlayer = _player.transform.position - _transform.position;
			bool isNear = toPlayer.sqrMagnitude <= rangeSquared;

			if (!isNear)
			{
				if (_debug)
				{
					Debug.DrawLine(_transform.position, _player.transform.position, Color.yellow, 0f, true);
					Debug.Log($"[PlayerIsVisible] Fuori portata. dist^2={toPlayer.sqrMagnitude} range^2={rangeSquared}");
				}
				return false;
			}

			Vector2 origin = _transform.position;
			Vector2 dir = toPlayer.normalized;
			// corregge gli argomenti: passare range e layerMask.value
			RaycastHit2D hit = Physics2D.Raycast(origin, dir, _range, _raycastMask.value);

			bool visible = hit.collider != null && hit.collider.gameObject == _player.gameObject;
			Color lineColor = visible ? Color.green : (hit.collider != null ? Color.red : Color.white);

			// linea visiva dalla posizione dell'enemy nella direzione del raycast
			Debug.DrawLine(_transform.position, _transform.position + (Vector3)(dir * _range), lineColor, 0f, true);

			if (_debug)
			{
				Debug.Log($"[PlayerIsVisible] hit={(hit.collider!=null?hit.collider.name:"none")} visible={visible} origin={origin} dir={dir} range={_range}");
			}

			return visible;
		}
	}

}