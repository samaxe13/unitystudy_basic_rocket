using UnityEngine;

public class rotatingPad : MonoBehaviour
{
    [SerializeField] [Range(100, 500)] float _rotSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotSpeed * Time.deltaTime);
    }
}
