using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ダンジョン実行処理
/// </summary>
public class DungeonProcessor
{
    // フロア実行クラス
    private FloorProcessor _floorProcessor = null;

    public void Inirialize()
    {
        _floorProcessor= new FloorProcessor();
        _floorProcessor.Initialize();
    }

    private eDungeonEndReason _eDungeonEndReason = eDungeonEndReason.Invalid;

    /// <summary>
    /// ダンジョン実行処理
    /// </summary>
    /// <returns></returns>
    public async UniTask<eDungeonEndReason> Execute()
    {

        _eDungeonEndReason = eDungeonEndReason.Invalid;

        // ダンジョンが終了するまでフロア実行処理をループ
        while (_eDungeonEndReason==eDungeonEndReason.Invalid)
        {
            await _floorProcessor.Execute();
        }
        return _eDungeonEndReason;
    }
}
