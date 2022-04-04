using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    [SerializeField] public float gasTotal = 300f;
    [SerializeField] float gasPerSecond = 100f;
    [SerializeField] float gasPerTank = 50f;
    [SerializeField] float rotSpeed = 100f;
    [SerializeField] float flySpeed = 100f;

    public UnityEvent gasChanged;
    public UnityEvent gasPickedUp;
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

    void OnTriggerEnter(Collider gas)
    {
        if (state == States.Dead || state == States.NextLevel) return;
        if (gas.gameObject.tag == "Battery")
        {
            gasTotal += gasPerTank;
            gasChanged?.Invoke();
            gasPickedUp?.Invoke();
            Destroy(gas.gameObject);
        }
    }
    
    void Launch()
    {
        if (gasTotal > 0f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            gasTotal -= gasPerSecond * Time.deltaTime;
            gasChanged?.Invoke();
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