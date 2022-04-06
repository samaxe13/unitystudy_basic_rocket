using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float _gasTotal = 300f;
    [SerializeField] private float _gasPerSecond = 100f;
    [SerializeField] private float _gasPerTank = 50f;
    [SerializeField] private float _rotSpeed = 200f;
    [SerializeField] private float _flySpeed = 1000f;

    public float GasMax = 300f;

    public GasChanged GasChanged;
    public UnityEvent GasPickedUp;
    public UnityEvent FlyStart;
    public UnityEvent FlyEnd;
    public Death Death;
    public Finish Finish;

    private bool _collisionOff = false;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    private enum States { Playing, Dead, NextLevel };
    private States _state = States.Playing;

    private void Start()
    {
        _gasTotal = GasMax;
        _state = States.Playing;
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_state == States.Playing)
        {
            Launch();
            RotateRocket();
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
                    Win();
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
            GasChanged?.Invoke(_gasTotal);
            GasPickedUp?.Invoke();
            Destroy(gas.gameObject, 2f);
        }
    }

    private void Launch()
    {
        if (_gasTotal > 0f && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
        {
            _gasTotal -= _gasPerSecond * Time.deltaTime;
            GasChanged?.Invoke(_gasTotal);
            _rigidBody.AddRelativeForce(Vector3.up * _flySpeed * Time.deltaTime);
            if (_audioSource.isPlaying == false) FlyStart?.Invoke();
        }
        else FlyEnd?.Invoke();
    }

    private void RotateRocket() // TODO: 1) GetKey -> GetAxis 2) replace if statements with transform.Rotate(Axis * Vector3.forward * _rotSpeed * Time.deltaTime)
    {
        _rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A)) transform.Rotate(_rotSpeed * Time.deltaTime * Vector3.forward);
        else if (Input.GetKey(KeyCode.D)) transform.Rotate(_rotSpeed * Time.deltaTime * -Vector3.forward);
        _rigidBody.freezeRotation = false;
    }

    private void Lose()
    {
        _state = States.Dead;
        FlyEnd?.Invoke();
        Death?.Invoke("death");
    }

    private void Win()
    {
        _state = States.NextLevel;
        Finish?.Invoke("finish");
    }
}
[System.Serializable]
public class Death : UnityEvent<string> { }
[System.Serializable]
public class Finish : UnityEvent<string> { }
[System.Serializable]
public class GasChanged : UnityEvent<float> { }