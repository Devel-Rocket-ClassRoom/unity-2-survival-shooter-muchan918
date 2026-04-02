using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private PlayerInput playerInput;
    private Camera mainCamera;

    public LayerMask groundLayer;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetAxis("Mouse X") == 0 && Input.GetAxis("Mouse Y") == 0) return;

        Ray ray = mainCamera.ScreenPointToRay(playerInput.MousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 lookTarget = hit.point;
            lookTarget.y = transform.position.y;
            transform.LookAt(lookTarget);
        }
    }
}