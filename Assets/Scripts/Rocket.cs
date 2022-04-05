using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    public float _gasMax = 300f;
    [SerializeField] private float _gasTotal = 300f;
    [SerializeField] private float _gasPerSecond = 100f;
    [SerializeField] private float _gasPerTank = 50f;
    [SerializeField] private float _rotSpeed = 200f;
    [SerializeField] private float _flySpeed = 1000f;

    public gasChanged gasChanged;
    public UnityEvent gasPickedUp;
    public UnityEvent flyStart;
    public UnityEvent flyEnd;
    public death death;
    public finish finish;

    private bool _collisionOff = false;


    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    enum States {Playing, Dead, NextLevel};
    States _state = States.Playing;

    private void Start()
    {
        _state = States.Playing;
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_state == States.Playing)
        {
            Launch();
            Rotation();
        }
    }
    
    private void OnCollisionEnter(Collision collision) 
    {
        if (_state == States.Playing && !_collisionOff)
        {
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
    }

    private void OnTriggerEnter(Collider gas)
    {
        if (_state == States.Playing && gas.gameObject.tag == "Battery")
        {
            gas.enabled = false;
            _gasTotal += _gasPerTank;
            gasChanged?.Invoke(_gasTotal);
            gasPickedUp?.Invoke();
            Destroy(gas.gameObject, 2f);
        }
    }
    
    private void Launch()
    {
        if (_gasTotal > 0f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            _gasTotal -= _gasPerSecond * Time.deltaTime;
            gasChanged?.Invoke(_gasTotal);
            _rigidBody.AddRelativeForce(Vector3.up * _flySpeed * Time.deltaTime);
            if(_audioSource.isPlaying == false) flyStart?.Invoke();
        }
        else flyEnd?.Invoke();
    }

    private void Rotation() // TODO: 1) GetKey -> GetAxis 2) replace if statements with transform.Rotate(Axis * Vector3.forward * _rotSpeed * Time.deltaTime)
    {
        _rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A)) transform.Rotate(Vector3.forward * _rotSpeed * Time.deltaTime); 
        else if (Input.GetKey(KeyCode.D)) transform.Rotate(-Vector3.forward * _rotSpeed * Time.deltaTime);
        _rigidBody.freezeRotation = false;
    }

    private void Lose()
    {
        _state = States.Dead;
        flyEnd?.Invoke();
        death?.Invoke("death");
    }

    private void Finish()
    {
        _state = States.NextLevel;
        finish?.Invoke("finish");
    }
}
[System.Serializable]
public class death : UnityEvent<string> { }
[System.Serializable]
public class finish : UnityEvent<string> { }
[System.Serializable]
public class gasChanged : UnityEvent<float> { }