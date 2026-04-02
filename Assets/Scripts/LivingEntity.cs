using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamagable
{
    public float startingHealth = 100f;



    public void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        throw new System.NotImplementedException();
    }
}
