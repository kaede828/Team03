using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mat : MonoBehaviour
{
    public Material[] _material;           // 割り当てるマテリアル.
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(State.matFlag1==false)
        {
            this.GetComponent<Renderer>().material = _material[0];
        }

        if (State.matFlag1)
        {
            this.GetComponent<Renderer>().material = _material[1];
        }
    }
}
