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
    [Header("Events")]
    [Space]
    public UnityEvent GasPickedUp;
    public GasChanged GasChanged;
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

    private void OnTriggerEnter(Collider gasCollider)
    {
        if (_state == States.Playing && gasCollider.gameObject.CompareTag("Battery"))
        {
            GasPickedUp?.Invoke();
            _gasTotal += _gasPerTank;
            GasChanged?.Invoke(_gasTotal);
        }
    }

    private void Launch()
    {
        if (_gasTotal > 0f && Input.GetButton("Launch"))
        {
            _gasTotal -= _gasPerSecond * Time.deltaTime;
            GasChanged?.Invoke(_gasTotal);
            _rigidBody.AddRelativeForce(_flySpeed * Time.deltaTime * Vector3.up);
            if (_audioSource.isPlaying == false) FlyStart?.Invoke();
        }
        else FlyEnd?.Invoke();
    }

    private void RotateRocket()
    {
        _rigidBody.freezeRotation = true;
        transform.Rotate(_rotSpeed * Input.GetAxis("Rotate") * Time.deltaTime * Vector3.forward);
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