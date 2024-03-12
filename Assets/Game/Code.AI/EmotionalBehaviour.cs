using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for managing the emotional state of the AI
public class EmotionalBehaviour : MonoBehaviour
{
    public EmotionalState CurrentState { get; private set; } = EmotionalState.Neutral;
    private float excitementLevel = 0;
    private float fearLevel = 0;

    public void UpdateExcitement(float value)
    {
        excitementLevel += value;
        UpdateEmotionalState();
    }

    public void UpdateFear(float value)
    {
        fearLevel += value;
        UpdateEmotionalState();
    }
    public void ResetEmotion()
    {
        CurrentState = EmotionalState.Neutral;
        excitementLevel = 0;
        fearLevel = 0;
        UpdateEmotionalState();

    }
    private void UpdateEmotionalState()
    {
        if (fearLevel > excitementLevel)
        {
            CurrentState = EmotionalState.Scared;
        }
        else if (excitementLevel > 0)
        {
            CurrentState = EmotionalState.Excited;
        }
        else if (fearLevel > 0 && excitementLevel > 0)
        {
            CurrentState = EmotionalState.Cautious;
        }
        else
        {
            CurrentState = EmotionalState.Neutral;
        }

        // Reset levels after evaluation
        excitementLevel = 0;
        fearLevel = 0;
    }

}
public enum EmotionalState
{
    Neutral,
    Excited,
    Scared,
    Cautious
}
