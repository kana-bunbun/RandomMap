using Cysharp.Threading.Tasks;
using System.Collections.Generic;
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
        _turntProcessor.Initialize();
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
        // ランダムフロア生成
        MapCreater.instance.CreateMap();

        List<SquareObject> _roomSquareList = new List<SquareObject>();
        // ランダムな部屋マスにプレイヤー配置
        MapSquareManager.instance.ExecuteAllSquare(square =>
        {
            // nullまたは部屋マスでなければreturn
            if (square == null ||
            square.squareData.terrain != eTerrain.Room) return;
            _roomSquareList.Add(square);
        });
        if (!CommonModule.IsEmpty(_roomSquareList))
        {
            SquareObject playerSquare = _roomSquareList[Random.Range(0, _roomSquareList.Count)];
            CharacterManager.instance.GetPlayer()?.SetSquare(playerSquare);
        }
        // フロア維持状態に遷移
        _endReason = eFloorEndReason.Invalid;
    }
    // フロア片付け
    private void TearDownFloor()
    {

    }

}
