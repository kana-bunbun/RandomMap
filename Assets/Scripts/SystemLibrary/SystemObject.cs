using Cysharp.Threading.Tasks;
using Unity.Android.Types;
using UnityEngine;

// ゲーム全体で使用される機能の基底クラス
public abstract class SystemObject : MonoBehaviour
{
    // 初期化処理
    public abstract UniTask Initialize();
}
