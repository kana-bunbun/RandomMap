using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームパートの基底クラス
/// すべてのパートが持っていてほしい機能
/// </summary>
public abstract class PartBase : MonoBehaviour
{
    /// <summary>
    /// アプリケーション開始時に一度だけ呼ばれる初期化処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask Initialize()
    {
        // 自身を非アクティブにする
        gameObject.SetActive(false);
    }



    /// <summary>
    /// パートのメイン処理実行前に呼ばれる準備処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask SetUp()
    {
        // 自身をアクティブにする
        gameObject.SetActive(true);
    }

    /// <summary>
    /// パートの実行処理
    /// </summary>
    /// <returns></returns>
    public abstract UniTask Execute();
    /// <summary>
    /// パートのメイン処理実行後に呼ばれる片付け処理
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask TearDown()
    {
        gameObject.SetActive(false);
    }


}
