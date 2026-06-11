using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// 1フロア
/// </summary>
public class FloorProcessor
{

    private TurnProcessor _turntProcessor = null;
    private eFloorEndReason _endReason = eFloorEndReason.Invalid;

    // 初期化
    public void Initialize()
    {
        _turntProcessor = new TurnProcessor();
    }
    public async UniTask Execute()
    {
        // フロア生成、準備
        SetUpFloor();
        // フロアの終了要因
        // 終了していない
        // 階段移動
        // ゲームオーバー
        // 中断

        // フロアが終了するまでループ
        while (_endReason==eFloorEndReason.Invalid)
        {
            // フロアが終了するまで1ターン処理をループ
            await _turntProcessor.Execute();
        }
        // フロア片付け
        TearDownFloor();

    }

    // フロア生成
    private void SetUpFloor()
    {
        _endReason = eFloorEndReason.Invalid;
        MapCreater.instance.CreateMap();
    }

    // フロア片付け
    private void TearDownFloor()
    {

    }

}
