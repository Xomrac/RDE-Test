using System.Collections.Generic;

namespace XomracCore.FSM
{

	public interface IState
	{
		void Enter();
		void Update();
		void FixedUpdate();
		void Exit();
	}

}