﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    //　出現させる敵を入れておく
    [SerializeField] GameObject enemy;
    //　出現させた敵を入れておくリスト
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();
    //　次に敵が出現するまでの時間
    [SerializeField] float appearNextTime;
    //　この場所から出現する敵の数
    [SerializeField] int maxNumOfEnemys;
    //　今何人の敵を出現させたか（総数）
    [SerializeField] private int numberOfEnemys;
    //　待ち時間計測フィールド
    private float elapsedTime;
    //  このスポナーから敵を出現させることが出来るかどうか
    [SerializeField]
    private bool isSpawn;


    void Start()
    {
        numberOfEnemys = 0;
        elapsedTime = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        //　この場所から出現する最大数を超えてたら何もしない
        if (numberOfEnemys >= maxNumOfEnemys || isSpawn == false)
        {
            return;
        }
        //　経過時間を足す
        elapsedTime += Time.deltaTime;

        //　経過時間が経ったら
        if (elapsedTime > appearNextTime)
        {
            elapsedTime = 0f;

            AppearEnemy();
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            DestroyEnemy();
        }
    }
    //　敵出現メソッド
    void AppearEnemy()
    {
        //　敵の向きをランダムに決定
        var randomRotationY = Random.value * 360f;

        GameObject gameObject = GameObject.Instantiate(enemy, transform.position, Quaternion.Euler(0f, randomRotationY, 0f));
        //生成した敵をリストに入れる
        enemyList.Add(gameObject);
        
        numberOfEnemys++;
        elapsedTime = 0f;
    }

    //　全ての敵消滅メソッド
    public void DestroyEnemy()
    {  
        //リスト内全てのエネミーを消滅させる
        for (int i = 0; i < enemyList.Count; i++)
        {
            Destroy(enemyList[i]);
        }
        //リストをクリアする
        enemyList.Clear();
        numberOfEnemys = 0;
    }

    //　スポナーをアクティブにする
    public void SpawnTrue()
    {
        isSpawn = true;
    }

    public void SpawnFalse()
    {
        isSpawn = false;
    }
}
