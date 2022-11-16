using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    public int Damage => _damage;

    public void OnTriggerEnter(Collider hitInfo)
    {
        Player _player = hitInfo.GetComponent<Player>();

        if (_player != null)
        {
            Debug.Log("HIT Player");

            _player.TakeDamage(Damage);

            Destroy(gameObject);
        }

        //Instantiate(impactEffect, transform.position, transform.rotation);
    }
}