using System;
using UnityEngine;
using UnityEngine.UI;

public class LivesSystem : Singleton<LivesSystem>
{
    public int totalLives;
    public int livesPerRegeneration;
    [Tooltip("Time to regenerate a live (in hours)")]
    public float regenerationTime;

    public Image[] livesImages;

    private DateTime _lastRegenerationTime;

    private void Start() {
        _lastRegenerationTime = DateTime.Now;
        UpdateLivesUI();
    }

    private void Update() {
        if (DateTime.Now - _lastRegenerationTime >= TimeSpan.FromHours(regenerationTime)) {
            RegenerateLives();
        }
    }

    public void LoseLife() {
        if (totalLives > 0) {
            totalLives--;
            UpdateLivesUI();
        }
    }

    private void RegenerateLives() {
        totalLives += livesPerRegeneration;
        _lastRegenerationTime = DateTime.Now;
        UpdateLivesUI();
    }

    private void UpdateLivesUI() {
        for (int i = 0; i < livesImages.Length; i++) {
            if (i < totalLives) {
                livesImages[i].enabled = true;
            }
            else {
                livesImages[i].enabled = false;
            }
        }
    }
}
