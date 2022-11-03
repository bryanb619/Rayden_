using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;

    [SerializeField] AudioSource FowardSteps, StrafeStepsLeft, StrafeStepsRight, BackwardsSteps;  


    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {
    
        if (Input.GetKey("w"))
        {
            animator.SetBool("isJogging", true);
            if (!FowardSteps.isPlaying)
            {
                FowardSteps.Play();
            }
        }

        if (!Input.GetKey("w"))
        {
            animator.SetBool("isJogging", false);
            FowardSteps.Stop();
        }

        if (Input.GetKey("s"))
        {
            animator.SetBool("isWalkingBackwards", true);
            if (!BackwardsSteps.isPlaying)
            {
                BackwardsSteps.Play();
            }
        }


        if (!Input.GetKey("s"))
        {
            animator.SetBool("isWalkingBackwards", false);
            BackwardsSteps.Stop();
        }



        if (Input.GetKey("a"))
        {
            animator.SetBool("isStrafeLeft", true);
            if (!StrafeStepsLeft.isPlaying)
            {
                StrafeStepsLeft.Play();
            }
        }


        if (!Input.GetKey("a"))
        {
            animator.SetBool("isStrafeLeft", false);
            StrafeStepsLeft.Stop();
        }


        if (Input.GetKey("d"))
        {
            animator.SetBool("isStrafeRight", true);
            if (!StrafeStepsRight.isPlaying)
            {
                StrafeStepsRight.Play();
            }
        }

        if (!Input.GetKey("d"))
            {
                animator.SetBool("isStrafeRight", false);
            StrafeStepsRight.Stop();
            }


        if (Input.GetKey("q"))
            animator.SetBool("isAzura", true);

        if (!Input.GetKey("q"))
            animator.SetBool("isAzura", false);

        if (Input.GetMouseButtonDown(0))
            animator.SetBool("isAstra", true);

        if (!Input.GetMouseButtonDown(0))
            animator.SetBool("isAstra", false);

        if (Input.GetMouseButtonDown(1))
            animator.SetBool("isAerisa", true);

        if (!Input.GetMouseButtonDown(1))
            animator.SetBool("isAerisa", false);

    }

}
