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
        Damege,//ダメージ
        Death,//死亡
    };

    //プレイヤー
    [SerializeField]
    GameObject TargetObject;
    //　攻撃した後のフリーズ時間
    [SerializeField]
    private float freezeTime = 1.5f;
    //　攻撃中の時間
    [SerializeField]
    private float attackTime = 1.5f;
    //　攻撃中に当たり判定を出す時間
    [SerializeField]
    private float attack = 1.0f;
    //　ダメ―ジを受けた際の硬直時間
    [SerializeField]
    private float damageTime = 1.5f;

    //　HP
    [SerializeField]
    private int enemyHp = 3;

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

    //プレイヤーからダメージを受けたか
    [SerializeField]
    private bool isDamage;


    //攻撃の当たり判定用
    [SerializeField]
    private SphereCollider sphereCollider;

    // 切られたときの効果音
    public AudioClip cutSE1;
    public AudioClip cutSE2;
    public AudioClip cutSE3;
    public AudioClip cutSE4;
    public AudioClip specialSE;

    //追加
    GameObject Player;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        TargetObject = GameObject.Find("Player");
        //初めは待機状態
        SetState(EnemyState.Wait);

        //追加
        Player = GameObject.Find("Player");
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //ダメージを受けてエネミーステートがすでにだめーじじゃなかったら
        if (isDamage&& state != EnemyState.Damege && state != EnemyState.Death)
        {
            SetState(EnemyState.Damege);
            isDamage = false;
        }

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

            //時間になったら当たり判定を出す
            if (elapsedTime > attack)
            {
                AttackStart();
            }

            //攻撃が終わったら攻撃後の硬直へ
            if (elapsedTime > attackTime)
            {
                AttackEnd();
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
        else if (state == EnemyState.Damege)
        {
            elapsedTime += Time.deltaTime;


            //攻撃をくらったときにhpが0以下ならDeathへ
            if (enemyHp <= 0)
            {
                SetState(EnemyState.Death);
            }

            //硬直が終わったら追いかける
            if (elapsedTime > damageTime)
            {
                ////攻撃をくらった後にHpが0以下なら消す
                //if (enemyHp <= 0)
                //{
                //    player.Point += 50;
                //    player.Kill += 1;
                //    Destroy(this.gameObject);
                //}
                //else
                //{
                SetState(EnemyState.Chase);
                //}
            }
        }
        else if (state == EnemyState.Death)
        {
            elapsedTime += Time.deltaTime;
            //アニメーションが終わるのを待ってEnemyを消す
            if (elapsedTime > damageTime)
            {
                player.Point += 50;
                player.Kill += 1;
                Destroy(this.gameObject);
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
            //AttackStart();
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
            //AttackEnd();
            animator.SetBool("Walk", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Attack", false);
        }
        else if (tempState == EnemyState.Damege)
        {
            elapsedTime = 0f;
            //hpを減らす
            enemyHp -= 1; 
            //移動をできなくする
            agent.updatePosition = false;
            //攻撃の当たり判定消す
            AttackEnd();

            //攻撃状態の場合アニメーションを中断する
            animator.ResetTrigger("Attack");
            //ダメージを受けたアニメーション
            animator.SetTrigger("Damage");
        }
        else if(tempState == EnemyState.Death)
        {
            elapsedTime = 0f;
            //攻撃状態の場合アニメーションを中断する
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Death");

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

    //エネミーがダメージを受けたか
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "PlayerAttack")
        {
            isDamage = true;
            if (collider.gameObject.name == "Attack1")
            {
                // オーディオを再生
                AudioSource.PlayClipAtPoint(cutSE1, transform.position);
            }
            else if (collider.gameObject.name == "Attack2")
            {
                // オーディオを再生
                AudioSource.PlayClipAtPoint(cutSE2, transform.position);
            }
            else if (collider.gameObject.name == "Attack3")
            {
                // オーディオを再生
                AudioSource.PlayClipAtPoint(cutSE3, transform.position);
            }
            else if (collider.gameObject.name == "Attack4")
            {
                // オーディオを再生
                AudioSource.PlayClipAtPoint(cutSE4, transform.position);
            }
            else if (collider.gameObject.name == "AttackSkill1")
            {
                // オーディオを再生
                AudioSource.PlayClipAtPoint(specialSE, transform.position);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name == "Attackable")
        {
            isAttack = false;
        }
        if (collider.gameObject.tag == "PlayerAttack")
        {
            isDamage = false;
        }
    }



    void AttackStart()
    {
        //攻撃する時に当たり判定をオンにする
        sphereCollider.enabled = true;
    }

    void AttackEnd()
    {
        if (sphereCollider.enabled == true)
        {
            sphereCollider.enabled = false;
        }
    }
}