using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;

    public UnityEvent energyPickedUp;
    public UnityEvent flyStart;
    public UnityEvent flyEnd;
    public death death;
    public finish finish;

    bool collisionOff = false;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum States {Playing, Dead, NextLevel};
    States state = States.Playing;

    void Start()
    {
        state = States.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (state == States.Playing)
        {
            Launch();
            Rotation();
        }
    }
    
    void OnCollisionEnter(Collision collision) 
    {
        if (state == States.Dead || state == States.NextLevel || collisionOff) return;
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

    void OnTriggerEnter(Collider energy)
    {
        if (state == States.Dead || state == States.NextLevel) return;
        switch (energy.gameObject.tag)
        {
            case "Battery":
                energyPickedUp?.Invoke();
                Destroy(energy.gameObject);
                break;
        }
    }
    
    void Launch()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed * Time.deltaTime);
            if(audioSource.isPlaying == false) flyStart?.Invoke();
        }
        else flyEnd?.Invoke();
    }

    void Rotation()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D)) transform.Rotate(-Vector3.forward * rotSpeed * Time.deltaTime);
        rigidBody.freezeRotation = false;
    }

    void Lose()
    {
        state = States.Dead;
        flyEnd?.Invoke();
        death?.Invoke("death");
    }

    void Finish()
    {
        finish?.Invoke("finish");
        state = States.NextLevel;
    }
}
[System.Serializable]
public class death : UnityEvent<string> { }
[System.Serializable]
public class finish : UnityEvent<string> { }