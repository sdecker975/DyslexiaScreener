using UnityEngine;

/// <summary>
/// Controller for changing timeScale on the fly, useful for debugging purposes.
/// </summary>
public class TimeScaleController : MonoBehaviour {

    [Header("Debug/Dynamic")]
    [Range(0f, 20f)]
    [SerializeField] private float timeScale = 1.0f;

    private void Start() {
        SetTimeScale(timeScale);
    }

    private void OnValidate() {
        SetTimeScale(timeScale);
    }

    private void OnDisable() {
        Time.timeScale = 1f;
    }

    private void OnDestroy() {
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Sets the timeScale.
    /// </summary>
    /// <param name="newTimeScale">The new value of timeScale.</param>
    public void SetTimeScale(float newTimeScale) {
        timeScale = newTimeScale;
        Time.timeScale = newTimeScale;
    }

    /// <summary>
    /// Increases current time scale.
    /// </summary>
    /// <param name="increment">The increment value.</param>
    public void IncreaseTimeScale(float increment) {
        timeScale += increment;
        SetTimeScale(timeScale);
    }

    /// <summary>
    /// Decreases current time scale.
    /// </summary>
    /// <param name="decrement">The decrement value.</param>
    public void DecreaseTimeScale(float decrement) {
        timeScale -= decrement;
        SetTimeScale(timeScale);
    }

}