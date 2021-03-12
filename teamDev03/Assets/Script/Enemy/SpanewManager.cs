using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//同じエリアにあるスポナーを管理するクラス
public class SpanewManager : MonoBehaviour
{
    //エネミースポナーを格納する配列
    [SerializeField]
    private SpawnerScript[] enemySpanew;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //範囲内にプレイヤーがいるか
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            //管理下にある全てのエネミースポナーをアクティブにする
            for (int i = 0; i < enemySpanew.Length;i++)
            {
                enemySpanew[i].SpawnTrue();
            }
        }
    }

    //離れた時に
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            //管理下にある全てのエネミースポナーを非アクティブにする
            for (int i = 0; i < enemySpanew.Length; i++)
            {
                enemySpanew[i].SpawnFalse();
            }
        }
    }
}
