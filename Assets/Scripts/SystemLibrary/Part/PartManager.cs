using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームのパート管理
/// </summary>
public class PartManager : SystemObject
{
    // シングルトンインスタンスへの参照
    // カスタムプロパティによってクラス外から参照可、代入不可と設定
    public static PartManager instance { get; private set; } = null;
    // プロジェクト上のプレハブへの参照
    [SerializeField]
    private PartBase[] _partOriginList = null;

    // 生成されたパートオブジェクトへの参照
    private PartBase[] _partList = null;

    // 現在実行中のパート
    private PartBase _currentPart = null;

    public override async UniTask Initialize()
    {
        // シングルトン：インスタンスが1つしかないことを保証しつつ、
        //               その単一のインスタンスをどこからでも参照できるデザインパターン

        // ①コンストラクタのアクセシビリティをprivateに → クラスの外側から new できなくなる
        // ②public で static な自身への参照を持つ
        instance = this;

        // パート数のキャッシュ
        int partLength = _partOriginList.Length;

        _partList = new PartBase[partLength];
        // パートオブジェクトの生成
        for (int i = 0; i < partLength; i++)
        {
            PartBase origin = _partOriginList[i];
            if (origin == null) continue;
            _partList[i] = Instantiate(origin, transform);
            // パートオブジェクトの初期化処理
            await _partList[i].Initialize();
        }

    }

    public async UniTask TransitionPart(eGamePart nextPart)
    {
        // 現在のパートの片付け
        if (_currentPart != null)await _currentPart.TearDown();

            // 次のパートの準備
            /// </summary>
            _currentPart = _partList[(int)nextPart];
        await _currentPart.SetUp();
        // 次のパートの実行
        UniTask task = _currentPart.Execute();
    }
}
