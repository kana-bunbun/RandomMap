using UnityEngine;

public class MapSquare 
{
    public int Id { get; private set; } = 1;
    public int posX { get; private set; } = -1;
    public int posY { get; private set; } = -1;

    // 不正な値で初期化
    public eTerrain terrain = eTerrain.Invalid;

    // 相互参照をしないように
    // 参照カウンタ式のGC(ガベージコレクション)が呼ばれなくなる
    // ガベージコレクション : 特定のアルゴリズムに則った自動的な参照の破棄
    // 参照カウンタ式のGC : 参照されている箇所をカウントしておき、カウントが0になったら解放される
    public int roomId { get; private set; } = -1;

    public MapSquare(int Id,int posX,int posY)
    {
        this.Id = Id;
        this.posX = posX;
        this.posY = posY;
    }
    public void SetTerrain(eTerrain terrain)
    {
        this.terrain = terrain;
    }

    public void SetRoomID(int id)
    {
        this.roomId = id;
    }


}
