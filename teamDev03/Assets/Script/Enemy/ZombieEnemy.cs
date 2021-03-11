using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Walk,//巡回
        Wait,//待機
        Chase,//追いかける
        Attack,//攻撃
        Freeze,//攻撃後硬直
        Damege//ダメージ
    };

    //プレイヤー
    //[SerializeField]
    GameObject TargetObject;
    //　攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 1.5f;
    //　攻撃中の時間
    [SerializeField]
    private float attackTime = 1.5f;
    //　経過時間
    private float elapsedTime;
    private Animator animator;

    NavMeshAgent agent;
    //　敵の状態
    [SerializeField]
    private EnemyState state;

    //プレイヤーを攻撃できるか
    [SerializeField]
    private bool isAttack;

    [SerializeField]
    private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        TargetObject = GameObject.Find("Player");
        //初めは待機状態
        SetState(EnemyState.Wait);
    }

    // Update is called once per frame
    void Update()
    {

        //待機状態
        if (state == EnemyState.Wait)
        {
            //プレイヤーを見つけたら追跡状態へ
            SetState(EnemyState.Chase);
        }
        //プレイヤー追跡状態
        else if (state == EnemyState.Chase)
        {
            //追跡対象を設定
            //if(TargetObject == null)
            //{
            //    TargetObject = GameObject.Find("Player");
            //}
            agent.SetDestination(TargetObject.transform.position);
            if (isAttack)
            {//攻撃可能な距離なら攻撃
                SetState(EnemyState.Attack);
            }
        }
        //
        else if (state == EnemyState.Attack)
        {
            elapsedTime += Time.deltaTime;

            //攻撃が終わったら攻撃後の硬直へ
            if (elapsedTime > attackTime)
            {
                SetState(EnemyState.Freeze);
            }
        }
        //　攻撃後のフリーズ状態
        else if (state == EnemyState.Freeze)
        {
            elapsedTime += Time.deltaTime;

            //硬直時間を超えたら再び追跡状態へ
            if (elapsedTime > freezeTime)
            {
                if (isAttack)
                {//攻撃可能なら続けて攻撃状態へ
                    SetState(EnemyState.Attack);
                }
                else
                {//それ以外なら追跡状態
                    SetState(EnemyState.Chase);
                }
            }
        }

        //navimeshの位置にEnemyを合わせる
        agent.nextPosition = transform.position;
        isAttack = false;
    }


    //　敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        state = tempState;
        if (tempState == EnemyState.Walk)
        {
            elapsedTime = 0f;
        }
        else if (tempState == EnemyState.Chase)
        {
            //移動可能
            agent.updatePosition = true;
            //　追いかける対象をセット
            agent.SetDestination(TargetObject.transform.position);
            animator.SetBool("Walk", true);
            animator.SetBool("Idle", false);
        }
        else if (tempState == EnemyState.Wait)
        {
            //待機状態
        }
        else if (tempState == EnemyState.Attack)
        {
            elapsedTime = 0f;
            //攻撃状態なら色を赤にする
            //GetComponent<Renderer>().material.color = Color.red;
            //攻撃処理
            AttackStart();
            //移動をできなくする
            agent.updatePosition = false;
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Attack", true);
        }
        else if (tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
            //色を白にする
            //GetComponent<Renderer>().material.color = Color.white;
            AttackEnd();
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Attack", false);
        }
        else if (tempState == EnemyState.Damege)
        {
            //移動をできなくする
            agent.updatePosition = false;

            //攻撃状態の場合アニメーションを中断する
            //ダメージを受けたアニメーション
        }
    }

    void OnTriggerStay(Collider collider)
    {

        //エネミーがプレイヤーを攻撃できる範囲にいるか
        if (collider.gameObject.name == "Attackable")
        {
            isAttack = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Attackable")
        {
            isAttack = false;
        }
    }

    void AttackStart()
    {
        //攻撃する時に当たり判定をオンにする
        sphereCollider.enabled = true;
    }

    void AttackEnd()
    {
        sphereCollider.enabled = false;
    }

}