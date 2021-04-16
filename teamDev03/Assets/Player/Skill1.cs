using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour
{
    [SerializeField] float Skill1Time=1.2f;
    [SerializeField] float elapsedTime;
    // Start is called before the first frame update
    void OnEnable()
    {   
        GetComponent<BoxCollider>().enabled = false;
    }
    void OnDisable()
    {
        GetComponent<BoxCollider>().enabled = false;
        elapsedTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > Skill1Time  )
        {
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}
