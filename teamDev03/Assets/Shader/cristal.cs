using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cristal : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        if(mat.HasProperty("_Color"))
        {
            mat.SetColor("_Color", Color.blue);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mat.SetColor("_Color", new Color(1, 0, 0));
    }
}
