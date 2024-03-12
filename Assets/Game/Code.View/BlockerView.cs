using UnityEngine;

[RequireComponent(typeof(BlockerComponent))]
public class BlockerView : BlockerViewBase
{
	protected BlockerComponent _blocker;

	protected override void Awake()
	{
		base.Awake();
		_blocker = GetComponent<BlockerComponent>();
	}

	private void LateUpdate()
	{
		(TileId.x, TileId.y) = _worldSystem.WorldToCell(transform.position.x, transform.position.y);

		var min = Mathf.Max(_blocker.SpawnTime, _blocker.DeathTime - 2.0f);
		var max = _blocker.DeathTime;
		var current = _worldSystem.Time + Time.time - _lastFixedUpdateTime;
		float transformationRatio = Mathf.InverseLerp(min, max, current);

		// desmos: 0.5-0.5\sin\left(x\cdot3\pi-.5\pi\right)
		float alpha = .5f - .5f * Mathf.Sin(transformationRatio * 3f * Mathf.PI - .5f * Mathf.PI);
		//Debug.Log($"T{Time.time.ToString("0.000")} -- F{Time.fixedTime.ToString("0.000")} -- C{current.ToString("0.000")} -- A{alpha.ToString("0.000")}");

		float red = transformationRatio;
		Color c = new(red, 0.3f, 0.1f, alpha);
		for (int i = 0; i < _renderers.Length; i++)
		{
			var renderer = _renderers[i];
			var startColor = _rendererStartColors[i];
			renderer.color = Color.Lerp(startColor, c, transformationRatio);
		}
	}
}