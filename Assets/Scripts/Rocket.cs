using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;
    
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] AudioClip finishSound;
    
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem boomParticles;
    [SerializeField] ParticleSystem finishParticles;

    bool collisionOff = false;
    int activeLevelIndex;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum States {Playing, Dead, NextLevel};
    States state = States.Playing;

    void Start()
    {
        state = States.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        activeLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (state == States.Playing)
        {
            Launch();
            Rotation();
        }
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }
    
    void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            LevelReload();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionOff = !collisionOff;
        }
    }
    
    void OnCollisionEnter(Collision collision) 
    {
        if (state == States.Dead || state == States.NextLevel || collisionOff)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Safe":
                print("ok");
                break;
            case "Finish":
                Finish();
                break;
            default:
                Lose();
                break;
        }        
    }

    void OnTriggerEnter(Collider other)
    {
        if (state == States.Dead || state == States.NextLevel)
        {
            return;
        }
        switch (other.gameObject.tag)
        {
            case "Battery":
                print("nrj++");
                break;
        }
    }

    void Lose()
    {
        state = States.Dead;
        audioSource.Stop();
        audioSource.PlayOneShot(boomSound);
        boomParticles.Play();
        flyParticles.Stop();
        Invoke("LevelReload", 2f);
    }

    void Finish()
    {
        state = States.NextLevel;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        finishParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }

    void LoadNextLevel()
    {
        if (activeLevelIndex == SceneManager.sceneCountInBuildSettings - 1)
        {
            activeLevelIndex = -1;
        }
        SceneManager.LoadScene(activeLevelIndex + 1);
    }

    void LevelReload() //on losing
    {
        SceneManager.LoadScene(activeLevelIndex);
    }

    void Launch()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if(audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
            }
        }
        else
        {
            audioSource.Pause();
            flyParticles.Stop();
        }
    }

    void Rotation()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotSpeed * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;
    }
}