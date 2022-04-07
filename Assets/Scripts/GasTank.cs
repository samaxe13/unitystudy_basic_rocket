using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GasTank : MonoBehaviour
{

    [SerializeField] private Rocket _rocket;
    
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = gameObject.GetComponent<AudioSource>();
        _rocket.GasPickedUp.AddListener(PlayPickupSound);
    }

    private void PlayPickupSound() => _audioSource.Play();

    private void DestroyMePlease() => Destroy(gameObject);
}
