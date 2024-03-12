using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
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
		int totalSeconds = (int)_worldSystem.Time;
		int minutes = totalSeconds / 60;
		int seconds = totalSeconds % 60;
		_text.text = $"{minutes}:{seconds.ToString("00")}";

		if (_worldSystem.GameOver)
			_text.color = Color.red;
    }
}
