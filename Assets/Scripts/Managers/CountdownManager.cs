using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountdownManager : Singleton<CountdownManager>
{
    public Text countdownText;
    public Text addSecondsText;
    public float totalTime = 60f;

    private float _remainingTime;
    private bool _isRunning = false;

    private Color _originalTextColor;

    private void Start() {
        _remainingTime = totalTime;
        countdownText.text = _remainingTime.ToString("F2");
        _originalTextColor = addSecondsText.color;
    }

    private void Update() {
        if (_isRunning) {
            _remainingTime -= Time.deltaTime;
            countdownText.text = _remainingTime.ToString("F2");

            if (_remainingTime <= 0) {
                StopCountdown();
                //Stop the Game
                GameManager.Instance.LostGame();
            }
        }
    }

    public void ResetCountdown() {
        _remainingTime = totalTime;
    }

    public void StartCountdown() {
        _isRunning = true;
    }

    public void StopCountdown() {
        _isRunning = false;
    }

    public void AddSeconds(float seconds) {
        _remainingTime += seconds;
        addSecondsText.text = "+" + seconds.ToString("F0");
        StartCoroutine(FadeOutText(addSecondsText));
    }

    public void SubstractSeconds(float seconds) {
        _remainingTime -= seconds;
        addSecondsText.text = "-" + seconds.ToString("F0");
        StartCoroutine(FadeOutText(addSecondsText));
    }

    IEnumerator FadeOutText(Text text) {
        text.color = _originalTextColor;
        float startTime = Time.time;
        while (Time.time - startTime <= 1.5F) {
            float t = (Time.time - startTime) / 1.5F;
            text.color = Color.Lerp(_originalTextColor, Color.clear, t);
            yield return new WaitForEndOfFrame();
        }
    }

}