using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool _isHoldingEscape;
    private float _holdTime;

    private const float holdDuration = 2F;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            GameManager.Instance.IsPaused = !GameManager.Instance.IsPaused;
            if (GameManager.Instance.IsPaused) {
                GameManager.Instance.PauseGame();
            }
            else {
                GameManager.Instance.UnpauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _isHoldingEscape = true;
            _holdTime = 0;
        }
        if (_isHoldingEscape) {
            if (Input.GetKey(KeyCode.Escape)) {
                _holdTime += Time.deltaTime;
                if (_holdTime >= holdDuration) {
                    ExitGame();
                }
            }
            else {
                _isHoldingEscape = false;
            }
        }
    }

    private void ExitGame() {
        Application.Quit();
        Debug.Log("Exiting...");
    }


}
