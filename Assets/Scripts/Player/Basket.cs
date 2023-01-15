using UnityEngine;
using UnityEngine.UI;

public class Basket : Singleton<Basket>
{
    public Transform parentObject;
    public Transform spawnObject;
    public Canvas canvas;

    private BoxCollider m_BoxCollider;

    private void Awake() {
        m_BoxCollider = GetComponent<BoxCollider>();
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
            PickUpbasket();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            //Disable the canvas
            canvas.gameObject.SetActive(false);
        }
    }

    private void PickUpbasket() {
        if (LivesSystem.Instance.totalLives <= 0) return;
        //Set the basket position
        transform.SetParent(parentObject);
        transform.position = parentObject.position;
        transform.rotation = parentObject.rotation;
        //Disable the canvas
        canvas.gameObject.SetActive(false);
        //Disable the trigger
        m_BoxCollider.enabled = false;
        //Start game
        GameManager.Instance.StartGame();
    }

    public void ReturnBasketToTable() {
        //Set the basket position
        transform.SetParent(spawnObject);
        transform.position = spawnObject.position;
        transform.rotation = spawnObject.rotation;
        //Enable the trigger
        m_BoxCollider.enabled = true;
    }
}




