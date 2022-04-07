using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class RocketMenu : MonoBehaviour
{
    [SerializeField] private float _flySpeed = 100f;

    public UnityEvent FlyStart;
    public UnityEvent FlyEnd;
    public UnityEvent Death;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    private enum States { Playing, Dead };
    private States _state = States.Playing;

    private bool _collisionOff = false;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_state == States.Playing)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))
            {
                _rigidBody.AddRelativeForce(_flySpeed * Time.deltaTime * Vector3.up);
                if (_audioSource.isPlaying == false) FlyStart?.Invoke();
            }
            else FlyEnd?.Invoke();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_collisionOff && collision.gameObject.CompareTag("Respawn"))
        {
            _state = States.Dead;
            _rigidBody.AddRelativeForce(Vector3.back * _flySpeed);
            _collisionOff = true;
            Death?.Invoke();
            Invoke(nameof(LoadNextLevel), 3f);
        }
    }

    private void LoadNextLevel()
    {
        if (PlayerPrefs.GetInt("current level") == 0) SceneManager.LoadScene(1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("current level"));
    }
}
