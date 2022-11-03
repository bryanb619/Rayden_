using System;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    //[SerializeField]
    private Player Player;

    private const float DamageIncreaseOverTime = 1.5f;

    private float Firedamage = 5;

    private float MaxDamage = 100;

    private int damage;
    public int Damage => damage;

    private bool Contact;

    private void Start()
    {
        Contact = false;
    }

    private void Update()
    {
        CalculateDamage();
    }


    private void OnTriggerEnter(Collider hitInfo)
    {
        Player _player = hitInfo.GetComponent<Player>();

        if (_player != null)
        {
            Contact = true;


            //StartCoroutine(WaitForSeconds());

            Debug.Log("Fire damage in Player");



            //Firedamage = (Firedamage + 10);



        }
        else
        {
            Contact = false;
        }

    }

    private void CalculateDamage()
    {
        if (Contact)
        {
            Firedamage = Mathf.Clamp(Firedamage + (DamageIncreaseOverTime * Time.deltaTime), 0.0f, MaxDamage);
            damage = (int)Math.Ceiling(Firedamage);

            ApplyDamage();
        }
    }

    public void ApplyDamage()
    {
        Player.TakeDamage(Damage);
    }


    
    /*
    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSecondsRealtime(0);
    }
    */

}
