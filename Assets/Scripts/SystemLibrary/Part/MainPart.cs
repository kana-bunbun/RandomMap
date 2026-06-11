using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ゲームのメインパート
/// </summary>
public class MainPart : PartBase
{
    // マップ管理クラス
    [SerializeField]
    private MapSquareManager mapManager = null;

    private DungeonProcessor _dungeonProcessor = null;
    public override async UniTask Initialize()
    {
        await base.Initialize();
        // マップ初期化
        mapManager?.Initialize();
        _dungeonProcessor = new DungeonProcessor();
        _dungeonProcessor.Inirialize();
    }

    public override async UniTask Execute()
    {
        eDungeonEndReason endReason = await _dungeonProcessor.Execute();
        // ダンジョンの終了要因に応じた処理

        switch (endReason)
        {
            case eDungeonEndReason.GameOver:
                // ゲームオーバーしたらタイトルパートへ遷移
                UniTask transT = PartManager.instance.TransitionPart(eGamePart.Title);
                break;
            case eDungeonEndReason.GameClear:
                // クリアしたらエンディングパート
                UniTask transE = PartManager.instance.TransitionPart(eGamePart.Ending);
                break;
        }
    }
}
