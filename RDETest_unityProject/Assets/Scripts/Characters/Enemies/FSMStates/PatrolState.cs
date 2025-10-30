using RDETest.Characters.Characters.Enemies;
using UnityEngine;
using XomracCore.FSM;

namespace RDE.Characters.Enemies.FMSStates
{

	public class PatrolState : IState
	{
		private Enemy _enemy;
		private EnemyMover _enemyMover;
		public PatrolState(Enemy enemy)
		{
			_enemy = enemy;
			_enemyMover = enemy.GetComponent<EnemyMover>();
		}

		public void Enter()
		{
			_enemy.ChangeColor(Color.yellow);
		}

		public void Update()
		{
			
		}

		public void FixedUpdate()
		{
			if (_enemyMover == null) return;
			var movementDirection = _enemy.PatrolDestination - _enemy.transform.position;
			_enemyMover.ChangeDirection(movementDirection.normalized);
			_enemyMover.Move();
		}

		public void Exit()
		{
			_enemyMover.Stop();
		}
	}

}