using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : Singleton<ScoreManager>
{
    // The total score
    public int totalScore;
    // Reference to the UI text element that displays the score
    public Text scoreText;

    void Start() {
        UpdateScoreText();
    }

    /// <summary>
    /// Add a certain amount of points to the total score
    /// </summary>
    /// <param name="points">The amount of points to add</param>
    public void AddPoints(int points) {
        totalScore += points;
        UpdateScoreText();
    }

    /// <summary>
    /// Reset the score to zero
    /// </summary>
    public void ResetScore() {
        totalScore = 0;
        UpdateScoreText();
    }

    /// <summary>
    /// Update the score text on the UI with the current total score
    /// </summary>
    private void UpdateScoreText() {
        StartCoroutine(AnimateScoreText());
    }

    /// <summary>
    /// Animate the score text from its current value to the new total score value over a period of time
    /// </summary>
    IEnumerator AnimateScoreText() {
        // Get the current score value from the score text
        int startScore = int.Parse(scoreText.text.Split(':')[1]);
        int currentScore = startScore;
        // The time it takes for the animation to complete
        float animationTime = 1f;
        float animationTimer = 0f;
        while (animationTimer < animationTime) {
            animationTimer += Time.deltaTime;
            // Interpolate the current score value between the start score and total score
            currentScore = (int)Mathf.Lerp(startScore, totalScore, animationTimer / animationTime);
            scoreText.text = "Score: " + currentScore;
            yield return null;
        }
        scoreText.text = "Score: " + totalScore;
    }
}

