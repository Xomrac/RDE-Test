namespace RDE.Characters.Base
{

	using UnityEngine;

	public abstract class ACharacterMover : MonoBehaviour
	{
		[SerializeField] protected float _maxMovementSpeed = 10f;
		public float MaxMovementSpeed => _maxMovementSpeed;

		[SerializeField] protected float _instantAcceleration = 5f;
		public float InstantAcceleration => _instantAcceleration;

		[SerializeField] protected AnimationCurve _accelerationCurve = AnimationCurve.Linear(0, 0, 1, 1);
		public AnimationCurve AccelerationCurve => _accelerationCurve;

		public float SpeedProportion => _currentSpeed / _maxMovementSpeed;

		protected float _currentSpeed;
		public float CurrentSpeed => _currentSpeed;
		private float _movementTime;
		private bool _isMoving;
		public bool IsMoving => _isMoving;

		protected virtual void Awake() { }

		protected virtual void Start()
		{
			_currentSpeed = 0;
		}

		protected void UpdateMovementSpeed(float deltaTime)
		{
			if (_isMoving)
			{
				_movementTime += deltaTime;
				float accelerationFactor = _accelerationCurve.Evaluate(_movementTime);
				_currentSpeed = Mathf.Lerp(_instantAcceleration, _maxMovementSpeed, accelerationFactor);
			}
			else
			{
				_movementTime = 0;
				_currentSpeed = 0;
			}
		}

		public virtual void Move()
		{
			_isMoving = true;
		}

		public virtual void Stop()
		{
			_isMoving = false;
			_currentSpeed = 0;
		}
	}

}