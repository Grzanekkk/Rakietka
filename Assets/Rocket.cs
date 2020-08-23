using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float speed = 18f;
    public float turnSpeed = 15f;
    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Thrust();

        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * speed);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    void OnCollisionEnter(Collision enterCollision)
    {
        print("Collision");

        if (enterCollision.gameObject.CompareTag("Finish"))
        {
            print("You Win!");
        }
        else if(enterCollision.gameObject.CompareTag("Obstacle"))
        {
            print("You lost your fUeL LmAO niCe");
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
}
