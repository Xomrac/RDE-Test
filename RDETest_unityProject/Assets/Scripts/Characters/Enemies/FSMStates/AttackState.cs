using RDETest.Characters.Characters.Enemies;
using UnityEngine;
using XomracCore.FSM;
using XomracCore.Patterns.SL;

namespace RDE.Characters.Enemies.FMSStates
{

	public class AttackState : IState
	{
		private Enemy _enemy;
		private EnemyMover _enemyMover;
		public AttackState(Enemy enemy)
		{
			_enemy = enemy;
			_enemyMover = ServiceLocator.Of(enemy).GetService<EnemyMover>();
		}

		public void Enter()
		{
			_enemyMover.Stop();
			_enemy.ChangeColor(Color.red);
		}

		public void Update()
		{
			Debug.Log("Attacking!");
		}

		public void FixedUpdate()
		{
			
		}

		public void Exit()
		{
		}
	}
}