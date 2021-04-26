﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    const float Max = 0.025f;


    //世界の状態
    [SerializeField] GameObject REAL;
    [SerializeField] GameObject DREAM;
    [SerializeField] GameObject GAP;

    [SerializeField] Image GapImage;
    [SerializeField] Image DreamImage;
    [SerializeField] Image RealImage;


    //プレイヤー
    GameObject Player;

    //クリスタルのマテリアル
    public Material mat;

    //剣のマテリアル
    public Material swordMat;

    public ParticleSystem particle;

    Color dreamcolor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    Color normalcolor = new Color(0.0f, 1.0f, 0.7f, 1.0f);

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

        //シェーダーの値取得
        mat = GetComponent<State>().mat;
        if (mat.HasProperty("_MyEmissionColor"))
        {
            //初期値
            mat.SetColor("_MyEmissionColor", new Color(0.0f, 0.8f, 1.0f, 0.0f));
        }

        if (swordMat.HasProperty("_MyEmissionColor"))
        {
            //初期値
            swordMat.SetColor("_MyEmissionColor", new Color(0.0f, 1.0f, 0.7f, 0.0f));
        }
        

        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        var color = particle.colorOverLifetime;
       
        gameTime += Time.deltaTime;
        fogDen = Mathf.Clamp(fogDen, Min, Max);
        Debug.Log(fogDen);
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

                //シェーダーの値変更
                mat.SetColor("_MyEmissionColor", new Color(0.0f, 0.8f, 1.0f, 0.0f));
                swordMat.SetColor("_MyEmissionColor", new Color(0.0f, 1.0f, 0.7f, 0.0f));

                color.color = normalcolor;
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

                //シェーダーの値変更
                mat.SetColor("_MyEmissionColor", new Color(1.0f, 0, 0.5f));
                swordMat.SetColor("_MyEmissionColor", new Color(1.0f, 1.0f, 1.0f, 0.0f));

                color.color = dreamcolor;
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
                for (int i = 0; i < spawner.Length; i++)
                {
                    //夢に移行するときにスポナーからすべての敵を消滅させる
                    spawner[i].DestroyEnemy();
                    //スポナーから敵が出現しないよう設定
                    spawner[i].SpawnFalse();
                }

                //シェーダーの値変更
                mat.SetColor("_MyEmissionColor", new Color(1.0f, 1.0f, 0));
                swordMat.SetColor("_MyEmissionColor", new Color(0.0f, 1.0f, 0.7f, 0.0f));

                color.color = normalcolor;
                break;
        }

        if (gameTime >= 0 && gameTime <= 60)
        {
            type = Game_Type.real;
            REAL.SetActive(true);
            DREAM.SetActive(false);
            GAP.SetActive(false);
            RealImage.fillAmount -= 1.0f / 60 * Time.deltaTime;
        }

        if (gameTime >= 60 && gameTime <= 120)
        {
            matFlag1 = true;
            type = Game_Type.dreame;
            REAL.SetActive(false);
            DREAM.SetActive(true);
            GAP.SetActive(false);
            DreamImage.fillAmount -= 1.0f / 60 * Time.deltaTime;
        }

        if (gameTime >= 120 && gameTime <= 150)
        {
            matFlag1 = false;
            type = Game_Type.interval;
            REAL.SetActive(false);
            DREAM.SetActive(false);
            GAP.SetActive(true);
            GapImage.fillAmount -= 1.0f / 30 * Time.deltaTime;
        }

        if (gameTime >= 150)
        {
            Player.GetComponent<Player>().PlayerRecovery();
            gameTime = 0;
            RealImage.fillAmount = 1;
            DreamImage.fillAmount = 1;
            GapImage.fillAmount = 1;
        }

    }
}
