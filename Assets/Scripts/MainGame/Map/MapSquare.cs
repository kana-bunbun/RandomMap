using UnityEngine;

public class MapSquare
{
    public int ID

    { get; private set; } = 1;
    public int posX { get; private set; } = -1;
    public int posY { get; private set; } = -1;

    // 不正な値で初期化
    public eTerrain terrain = eTerrain.Invalid;

    // 相互参照をしないように
    // 参照カウンタ式のGC(ガベージコレクション)が呼ばれなくなる
    // ガベージコレクション : 特定のアルゴリズムに則った自動的な参照の破棄
    // 参照カウンタ式のGC : 参照されている箇所をカウントしておき、カウントが0になったら解放される
    public int roomID { get; private set; } = -1;

    // マスにいるキャラクターのID
    public int characterID { get; private set; } = -1;

    public MapSquare(int ID, int posX, int posY)
    {
        this.ID = ID;
        this.posX = posX;
        this.posY = posY;
    }
    public void SetTerrain(eTerrain terrain)
    {
        this.terrain = terrain;
    }

    public void SetRoomID(int ID)
    {
        this.roomID = ID;
    }


    /// <summary>
    /// マスにいるキャラクターの設定
    /// </summary>
    public void SetCharacter(int characterID)
    {
        this.characterID = characterID; 
    }
    /// <summary>
    /// マスにいるキャラクター情報の削除
    /// </summary>
    public void RemoveCharacter()
    {
        this.characterID = -1;
    }
}
