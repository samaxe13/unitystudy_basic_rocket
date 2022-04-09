using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
public class GasTank : MonoBehaviour
{

    [SerializeField] private Rocket _rocket;
    
    private AudioSource _audioSource;
    private BoxCollider _collider;
    private Animator _animator;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _collider = gameObject.GetComponent<BoxCollider>();
        _animator = gameObject.GetComponent<Animator>();
        _rocket.GasPickedUp.AddListener(OnPickup);
    }

    private void OnPickup()
    {
        _collider.enabled = false;
        _audioSource.Play();
        _animator.SetTrigger("picked");
    }

    private void DestroyMePlease() => Destroy(gameObject);
}
