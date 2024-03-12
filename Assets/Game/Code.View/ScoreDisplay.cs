using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
	Text _text;
	WorldSystem _worldSystem;

	private void Awake()
	{
		_text = GetComponent<Text>();
		_worldSystem = FindFirstObjectByType<WorldSystem>();
	}

	// Update is called once per frame
	void LateUpdate()
	{
		_text.text = _worldSystem.Score.ToString();

		if (_worldSystem.GameOver)
			_text.color = Color.red;
	}
}
