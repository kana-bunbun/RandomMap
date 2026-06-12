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
    private MapSquareManager _mapManager = null;

    // キャラクター管理クラス
    [SerializeField]
    private CharacterManager _characterManager = null;
    
    private DungeonProcessor _dungeonProcessor = null;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        // マップ初期化
        _mapManager?.Initialize();
        // キャラクター初期化
        _characterManager?.Initialize();
        _dungeonProcessor = new DungeonProcessor();
        _dungeonProcessor.Inirialize();
    }

    public override async UniTask SetUp()
    {
        await base.SetUp();
        // プレイヤーの生成
        CharacterManager.instance.CreatePlayer(0, 0);
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

    public override async UniTask TearDown()
    {
        await base.TearDown();

        // プレイヤー破棄
        CharacterManager.instance.DeleteCharacter(CharacterManager.instance.GetPlayer());

    }

}
