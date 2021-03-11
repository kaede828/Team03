using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    //　出現させる敵を入れておく
    [SerializeField] GameObject enemy;
    //　次に敵が出現するまでの時間
    [SerializeField] float appearNextTime;
    //　この場所から出現する敵の数
    [SerializeField] int maxNumOfEnemys;
    //　今何人の敵を出現させたか（総数）
    private int numberOfEnemys;
    //　待ち時間計測フィールド
    private float elapsedTime;

    void Start()
    {
        numberOfEnemys = 0;
        elapsedTime = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        //　この場所から出現する最大数を超えてたら何もしない
        if (numberOfEnemys >= maxNumOfEnemys)
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
    }
    //　敵出現メソッド
    void AppearEnemy()
    {
        //　敵の向きをランダムに決定
        var randomRotationY = Random.value * 360f;

        GameObject.Instantiate(enemy, transform.position, Quaternion.Euler(0f, randomRotationY, 0f));
        
        numberOfEnemys++;
        elapsedTime = 0f;
    }
}
