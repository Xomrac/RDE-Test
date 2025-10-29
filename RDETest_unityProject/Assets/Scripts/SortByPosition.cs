using UnityEngine.Rendering;

namespace RDE
{
	using UnityEngine;

	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(SortingGroup))]
	public class SortByPosition : MonoBehaviour
	{
		[Tooltip("Offset to add to the calculated order")]
		[SerializeField] private int _orderOffset = 0;
		[Tooltip("Bigger = more precise, but more sensitive")]
		[SerializeField] private int _precision = 100;
		[Tooltip("Point from which to calculate sorting")]
		[SerializeField] private Transform _sortPoint;

		private SortingGroup _group;

		private void Awake()
		{
			_group = GetComponent<SortingGroup>();
			
			if (_sortPoint == null) _sortPoint = transform;
		}

		private void Update()
		{
			float y = -_sortPoint.position.y;
			int order = Mathf.RoundToInt(y * _precision) + _orderOffset;
			_group.sortingOrder = order;
		}
	}


}