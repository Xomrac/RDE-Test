using UnityEngine;
using XomracCore.FSM;

namespace RDE.Characters.Enemies.FMSStates
{

	public class AttackState : IState
	{

		public void Enter()
		{
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