using Cysharp.Threading.Tasks;
using NUnit.Framework.Constraints;
using UnityEngine;

/// <summary>
/// プレイヤーの入力受付、一部行動処理
/// </summary>
public class AcceptPlayerInput
{
    private System.Action<MoveAction> _addMove = null;
    public void Initialize(System.Action<MoveAction> addMove)
    {
        _addMove = addMove;
    }
    
    public async UniTask AcceptInput()
    {
        while (true)
        {
            // プレイヤー入力の受付
            // 移動入力受付
            if (AcceptMove()) break;

            // 内部の実行

            // 1フレーム待機
            await UniTask.DelayFrame(1);

        }
    }

    private bool AcceptMove()
    {
        // 8方向の入力受付
        eDirectionEight inputDirection = AcceptDirInput();
        if(inputDirection== eDirectionEight.Invalid)return false;
        // 移動可否の判定
        CharacterObject character = CharacterManager.instance.GetPlayer();
        if(character == null ) return false;
        // アクセス回数を減らすため、1行の長さを減らすためキャッシュ
        int playerPosX = character.characterData.posX;
        int playerPosY = character.characterData.posY;
        // 指定方向の隣接座標を取得
        SquareObject moveSquare = MapSquareManager.instance.GetToDirSquare(playerPosX, playerPosY,inputDirection);
        // 移動先のマスが移動可能でなければ処理を抜ける
        if (!MapUtility.instance.CanMove(playerPosX, playerPosY, moveSquare, inputDirection)) return false;
        // 移動可能なので移動する
        // MoveAction生成
        MoveAction moveAction = new MoveAction();

        // 受けた入力に応じて内部的に移動
        SquareObject playerSquare = MapSquareManager.instance.GetSquare(playerPosX, playerPosY);
        ChebyshevMoveData moveData = new ChebyshevMoveData(playerSquare.squareData.ID, moveSquare.squareData.ID, inputDirection);
        moveAction.ExecuteData(character, moveData);
        // TurnProcessorの移動リストに追加
        _addMove?.Invoke(moveAction);

        return true;
    }

    /// <summary>
    /// 入力されている移動方向を返す
    /// </summary>
    /// <returns></returns>
    private eDirectionEight AcceptDirInput()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            // 上キーが押されている

            // 右キーが押されていたら右上
            if (Input.GetKey(KeyCode.RightArrow))
            {
                return eDirectionEight.UpRight;
            }
            // 左キーが押されていたら左上
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                return eDirectionEight.UpLeft;
            }
            return eDirectionEight.Up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // 下キーが押されている


            // 右キーが押されていたら右下
            if (Input.GetKey(KeyCode.RightArrow))
            {
                return eDirectionEight.DownRight;
            }
            // 左キーが押されていたら左下
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                return eDirectionEight.DownLeft;
            }
            return eDirectionEight.Down;
        }
        else
        {
            // 右キーが押されていたら右
            if (Input.GetKey(KeyCode.RightArrow))
            {
                return eDirectionEight.Right;
            }
            // 左キーが押されていたら左
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                return eDirectionEight.Left;
            }
        }
        // 何も押されていない
        return eDirectionEight.Invalid;

    }

}
