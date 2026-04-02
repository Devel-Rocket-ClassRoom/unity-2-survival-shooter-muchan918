using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;

    private void Awake()
    {
        offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
