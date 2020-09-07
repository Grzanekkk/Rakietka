using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    public float speed = 18f;
    public float turnSpeed = 15f;
    Rigidbody rb;
    AudioSource audioSource;

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
        if (state != State.Dead)
        {
            Thrust();

            Rotate();
        }

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * speed);
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

    void OnCollisionEnter(Collision enterCollision)
    {
        if (state != State.Alive)
            return;
        
        string collisionTag = enterCollision.gameObject.tag;

        if (collisionTag == "Finish")
        {
            LevelFinished();
        }
        else if(collisionTag == "Obstacle")
        {
            print("You lost your fUeL LmAO niCe");
        }
        else if(collisionTag == "Deadly")
        {
            Death();
        }
    }

    void Death()
    {
        state = State.Dead;
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        print("You died XDDD");
        Invoke("ReloadLevel", 1f);
    }

    void LevelFinished()
    {
        state = State.Transcending;
        audioSource.PlayOneShot(win);
        winParticles.Play();
        print("You Win!");
        Invoke("LoadNextScene", 1f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
