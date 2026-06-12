using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

// 1回分の移動アクション
public class MoveAction
{

    // 移動キャラ
    private CharacterObject _character = null;
    // 移動情報
    private ChebyshevMoveData _moveData = null;

    /// <summary>
    /// 内部的な移動処理
    /// </summary>
    public void ExecuteData(CharacterObject character,ChebyshevMoveData moveData)
    {
        // 移動情報のキャッシュ
        _character = character;
        _moveData = moveData;

        // 内部的な移動
        character.characterData.SetSquare(MapSquareManager.instance.GetSquare(moveData.targetSquareID));
        character.characterData.SetDirection(moveData.dir);
    }

    /// <summary>
    /// 見た目上の処理
    /// </summary>
    /// <returns></returns>
    public async UniTask ExecuteObject(float durationSecond)
    {
        // 移動先、移動座標の取得
        
        // 移動元のマス
        SquareObject startSquare = MapSquareManager.instance.GetSquare(_moveData.sourceSquareID);
        // 移動後のマス
        SquareObject goalSquare = MapSquareManager.instance.GetSquare(_moveData.targetSquareID);

        // 座標の差を求める
        Vector3 startPos = startSquare.GetCharacterRoot().position;
        Vector3 goalPos = goalSquare.GetCharacterRoot().position;

        // 補間移動
        float elapsedSecond = 0.0f;

        while (elapsedSecond < durationSecond)
        {
            // 経過時間の累積
            elapsedSecond += Time.deltaTime;

            // 補間座標の取得
            float t = elapsedSecond / durationSecond;

            // 座標を補完する
            Vector3 movePos = Vector3.Lerp(startPos, goalPos, t);
            // 補間した座標の設定
            _character.SetPosition(movePos);
            // 1フレーム待機
            await UniTask.DelayFrame(1);
        }
        // 念のためゴール座標に設定
        _character.SetPosition(goalPos);
    }
}