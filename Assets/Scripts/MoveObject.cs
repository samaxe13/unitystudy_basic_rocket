using UnityEngine;

[DisallowMultipleComponent]

public class MoveObject : MonoBehaviour
{
    [SerializeField] private Vector3 _movePosition;
    [SerializeField] private float _moveSpeed;
    [SerializeField][Range(0, 1)] private float _moveProgress;

    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _moveProgress = Mathf.PingPong(Time.time * _moveSpeed, 1f);
        Vector3 offset = _movePosition * _moveProgress;
        transform.position = _startPosition + offset;
    }
}
