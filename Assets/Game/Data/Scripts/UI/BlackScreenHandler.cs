using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreenHandler : MonoBehaviour
{

    [SerializeField] private GameObject blackScreen;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        blackScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= 10)
        {
            Destroy(gameObject);
        }
    }
}
