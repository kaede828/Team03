﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour
{
    private bool bossState;
    private float gameTime;

    public GameObject boss;
    public GameObject attack;

    public SpawnerScript[] spawner;

    private SkinnedMeshRenderer mesh;
    private BoxCollider col;
    private BoxCollider attackCol;

    public static bool matFlag1;

    enum Game_Type
    {
        real = 0,
        dreame = 1,
        interval = 2,
    }

    Game_Type type;

    // Start is called before the first frame update
    void Start()
    {
        matFlag1 = false;
        type = Game_Type.real;
        //falseの時は見えない、trueの時は見える
        bossState = false;
        gameTime = 0;
        mesh = boss.GetComponent<SkinnedMeshRenderer>();
        col = boss.GetComponent<BoxCollider>();
        attackCol = attack.GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        switch (type)
        {
            case Game_Type.real:
                bossState = false;

                //メッシュレンダラー、当たり判定のON/OFF
                mesh.enabled = false;
                col.enabled = false;
                attackCol.enabled = false;
                break;

            case Game_Type.dreame:
                bossState = true;

                //メッシュレンダラー、当たり判定のON/OFF
                mesh.enabled = true;
                col.enabled = true;
                //attackCol.enabled = true;
                for (int i = 0; i < spawner.Length; i++)
                {
                    //夢に移行するときにスポナーからすべての敵を消滅させる
                    spawner[i].DestroyEnemy();
                    //スポナーから敵が出現しないよう設定
                    spawner[i].SpawnFalse();
                }
                break;

            case Game_Type.interval:
                bossState = true;

                //メッシュレンダラー、当たり判定のON/OFF
                mesh.enabled = true;
                col.enabled = true;
               // attackCol.enabled = true;
                break;
        }

        if (gameTime >= 0 && gameTime <= 1)
        {
            matFlag1 = true;
            type = Game_Type.real;
            Debug.Log("現実");
        }

        if (gameTime >= 1 && gameTime <= 120)
        {
            type = Game_Type.dreame;
            Debug.Log("夢");
        }

        if (gameTime >= 120 && gameTime <= 180)
        {
            matFlag1 = false;
            type = Game_Type.interval;
            Debug.Log("狭間");
        }

        if (gameTime >= 180)
        {
            gameTime = 0;
        }

    }
}
