using UnityEngine;

public class rotatingPad : MonoBehaviour
{
    [SerializeField][Range(100, 500)] private float _rotSpeed = 100f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * _rotSpeed * Time.deltaTime);
    }
}
