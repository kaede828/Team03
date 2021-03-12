using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject Player;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        player.Point += 1;
    }
}
