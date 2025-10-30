namespace XomracCore.FSM
{

	public class FSMTransition
	{
		private IState _from;
		public IState From => _from;
		private IState _to;
		public IState To => _to;
		private IFSMTransitionCondition _condition { get; }
		public IFSMTransitionCondition Condition => _condition;

		public FSMTransition(IState from, IState to, IFSMTransitionCondition condition)
		{
			_from = from;
			_to = to;
			_condition = condition;
		}
	}

}