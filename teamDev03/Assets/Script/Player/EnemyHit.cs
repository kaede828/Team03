using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {

        //攻撃した相手がEnemyの場合
        if (collider.gameObject.tag == "Enemy")
        {

            Destroy(collider.gameObject);

        }
    }
}
