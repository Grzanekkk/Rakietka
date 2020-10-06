using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Rocket : MonoBehaviour
{
    public float speed = 18f;
    public float turnSpeed = 15f;
    public float levelLoadDelay = 1f;
    Rigidbody rb;
    AudioSource audioSource;

    public bool collisionsEnabled = true;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip win;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem deathParticles;

    enum State
    {
        Alive,
        Dead,
        Transcending
    }

    State state = State.Alive;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();

            Rotate();
        }

        if (Debug.isDebugBuild)
        {
            DeveloperKeys();
        }
    }


    #region Movement

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * speed * 100 * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }

            mainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Pause();
        }
    }

    private void Rotate()
    {

        if (Input.GetKey(KeyCode.A))
        {
            rb.freezeRotation = true;
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.freezeRotation = true;
            transform.Rotate(-Vector3.forward, turnSpeed * Time.deltaTime);
        }

        rb.freezeRotation = false;
    }


    #endregion Movement


    void DeveloperKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))    // insta przejście do kolejnego levela
        {
            print("Level Skipped");
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.K))    // wyłączenie kolizji
        {         
            ToggleColiisions();
        }
    }

    #region Collision

    void OnCollisionEnter(Collision enterCollision)
    {
        if (state != State.Alive || !collisionsEnabled)
            return;

        string collisionTag = enterCollision.gameObject.tag;

        if (collisionTag == "Finish")
        {
            LevelFinished();
        }
        else if (collisionTag == "Obstacle")
        {
            print("You lost your fUeL LmAO niCe");
        }
        else if (collisionTag == "Deadly")
        {
            Death();
        }
    }

    void ToggleColiisions()
    {
        Collider[] childrenColiders = GetComponentsInChildren<Collider>();

        foreach(Collider collider in childrenColiders)
        {
            collider.enabled = !collider.enabled;
        }

        collisionsEnabled = !collisionsEnabled;      //Prosty przełącznik

        if (collisionsEnabled)
            print("Collisions Enabled");      
        else
            print("Collisions Disabled");
    }


    void Death()
    {
        state = State.Dead;
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        print("You died XDDD");
        Invoke("ReloadLevel", levelLoadDelay);
    }

    #endregion Collision

    #region Level Loading

    void LevelFinished()
    {
        state = State.Transcending;
        audioSource.PlayOneShot(win);
        winParticles.Play();
        print("You Win!");
        Invoke("LoadNextScene", levelLoadDelay);
    }

    int nextScene = 0;

    void LoadNextScene()
    {
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }
        else
        {
            nextScene++;
        }

        SceneManager.LoadScene(nextScene);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion Level Loading

}
