using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform[] points;
    [SerializeField] int destPoint = 0;
    private NavMeshAgent agent;
    Player p;
    Vector3 playerPos;
    public GameObject player;
    float distance;
    public int Hp;
    public bool isAttack;
    public bool isDamage;
    public float damage;
    bool aflag;
    //　経過時間
    private float elapsedTime;
    //管理ナンバー
    float aiCount;
    //硬直時間
    public float rigorTime = 1.5f;
    //攻撃時間
    public float attackTime = 1.5f;
    public float attackTime2 = 1.5f;

    private Animator anima;
    [SerializeField] float trackingRange = 3f;
    [SerializeField] float quitRange = 5f;
    [SerializeField] float stopDistance = 1.5f;
    [SerializeField] float moveDistance = 5f;
    [SerializeField] bool tracking = false;
    [SerializeField] static bool siyaflag = false;

    [SerializeField]
    private BoxCollider boxCollider1;
    [SerializeField]
    private GameObject siya;
    //[SerializeField]
    //private MeshRenderer mesh1;

    void Start()
    {
        p = GetComponent<Player>();
        Hp = 100;
        agent = GetComponent<NavMeshAgent>();
        elapsedTime = 0;
        aiCount = 1;
        // autoBraking を無効にすると、目標地点の間を継続的に移動します
        //(つまり、エージェントは目標地点に近づいても
        // 速度をおとしません)
        agent.autoBraking = false;
        aflag = false;
        anima = GetComponent<Animator>();
        //GotoNextPoint();

        //追跡したいオブジェクトの名前を入れる
        //player = GameObject.Find("Player");
    }


    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返します
        if (points.Length == 0)
            return;

        // エージェントが現在設定された目標地点に行くように設定します
        agent.destination = points[destPoint].position;

        // 配列内の次の位置を目標地点に設定し、
        // 必要ならば出発地点にもどります
        destPoint = (destPoint + 1) % points.Length;
    }


    void Update()
    {


        Debug.Log(aiCount);
        switch (aiCount)
        {
            case 1:
                Move();
                break;
            case 2:
                Freeze();
                break;
            case 3:
                Attack();
                break;
            case 4:
                Attack2();
                break;

        }
        if(isDamage)
        {

            elapsedTime = 0;
            Hp -= 1;
            anima.SetBool("Attack",false);
            anima.SetBool("Walk",false);
            anima.SetBool("Idle",false);
            anima.SetTrigger("Damge");
            aiCount = 1;
        }

        isAttack = false;
    }

    void OnDrawGizmosSelected()
    {
        //trackingRangeの範囲を赤いワイヤーフレームで示す
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        //quitRangeの範囲を青いワイヤーフレームで示す
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, quitRange);
    }

    #region ボス移動
    void Move()
    {
        anima.SetBool("Run", false);
        anima.SetBool("Walk", true);
        //Playerとこのオブジェクトの距離を測る
        playerPos = player.transform.position;
        distance = Vector3.Distance(this.transform.position, playerPos);
        agent.speed = 5;
        elapsedTime += Time.deltaTime;
        if (tracking)
        {
            if (elapsedTime > rigorTime)
            {
                aiCount = 2;
                elapsedTime = 0;
            }
            //追跡の時、quitRangeより距離が離れたら中止
            if (Player.BossAttack()==false)
                tracking = false;
        }
        else
        {
            //PlayerがtrackingRangeより近づいたら追跡開始
            if (Player.BossAttack())
            {
                aiCount = 2;
                tracking = true;

            }


            // エージェントが現目標地点に近づいてきたら、
            // 次の目標地点を選択します
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();

            }
        }
    }
    #endregion

    #region フリーズ
    void Freeze()
    {
        anima.SetBool("Walk", false);
        anima.SetBool("Idle", true);
        // 補完スピードを決める
        float speed = 0.1f;
        // ターゲット方向のベクトルを取得
        Vector3 relativePos = playerPos - this.transform.position;
        // 方向を、回転情報に変換
        Quaternion rotation = Quaternion.LookRotation(new Vector3(0, 0, relativePos.z));
        // 現在の回転情報と、ターゲット方向の回転情報を補完する
        transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        elapsedTime += Time.deltaTime;
        agent.speed = 0;
        if (elapsedTime > rigorTime)
        {

            aiCount = 3;
            elapsedTime = 0;
        }
    }
    #endregion

    void Attack()
    {
        anima.SetBool("Idle", false);
        anima.SetBool("Run", true);
        elapsedTime += Time.deltaTime;
        Vector3 vec = playerPos - this.transform.position;
        vec.Normalize();
        vec *= 10;
        this.GetComponent<Rigidbody>().velocity = vec;
        if (elapsedTime > attackTime)
        {
            elapsedTime = 0;
            aiCount = 4;
        }

    }

    void Attack2()
    {
        agent.SetDestination(playerPos);
        if (isAttack)
        {
            AttackStart();
                anima.SetBool("Attack", true);
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > attackTime2)
        {
            AttackEnd();
            aiCount = 1;
            elapsedTime = 0;
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

    void OnTriggerStay(Collider collider)
    {
        //エネミーがプレイヤーを攻撃できる範囲にいるか
        if (collider.gameObject.name == "Attackable")
        {
            isAttack = true;
        }
        if (collider.gameObject.name == "Siya")
        {
            siyaflag = true;
        }
    }

    //エネミーがダメージを受けたか
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "PlayerAttack")
        {
            isDamage = true;
        }

    }

    void AttackStart()
    {
        //攻撃する時に当たり判定をオンにする
        boxCollider1.enabled = true;
    }

    void AttackEnd()
    {
        if (boxCollider1.enabled == true)
        {
            boxCollider1.enabled = false;
        }
    }
}
