using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Singleton<Table>
{
    public Canvas canvas;

    private BoxCollider m_BoxCollider;

    private void Awake() {
        m_BoxCollider = GetComponent<BoxCollider>();
        m_BoxCollider.enabled = false;
        //Disable the canvas
        canvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //Enable the canvas
            canvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) {
            Basket.Instance.ReturnBasketToTable();
            GameManager.Instance.LostGame();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //Disable the canvas
            canvas.gameObject.SetActive(false);
        }
    }

    public void EnableTrigger() {
        m_BoxCollider.enabled = true;
    }

    public void DisableTrigger() {
        m_BoxCollider.enabled = false;
        canvas.gameObject.SetActive(false);
    }
}
