using UnityEngine;
using UnityEngine.Assertions;

public class BlockerViewBase : MonoBehaviour
{
	public Vector2Int TileId;

	protected WorldSystem _worldSystem;

	protected SpriteRenderer[] _renderers;
	protected Color[] _rendererStartColors;

	protected float _lastFixedUpdateTime;

	protected virtual void Awake()
	{
		_worldSystem = FindFirstObjectByType<WorldSystem>();

		_renderers = GetComponentsInChildren<SpriteRenderer>();
		_rendererStartColors = new Color[_renderers.Length];
		for (int i = 0; i < _renderers.Length; i++)
			_rendererStartColors[i] = _renderers[i].color;
	}

	protected virtual void OnEnable()
	{
		Assert.IsTrue(Time.inFixedTimeStep);
		_lastFixedUpdateTime = Time.time;
	}

	protected virtual void FixedUpdate()
	{
		_lastFixedUpdateTime = Time.time;
	}
}