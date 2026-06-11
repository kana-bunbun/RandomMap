using UnityEngine;

/// <summary>
/// ゲームキャラクターの基底クラス
/// </summary>
public abstract class CharacterBase
{
    // 識別用のID
    public int ID { get; private set; } = -1;
    // キャラのマス座標
    public int posX { get; private set; } = -1;
    public int posY { get; private set; } = -1;
    // 向き
    public eDirectionEight direction { get; private set; } = eDirectionEight.Invalid;

    // 使用前準備
    public void SetUp(int ID)
    {
        this.ID = ID;
    }
    // 使用後片付け
    public void Teardown()
    {
        this.ID = -1;
    }

    public void SetSquare (SquareObject square)
    {
        if (square == null) return;

        // 現在マスから取り除く
        SquareObject current = MapSquareManager.instance.GetSquare(posX, posY);
        if (current != null) current.squareData.RemoveCharacter();

        // 座標の変更
        posX = square.squareData.posX;
        posY = square.squareData.posY;

        // マスにキャラクターIDを設定
        square.squareData.SetCharacter(ID);
    }
}
