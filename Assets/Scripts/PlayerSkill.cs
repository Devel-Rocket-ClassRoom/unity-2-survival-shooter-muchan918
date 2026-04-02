using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private PlayerInput playerInput;
    private ISkill dashSkill;
    private ISkill explodeSkill;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        dashSkill = GetComponentInChildren<DashSkill>();
        explodeSkill = GetComponentInChildren<ExplodeSkill>();
    }

    private void Update()
    {
        if (playerInput.Dash) dashSkill.Use();
        if (playerInput.Explode) explodeSkill.Use();
    }
}