using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;

    private PlayerInput playerInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.Fire)
        {
            gun.Fire();
        }
    }
}
