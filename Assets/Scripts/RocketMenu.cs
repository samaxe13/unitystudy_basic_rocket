using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMenu : MonoBehaviour
{
    [SerializeField] float flySpeed = 100f;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip boomSound;
    [SerializeField] ParticleSystem flyParticles;
    [SerializeField] ParticleSystem boomParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool collisionOff = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) 
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if(audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(flySound);
                flyParticles.Play();
            }
        }
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (!collisionOff)
        {
            if(collision.gameObject.tag == "Respawn")
            {
                rigidBody.AddRelativeForce(Vector3.back * flySpeed);
                collisionOff = true;
                audioSource.Stop();
                audioSource.PlayOneShot(boomSound);
                boomParticles.Play();
                flyParticles.Stop();
                Invoke("LoadFirstLevel", 3f);
            }
        }
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }
}
