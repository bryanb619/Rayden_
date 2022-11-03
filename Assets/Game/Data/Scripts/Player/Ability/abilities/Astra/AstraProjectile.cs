using UnityEngine;

public class AstraProjectile : MonoBehaviour
{
    //private float Bspeed = 20f;
    //private int Bdamage = 20;
    //public Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
       // rb.velocity = transform.forward * Bspeed;
    }

    public void OnTriggerEnter(Collider hitInfo)
    {
        AIBehaviour enemy = hitInfo.GetComponent<AIBehaviour>();
        if (enemy != null)
        {
            enemy.TakeDamage(20);
            Debug.Log("HIT");
        }

        //Instantiate(impactEffect, transform.position, transform.rotation);
    }
}