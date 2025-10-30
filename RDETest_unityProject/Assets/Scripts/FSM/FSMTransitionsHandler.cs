using System.Collections.Generic;
using System.Linq;

namespace XomracCore.FSM
{

	public class FSMTransitionsHandler
	{
		private readonly List<FSMTransition> _allTransitions = new();
		private readonly List<FSMTransition> _anyTransitions = new();
		private List<FSMTransition> _currentStateTransitions = new();

		public void SetCurrentState(IState state)
		{
			_currentStateTransitions = _allTransitions.FindAll(transition => transition.From == state);
		}

		public void AddTransition(IState from, IState to, IFSMTransitionCondition condition)
		{
			var transition = new FSMTransition(from, to, condition);
			_allTransitions.Add(transition);
		}

		public void AddAnyTransition(IState to, IFSMTransitionCondition condition)
		{
			var transition = new FSMTransition(null, to, condition);
			_anyTransitions.Add(transition);
		}

		public FSMTransition GetTransition()
		{
			foreach (FSMTransition transition in _anyTransitions.Where(transition => transition.Condition.IsMet()))
			{
				return transition;
			}

			foreach (FSMTransition transition in _currentStateTransitions.Where(transition => transition.Condition.IsMet()))
			{
				return transition;
			}

			return null;
		}
	}

}