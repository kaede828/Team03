using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //ボスに渡すフラグ
    public static bool bossAttack;

    //武器表示・非表示
    [SerializeField] GameObject Sword;
    [SerializeField] GameObject BackSword;

    //移動
    private Vector3 Move;//移動
    bool MovePossible;//移動可能か
    [SerializeField] float Speed;//移動量
    private float DefaultSpeed;//初期移動量
    float moveX = 0f;
    float moveZ = 0f;

    private Vector3 LastPos;//最後の場所

    //回転の速さ
    [SerializeField] GameObject MainCamera;
    [SerializeField] float AngleSpeed = 6.0f;

    //ジャンプ
    private Vector3 Jump;//ジャンプ
    [SerializeField] float JumpPower=10.0f;//ジャンプ力

    //回避
    private bool Avert;
    [SerializeField] float AvertTime=0.2f;//回避時間
    [SerializeField] float AvertSpeed=15;
    float Timer;

    //体力
    public int MaxHP = 300;
    public int HP=0;
    [SerializeField] Image HPBarG;
    [SerializeField] Text HPtext;
    [SerializeField] Text MaxHPtext;

    //攻撃
    [SerializeField] GameObject Attack1;
    [SerializeField] GameObject Attack2;
    [SerializeField] GameObject Attack3;
    [SerializeField] GameObject Attack4;

    //必殺技
    [SerializeField] GameObject AttackSkill1;

    //強化ポイント
    public int Point;
    private string PointT;
    [SerializeField] Text PointText; // Textオブジェクト

    //強化メニュー
    [SerializeField] GameObject PowerUpMenu;
    [SerializeField] GameObject RedCursor;
    [SerializeField] GameObject GreenCursor;
    int SelectNum;

    //技が使えるか
    [SerializeField] Image SkillImage;
    bool Skill1=false;
    float SkillTime=0;

    //アニメーション
    private Animator animator;
    private const string key_isRun = "isRun";//フラグの名前
    private const string key_isJump = "isJump";
    private const string key_isAvert = "isAvert";
    private const string key_isAttack = "isAttack";
    private const string key_isDamage = "isDamage";
    private const string key_isSkill = "isSkill";
    AnimatorClipInfo[] clipInfo;
    string clipName;
    int animatorNum;

    CharacterController characterController;

    //パーティクル
    public GameObject particle;

    //倒した敵の数
    public int Kill=0;
    public static int KillEnemy=0;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        this.animator = GetComponent<Animator>();

        DefaultSpeed = Speed;
        PowerUpMenu.SetActive(false);

        HP = MaxHP;
    }

    void FixedUpdate()
    {
        bossAttack = false;

        PointT = Point.ToString();
        PointText.text = PointT;

        // アニメーションの情報取得
        clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        // 再生中のクリップ名
        clipName = clipInfo[0].clip.name;
        //アニメーションの状態
        if (clipName == "Sword And Shield Idle") animatorNum = 0;
        if (clipName == "Running") animatorNum = 1;
        if (clipName == "Sword And Shield Jump") animatorNum = 2;
        if (clipName == "Rowling") animatorNum = 3;
        if (clipName == "Sword And Shield Slash2") animatorNum = 4;
        if (clipName == "Sword And Shield Kick") animatorNum = 5;
        if (clipName == "Sword And Shield Slash") animatorNum = 6;
        if (clipName == "Sword And Shield Attack") animatorNum = 7;
        //スキル
        if (clipName == "Sword And Shield Casting") animatorNum = 8;

        //攻撃中でなければ
        if (animatorNum<4)
        {
            //移動
            moveX = Input.GetAxis("Horizontal") * Speed;
            moveZ = Input.GetAxis("Vertical") * Speed;
            Move = MainCamera.transform.rotation * new Vector3(moveX, 0, moveZ);
            characterController.SimpleMove(Move);

            //進行方向の回転
            Vector3 angle = new Vector3(0, Input.GetAxis("HorizontalRight") * AngleSpeed, 0);
            MainCamera.transform.Rotate(angle);

            //どこからどこに進んだか
            Vector3 diff = transform.position - LastPos;
            diff = new Vector3(diff.x, 0, diff.z);
            //最後の場所を更新
            LastPos = transform.position;


            //移動しているか
            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
            {
                this.animator.SetBool(key_isRun, false);
            }
            else
            {
                //ベクトルの大きさが0.01以上の時に向きを変える
                if (diff.magnitude > 0.01f)
                {
                    transform.rotation = Quaternion.LookRotation(diff);
                }
                this.animator.SetBool(key_isRun, true);
            }
        }

     
        //ジャンプ
        if (characterController.isGrounded)//地面についているか
        {
            if (Input.GetKeyDown("joystick button 0"))
            {
                //ジャンプ
                Jump.y = JumpPower;
                this.animator.SetTrigger(key_isJump);

            }
        }


        //重力
        Jump.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(Jump * Time.deltaTime);

        //回避
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown("joystick button 2"))
            {
                //Avert = true;
                this.animator.SetTrigger(key_isAvert);
            }
        }

        ////回避したら
        //if (Avert == true)
        //{
        //    if (Timer <= AvertTime)
        //    {
        //        //スピードアップ
        //        Speed = AvertSpeed;
        //        Timer += Time.deltaTime;

        //        //無敵処理
        //    }
        //    else
        //    {
        //        //通常状態へ
        //        Speed = DefaultSpeed;
        //        Timer = 0;
        //        Avert = false;
        //    }

        //}      
        //スキルタイム
        
        if(SkillTime>=20)
        {
            Skill1 = true;
        }
        else
        {
            SkillTime += Time.deltaTime;
            Skill1 = false;
        }
        SkillImage.fillAmount += 1.0f / 20 * Time.deltaTime;

        //攻撃
        if (characterController.isGrounded)//地面についていたら
        {
            if (Input.GetKeyDown("joystick button 1"))
            {
                this.animator.SetTrigger(key_isAttack);
                
            }
            //スキル1
            if(Skill1==true)
            {
                if (Input.GetKeyDown("joystick button 3"))
                {
                    this.animator.SetTrigger(key_isSkill);
                    //SkillTime = 0;
                    //SkillImage.fillAmount = 0;
                }
            }
           
        }

        //雑な攻撃の当たり判定
        switch (animatorNum)
        {
            case 0:
                Attack1.SetActive(false);
                Attack2.SetActive(false);
                Attack3.SetActive(false);
                Attack4.SetActive(false);
                AttackSkill1.SetActive(false);

                particle.SetActive(false);
                this.tag = ("Player");
                break;
            case 1:
                Attack1.SetActive(false);
                Attack2.SetActive(false);
                Attack3.SetActive(false);
                Attack4.SetActive(false);
                AttackSkill1.SetActive(false);

                particle.SetActive(false);
                this.tag = ("Player");
                Sword.SetActive(false);
                BackSword.SetActive(true);
                break;
            case 2:
                Sword.SetActive(false);
                BackSword.SetActive(true);
                break;
            case 3:
                this.tag = ("PlayerAvert");
                Sword.SetActive(false);
                BackSword.SetActive(true);
                break;
            case 4:
                Attack1.SetActive(true);
                Sword.SetActive(true);
                BackSword.SetActive(false);
                particle.SetActive(true);
                break;
            case 5:
                Attack2.SetActive(true);
                Attack1.SetActive(false);
                particle.SetActive(true);
                break;
            case 6:
                Attack3.SetActive(true);
                Attack2.SetActive(false);
                particle.SetActive(true);
                break;
            case 7:
                Attack4.SetActive(true);
                Invoke("ColliderReset", 0.5f);               
                Attack3.SetActive(false);
                particle.SetActive(true);
                break;
            case 8:
                Sword.SetActive(true);
                BackSword.SetActive(false);
                AttackSkill1.SetActive(true);
                break;
        }


        //メニュー        
        if (Input.GetKey("joystick button 4"))
        { PowerUpMenu.SetActive(true); }
        else
        { PowerUpMenu.SetActive(false);}

        //メニューが表示されていたら
        if(PowerUpMenu.activeSelf==true)
        {
            ////D-Pad　なぜか逆
            //float dph = Input.GetAxis("D_PAD_H");
            //float dpv = Input.GetAxis("D_PAD_V");
            //if ((dph != 0) || (dpv != 0))
            //{
            //    Debug.Log("D Pad:" + dph + "," + dpv);
            //}
            switch (SelectNum)
            {
                case 0:
                    //体力上限アップ
                    if (Point >= 500)
                    {
                        if (Input.GetKeyDown("joystick button 1"))
                        {
                            MaxHP += 50;
                            HP += 50;
                            Point -= 500;
                        }
                        GreenCursor.SetActive(true);
                        RedCursor.SetActive(false);                     
                    }
                    else
                    {
                        GreenCursor.SetActive(false);
                        RedCursor.SetActive(true);
                    }
                                   
                    break;
            }           
        }

        //HP表示
        MaxHPtext.text= MaxHP.ToString();
        HPtext.text = HP.ToString();

        //プレイヤー脂肪
        if(HP<=0)
        {
            SceneManager.LoadScene("Ending");
        }

        KillEnemy = Kill;

    }
    //攻撃の当たり判定を消す
    private void ColliderReset()
    {
        Attack4.SetActive(false);
    }

    //ダメージ
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "EnemyAttack")
        {
            HP -= 30;
            HPBarG.fillAmount =(float) HP/ MaxHP;
            this.animator.SetTrigger(key_isDamage);
        }
        if(collider.gameObject.tag=="BossEye")
        {
            bossAttack = true;
        }

    }

    public static bool BossAttack()
    {
        return bossAttack;
    }
    //倒した数
    public static int getKillEnemy()
    {
        return KillEnemy;
    }

    public void PlayerRecovery()
    {
        HP = MaxHP;
        HPBarG.fillAmount = (float)HP / MaxHP;
    }
}
