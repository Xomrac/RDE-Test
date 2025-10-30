using UnityEngine;

namespace XomracCore.FSM
{

	public class FiniteStateMachine
	{
		private IState _currentState;
		public IState CurrentState => _currentState;
		private readonly FSMTransitionsHandler _transitionsHandler = new();
		public FSMTransitionsHandler Transitions => _transitionsHandler;
		private bool _paused = false;

		public void ChangeState(IState newState)
		{
			if (newState == _currentState)
			{
				return;
			}
			LogStateChange(_currentState, newState);
			_currentState?.Exit();
			_currentState = newState;
			_transitionsHandler.SetCurrentState(_currentState);
			_currentState?.Enter();
		}

		public void Update()
		{
			if (_paused) return;

			var transition = _transitionsHandler.GetTransition();
			if (transition != null)
			{
				ChangeState(transition.To);
			}
			_currentState?.Update();
		}
		
		public void FixedUpdate()
		{
			if (_paused) return;
			_currentState?.FixedUpdate();
		}

		public void Pause()
		{
			_paused = true;
		}

		public void Resume()
		{
			_paused = false;
		}

		public void Stop()
		{
			_currentState?.Exit();
			_currentState = null;
		}

		private void LogStateChange(IState fromState, IState toState)
		{
			string fromStateName = fromState != null ? fromState.GetType().Name : "null";
			string toStateName = toState != null ? toState.GetType().Name : "null";
			Debug.Log($"<color=blue>FSM Transition:</color> <color=red>{fromStateName}</color> -> <color=green>{toStateName}</color>");
		}
	}

}