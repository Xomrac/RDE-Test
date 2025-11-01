using RDETest.Characters.Characters.Enemies;
using UnityEngine;
using XomracCore.FSM;
using XomracCore.Patterns.SL;

namespace RDE.Characters.Enemies.FMSStates
{

	public class IdleState : IState
	{
		private Enemy _enemy;
		private EnemyMover _enemyMover;

		public IdleState(Enemy enemy)
		{
			_enemy = enemy;
			_enemyMover = ServiceLocator.Of(enemy).GetService<EnemyMover>();
		}

		public void Enter()
		{
			_enemyMover?.Stop();
			_enemy.ChangeColor(Color.blue);
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