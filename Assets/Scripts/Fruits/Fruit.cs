using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, IPooledObject
{
    public string poolTag;
    public int pointsToAdd = 10;
    public int secondsToAdd = 3;
    public float impulseForce = 5F;

    [Header("Audio Clips")]
    public AudioClip[] fruitSpawnSounds;
    public AudioClip onBounceSound;
    public AudioClip onPickUpSound;

    private float _bounceDuration = 1F;

    private bool _isColliding = false;
    private bool _bouncing = false;

    private AudioSource m_AudioSource;
    private Rigidbody m_RigidBody;

    private void Awake() {
        m_RigidBody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        m_RigidBody.useGravity = false;
    }

    public void OnObjectSpawned() {
        //Physics
        m_RigidBody.useGravity = true;
        // Apply an impulse force in the spawn direction
        m_RigidBody.AddForce(transform.forward * Random.Range(impulseForce - .5F, impulseForce + .5F), ForceMode.Impulse);
        //Sound
        int randomSoundIndex = Random.Range(0, fruitSpawnSounds.Length);
        m_AudioSource.PlayOneShot(fruitSpawnSounds[randomSoundIndex]);
    }


    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Basket")) {
            _isColliding = true;
        }
        if (!_bouncing) {
            _bouncing = true;
            StartCoroutine(Bounce());
        }
        //Play Sound
        if (!m_AudioSource.isPlaying) m_AudioSource.PlayOneShot(onBounceSound, .5F);
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Basket")) {
            _isColliding = false;
        }
    }

    private IEnumerator Bounce() {
        float startTime = Time.time;
        while (_bouncing) {
            if (Time.time - startTime >= _bounceDuration && _isColliding) {
                //Add points
                ScoreManager.Instance.AddPoints(pointsToAdd);
                //Add seconds
                CountdownManager.Instance.AddSeconds(secondsToAdd);
                //Play Sound:
                AudioSource.PlayClipAtPoint(onPickUpSound, ScoreManager.Instance.transform.position);
                _isColliding = false;
            }
            if (Time.time - startTime >= _bounceDuration && !_isColliding) {
                _bouncing = false;
            }
            yield return null;
        }
        ReturnToPool();
    }


    private void ReturnToPool() {
        //Reset the rigidbody
        m_RigidBody.useGravity = false;
        m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.angularVelocity = Vector3.zero;
        //Return to the pool
        ObjectPooler.Instance.ReturnToPool(poolTag, gameObject);
    }
}
