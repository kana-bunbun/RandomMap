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

    public override async UniTask Initialize()
    {
        await base.Initialize();
        // マップ初期化
        mapManager?.Initialize();
    }
    public override async UniTask Execute()
    {
        MapCreater.instance.CreateMap();
    }
}
