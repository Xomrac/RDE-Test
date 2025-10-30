using RDE.Characters.PlayerCharacter;
using RDETest.Characters.Characters.Enemies;
using UnityEngine;
using XomracCore.FSM;

namespace RDE.Characters.Enemies.FMSStates
{

	public class ChaseState : IState
	{
		private Enemy _enemy;
		private Player _player;
		private EnemyMover _enemyMover;
		public ChaseState(Enemy enemy, Player player)
		{
			_enemy = enemy;
			_player = player;
			_enemyMover = enemy.GetComponent<EnemyMover>();
		}

		public void Enter()
		{
			_enemy.ChangeColor(Color.red);
			_enemyMover.SetTarget(_player.transform);
		}

		public void Update()
		{
			
		}

		public void FixedUpdate()
		{
			if (_enemyMover == null) return;
			Debug.Log("Fixed Update - Chasing Player");
			var movementDirection = _player.transform.position - _enemy.transform.position;
			_enemyMover.ChangeDirection(movementDirection.normalized);
			_enemyMover.Move();
		}

		public void Exit()
		{
		}
	}

}