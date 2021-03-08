using UnityEngine;

public class SampleAnimation : MonoBehaviour
{

    // Animator コンポーネント
    private Animator animator;

    // 設定したフラグの名前
    private const string key_isRun = "isRun";
    private const string key_isJump = "isJump";

    // 初期化メソッド
    void Start()
    {
        // 自分に設定されているAnimatorコンポーネントを習得する
        this.animator = GetComponent<Animator>();
    }

    // 1フレームに1回コールされる
    void Update()
    {

        // 矢印下ボタンを押下している
        if (Input.GetKey(KeyCode.DownArrow))
        {
            // WaitからRunに遷移する
            this.animator.SetBool(key_isRun, true);
        }
        else
        {
            // RunからWaitに遷移する
            this.animator.SetBool(key_isRun, false);
        }

        // Wait or Run からJumpに切り替える処理
        // スペースキーを押下している
        if (Input.GetKey(KeyCode.Space))
        {
            // Wait or RunからJumpに遷移する
            this.animator.SetBool(key_isJump, true);
        }
        else
        {
            // JumpからWait or Runに遷移する
            this.animator.SetBool(key_isJump, false);
        }
    }
}
