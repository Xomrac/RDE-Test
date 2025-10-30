using RDETest.Characters.Characters.Enemies;
using UnityEngine;
using XomracCore.FSM;

namespace RDE.Characters.Enemies.FMSStates
{

	public class IdleState : IState
	{
		private Enemy _enemy;
		private EnemyMover _enemyMover;

		public IdleState(Enemy enemy)
		{
			_enemy = enemy;
			_enemyMover = enemy.GetComponent<EnemyMover>();
		}

		public void Enter()
		{
			_enemyMover?.Stop();
		}

		public void Update() { }

		public void FixedUpdate()
		{
		}

		public void Exit()
		{
			_enemy.SetNewPatrolDestination();
		}
	}

}