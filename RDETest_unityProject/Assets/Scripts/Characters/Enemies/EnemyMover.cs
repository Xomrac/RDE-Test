using RDE.Characters.Base;
using UnityEngine;
using UnityEngine.AI;

namespace RDETest.Characters.Characters.Enemies
{

	public class EnemyMover : A2DCharacterMover
	{
		[SerializeField] private NavMeshAgent _agent;
		
		private Transform _target;

		protected override void Start()
		{
			base.Start();
			_agent.updateRotation = false;
			_agent.updateUpAxis = false;
		}
		
		public void SetTarget(Transform target)
		{
			_target = target;
		}

		public override void Move()
		{
			if (_movementDirection == Vector2.zero || _target == null)
			{
				return;
			}
			base.Move();
			_currentSpeed = _agent.speed;
			_agent.isStopped = false;
			_agent.SetDestination(_target.position);
			CharacterMoved?.Invoke(_movementDirection);
		}
		
		public override void Stop()
		{
			base.Stop();
			_currentSpeed = 0f;
			ChangeDirection(Vector2.zero);
			_agent.isStopped = true;
			CharacterStopped?.Invoke(_previousDirection);
		}
	}

}