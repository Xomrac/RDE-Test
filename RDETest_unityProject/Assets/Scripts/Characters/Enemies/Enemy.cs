using System;
using System.Collections.Generic;
using RDE.Characters.Base;
using RDE.Characters.Enemies.FMSStates;
using RDE.Characters.PlayerCharacter;
using RDETest.Characters.Characters.Enemies.FSMConditions;
using UnityEngine;
using XomracCore.FSM;

namespace RDETest.Characters.Characters.Enemies
{

	public class Enemy : ACharacter
	{
		[SerializeField] private Transform[] _waypoints;
		[SerializeField] private float _idleTime = 5f;
		[SerializeField] private float _distanceTolerance = 0.4f;
		[SerializeField] private float _detectionRange = 10f;
		[SerializeField] private float _attackRange = 2f;
		[SerializeField] private LayerMask _layersToCheck;
		[SerializeField] private SpriteRenderer _renderer;

		

		public void ChangeColor(Color newColor)
		{
			if (_renderer != null)
			{
				_renderer.color = newColor;
			}
		}
		
		private FiniteStateMachine _fsm;
		private Player _player;
		private Vector3 _patrolDestination;
		public Vector3 PatrolDestination => _patrolDestination;
		private Transform[] _lastWaypoints = new Transform[2];
		private List<Transform> _availableWaypoints = new();
		
		

		private void Start()
		{
			_availableWaypoints.AddRange(_waypoints);
			SetNewPatrolDestination();
			_player = FindFirstObjectByType<Player>();
			if (_player == null)
			{
				Debug.LogError("Enemy: Player not found in the scene. Disabling Enemy script.");
				enabled = false;
				return;
			}
			SetupFSM();
		}

		public void SetNewPatrolDestination()
		{
			if (_waypoints.Length == 0) return;
			var randomIndex = UnityEngine.Random.Range(0, _availableWaypoints.Count);
			_patrolDestination = _availableWaypoints[randomIndex].position;
			var temp = _lastWaypoints[1];
			_lastWaypoints[1] = _lastWaypoints[0];
			_lastWaypoints[0] = _availableWaypoints[randomIndex];
			_availableWaypoints.RemoveAt(randomIndex);
			if (temp != null)
			{
				_availableWaypoints.Add(temp);
			}
		}

		private void SetupFSM()
		{
			var idleState = new IdleState(this);
			var patrolState = new PatrolState(this);
			var chaseState = new ChaseState(this, _player);
			var attackState = new AttackState();
			var playerIsVisibleCondition = new PlayerIsVisibleCondition(_detectionRange, _player, transform, _layersToCheck);
			var isNearPlayerCondition = new IsNearPlayerCondition(_attackRange, _player.transform, transform);

			_fsm = new FiniteStateMachine();

			_fsm.Transitions.AddAnyTransition(attackState, isNearPlayerCondition);
			_fsm.Transitions.AddAnyTransition(chaseState, playerIsVisibleCondition);
			_fsm.Transitions.AddTransition(idleState, patrolState, new HasWaitedEnoughTimeCondition(_idleTime));

			_fsm.Transitions.AddTransition(patrolState, idleState, new HasReachedDestinationCondition(this, _distanceTolerance));
			_fsm.Transitions.AddTransition(patrolState, chaseState, playerIsVisibleCondition);

			_fsm.Transitions.AddTransition(chaseState, patrolState, new PlayerIsNotVisibleCondition(_detectionRange, _player, transform, _layersToCheck));
			_fsm.Transitions.AddTransition(chaseState, attackState, isNearPlayerCondition);

			_fsm.Transitions.AddTransition(attackState, chaseState, new IsFarFromPlayerCondition(_attackRange, _player.transform, transform));
			
			_fsm.ChangeState(patrolState);
		}

		private void Update()
		{
			_fsm.Update();
		}
		private void FixedUpdate()
		{
			_fsm.FixedUpdate();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, _detectionRange);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, _attackRange);
			Gizmos.color = Color.cyan;
			Gizmos.DrawSphere(_patrolDestination, 0.1f);
		}
#endif

	}

}