using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour { 
    [SerializeField] float deathDelay = 1f;
    [SerializeField] float winDelay = 1f;
    [Header("Sounds")]
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip win;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;
    private new AudioSource audio;
    private bool isTransitioning = false;
    private bool collisionDisabled = false;
    private void Start() {
        audio = GetComponent<AudioSource>();
    }

    private void Update() {
        RespondToDebugKeys();
    }

    private void RespondToDebugKeys() {
        if (Input.GetKey(KeyCode.L) && !isTransitioning) {
            StartCoroutine(LoadNextLevel());
        } else if (Input.GetKey(KeyCode.C)) {
            collisionDisabled = !collisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision) { 
        if (!isTransitioning && !collisionDisabled) {
            switch (collision.gameObject.tag) {
                case "Fuel":
                    Debug.Log("Got fuel..."); 
                    break;
                case "Friendly":
                    break;
                case "Finish":
                    StartCoroutine(LoadNextLevel());
                    break;
                default: 
                    StartCoroutine(ReloadLevel()); 
                    break;
            }
        }
    }

    private IEnumerator ReloadLevel() {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audio.PlayOneShot(death);
        var deathPS = Instantiate(deathParticles, transform.position, Quaternion.identity);
        deathPS.transform.localScale = new Vector3(3, 3, 3);
        Destroy(deathPS.gameObject, deathDelay); 
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isTransitioning = false;
    }

    private IEnumerator LoadNextLevel() { 
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audio.PlayOneShot(win);
        Destroy(Instantiate(successParticles, transform.position, Quaternion.identity), winDelay);
        yield return new WaitForSeconds(winDelay);
        SceneManager.LoadScene(currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings ? currentSceneIndex + 1 : 0);
        isTransitioning = false;
    }
}
