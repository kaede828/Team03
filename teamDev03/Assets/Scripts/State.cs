using System.Collections;
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

    float fogDen;
    //FogDensityの最小値、最大値
    const float Min = 0;
    const float Max = 0.03f;


    //世界の状態
    [SerializeField] GameObject REAL;
    [SerializeField] GameObject DREAM;
    [SerializeField] GameObject GAP;

    //プレイヤー
    GameObject Player;

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
        Player = GameObject.Find("Player");

        fogDen = Mathf.Min(fogDen, Min);
        fogDen = Mathf.Max(fogDen, Max);
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        switch (type)
        {
            case Game_Type.real:
                bossState = false;
                fogDen = 0;
                RenderSettings.fog = false;

                //メッシュレンダラー、当たり判定のON/OFF
                mesh.enabled = false;
                col.enabled = false;
                attackCol.enabled = false;
                break;

            case Game_Type.dreame:
                bossState = true;

                fogDen += 0.0001f;
                RenderSettings.fog = true;
                RenderSettings.fogDensity = fogDen;

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

                fogDen -= 0.0004f;
                RenderSettings.fog = true;
                RenderSettings.fogDensity = fogDen;

                //メッシュレンダラー、当たり判定のON/OFF
                mesh.enabled = true;
                col.enabled = true;
               // attackCol.enabled = true;
                break;
        }

        if (gameTime >= 0 && gameTime <= 60)
        {
            matFlag1 = true;
            type = Game_Type.real;
            REAL.SetActive(true);
            DREAM.SetActive(false);
            GAP.SetActive(false);
        }

        if (gameTime >= 60 && gameTime <= 120)
        {
            type = Game_Type.dreame;
            REAL.SetActive(false);
            DREAM.SetActive(true);
            GAP.SetActive(false);
        }

        if (gameTime >= 120 && gameTime <= 180)
        {
            matFlag1 = false;
            type = Game_Type.interval;
            REAL.SetActive(false);
            DREAM.SetActive(false);
            GAP.SetActive(true);
        }

        if (gameTime >= 180)
        {
            Player.GetComponent<Player>().PlayerRecovery();
            gameTime = 0;
        }

    }
}
