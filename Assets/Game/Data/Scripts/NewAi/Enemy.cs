using UnityEngine;

public class Enemy : MonoBehaviour
{
    private AIBehaviour AIBehaviour;

    [SerializeField] Animator animator;

    Vector3 previousPos;

    float curSpeeed;

    private int _TestDamage = 20;
    private int _KillAll = 1000;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckForAICheats();

        Vector3 curMove = transform.position - previousPos;
        curSpeeed = curMove.magnitude / Time.deltaTime;
        previousPos = transform.position;

        PlayAnim();

    }
    private void CheckForAICheats()
    {
        if (Input.GetKeyDown(KeyCode.J)) // TEST CODE
        {
            AIBehaviour.TakeDamage(_TestDamage);
            Debug.Log("20% of to all damage");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("1000% of damage, All enemies are now killed!");
            AIBehaviour.TakeDamage(_KillAll);
        }
    }

    private void PlayAnim()
    {
        if (curSpeeed >= 0.2)
        {
            //gameObject.GetComponent<Animator>();
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsIdle", false);
            // play anim

        }
        else if (curSpeeed <= 0.19)
        {
            // stop anim
            //gameObject.GetComponent<Animator>();
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsIdle", true);
        }
        //Debug.Log(" Nova speed is : " + curSpeeed);

    }
    public void ShootAnim()
    {
        animator.SetBool("IsAtacking", true);

    }

    public void ShootCancel()
    {
        animator.SetBool("IsAtacking", false);

    }
}