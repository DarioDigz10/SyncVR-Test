using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Transform mainMenuTransform;
    public Transform pointUp;
    public Transform pointDown;
    [Space]
    public Spawner[] spawners;
    [Space]
    public Text gameStatusText;

    private bool _isPaused = false;
    private bool _gameLost = false;
    private bool _gameStarted = false;


    public bool IsPaused { get => _isPaused; set => _isPaused = value; }


    public void Update() {
        if (!_gameStarted) {
            if (!_gameLost) gameStatusText.text = "Pick up the basket to start!\n\nHold ESC to close the game.";
            else gameStatusText.text = "GAME OVER!" + "\n\nYour score: " + ScoreManager.Instance.totalScore + "\n\nHold ESC to close the game.";
        }
        else if (_isPaused) gameStatusText.text = "Game Paused!\n\nPut the basket in the table to finish the run.\n\nHold ESC to close the game.";
        else gameStatusText.text = "FRUIT PICKER\n\nMain Menu";
    }


    public void StartGame() {
        if (LivesSystem.Instance.totalLives <= 0) return;
        if (_gameStarted) return;
        _gameStarted = true;
        _isPaused = false;
        _gameLost = false;
        //take a live
        LivesSystem.Instance.LoseLife();
        // Reset the score to 0
        ScoreManager.Instance.ResetScore();
        // Start the countdown
        CountdownManager.Instance.ResetCountdown();
        CountdownManager.Instance.StartCountdown();
        // Start the spawners
        foreach (Spawner spawner in spawners) {
            spawner.StartSpawning();
        }
        //Hide Main Menu
        MoveMainMenu(pointDown.position, pointUp.position);
    }

    public void PauseGame() {
        if (_gameStarted) {
            // Stop the spawners
            foreach (Spawner spawner in spawners) {
                spawner.StopSpawning();
            }
            //Stop countdown
            CountdownManager.Instance.StopCountdown();
            //Show Main Menu
            MoveMainMenu(pointUp.position, pointDown.position);

            Table.Instance.EnableTrigger();
        }
    }

    public void UnpauseGame() {
        if (_gameStarted) {
            // Start the spawners
            foreach (Spawner spawner in spawners) {
                spawner.StartSpawning();
            }
            //Start countdown
            CountdownManager.Instance.StartCountdown();
            //Hide Main Menu
            MoveMainMenu(pointDown.position, pointUp.position);

            Table.Instance.DisableTrigger();
        }
    }

    public void LostGame() {
        _gameLost = true;
        _gameStarted = false;
        // Stop the spawners
        foreach (Spawner spawner in spawners) {
            spawner.StopSpawning();
        }
        //Reset Basket
        Basket.Instance.ReturnBasketToTable();
        //Show Main Menu
        MoveMainMenu(pointUp.position, pointDown.position);
    }

    public void MoveMainMenu(Vector3 pointA, Vector3 pointB) {
        // Start a coroutine that moves the GameManager object from point A to point B over time
        StartCoroutine(MoveObject(mainMenuTransform, pointA, pointB, 1F));
    }

    private IEnumerator MoveObject(Transform transform, Vector3 startPos, Vector3 endPos, float time) {
        float elapsedTime = 0;
        while (elapsedTime < time) {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

}

