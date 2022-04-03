using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RocketMenu : MonoBehaviour
{
    [SerializeField] float flySpeed = 100f;

    public UnityEvent flyStart;
    public UnityEvent flyEnd;
    public UnityEvent death;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum States {Playing, Dead};
    States state = States.Playing;

    bool collisionOff = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == States.Playing)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) 
            {
                rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
                if(audioSource.isPlaying == false) flyStart?.Invoke();
            }
            else flyEnd?.Invoke();  
        }
        
    }

    void OnCollisionEnter(Collision collision) 
    {
        if (!collisionOff)
        {
            if(collision.gameObject.tag == "Respawn")
            {
                state = States.Dead;
                rigidBody.AddRelativeForce(Vector3.back * flySpeed);
                collisionOff = true;
                death?.Invoke();
                Invoke("LoadNextLevel", 3f);
            }
        }
    }

    void LoadNextLevel()
    {
        if(PlayerPrefs.GetInt("current level") == 0) SceneManager.LoadScene(1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("current level"));
    }
}
