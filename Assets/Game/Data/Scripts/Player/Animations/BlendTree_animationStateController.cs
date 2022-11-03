using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendTree_animationStateController : MonoBehaviour
{
    
     Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float _acceleration = 2.0f;
    public float _decceleration = 2.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {
        bool isJogging = Input.GetKey("w");
        bool isWalkingBackwards = Input.GetKey("s");
        bool isStrafeLeft = Input.GetKey("a");
        bool isStrafeRight = Input.GetKey("d");
        bool isAzura = Input.GetKey("q");
        bool isAstra = Input.GetKey("e");
        bool isAerisa = Input.GetKey("r");

        if (isJogging && velocityZ < 0.5f)
        {
            velocityZ += Time.deltaTime * _acceleration;
        }

        if (isStrafeLeft && velocityX > -0.5)
        {
            velocityX -= Time.deltaTime * _acceleration;
        }

        if (isStrafeRight && velocityX > 0.5)
        {
            velocityX += Time.deltaTime * _acceleration;


            // decrease velocityZ
            if (!isJogging && velocityZ < 0.0f)
            {
                velocityZ -= Time.deltaTime * _decceleration;
            }

            //reset velocityZ
            if (!isJogging && velocityZ < 0.0f)
            {
                velocityZ = 0.0f;
            }

            animator.SetFloat("Velocity Z", velocityZ);
            animator.SetFloat("Velocity X", velocityX);

        }
    }
}

