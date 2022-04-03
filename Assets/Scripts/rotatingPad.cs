using UnityEngine;

public class rotatingPad : MonoBehaviour
{
    [SerializeField] [Range(100, 500)] float rotSpeed = 100f;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
    }
}
