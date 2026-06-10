using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲーム全体で使用される昨日の管理
/// </summary>
public class SystemManager : MonoBehaviour
{
    // シリアライズ : 直列化(何かを何かに変換する処理)
    [SerializeField]
    private SystemObject[] _systemObjectList = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    private async UniTask Initialize()
    {
        // 各システムオブジェクトの生成
        for(int i = 0; i < _systemObjectList.Length; i++)
        {
            SystemObject origin=_systemObjectList[i];
            if (origin == null) continue;
            // オブジェクトの生成
            SystemObject systemObj= Instantiate(origin,transform);
            await systemObj.Initialize();
        }
        // タイトルパートの実行
        UniTask task = PartManager.instance.TransitionPart(eGamePart.Title);
    }
}
