using UnityEngine;

public class RotatingPad : MonoBehaviour
{
    [SerializeField][Range(100, 500)] private float _rotSpeed = 100f;

    private void Update()
    {
        transform.Rotate(_rotSpeed * Time.deltaTime * Vector3.forward);
    }
}
