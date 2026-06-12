using UnityEngine;

/// <summary>
/// マップ及びマスに繰り返し使われる関連処理
/// </summary>
class MapUtility
{

    private static MapUtility _instance=null;
    public static  MapUtility instance { get
        {
            if (_instance == null) _instance = new MapUtility();
            return _instance;
        }
    }

    /// <summary>
    /// 移動可否判定
    /// </summary>
    public bool CanMove(int startX,int startY,SquareObject moveSquare,eDirectionEight moveDirection)
    {
        // 移動先のマスにキャラクターがいる時移動不可
        if (moveSquare.squareData.characterID >= 0 )return false;
        // 移動先の地形が壁なら移動不可
        if (!moveSquare.squareData.terrain.CanMove()) return false;


        // 斜め移動でなければ終わり
        if (!moveDirection.IsSlant()) return true;
        // 斜め移動なら方向を分割、各方向の地形判定
        eDirectionFour[] separateDirection = moveDirection.SeparateDirecttion();
        for(int i = 0; i < separateDirection.Length; i++)
        {
            // 隣接マスを取得
            SquareObject square = MapSquareManager.instance.GetToDirSquare(startX, startY, separateDirection[i]);
            if (square == null) continue;
            if (square.squareData.terrain.CanMove()) continue;
            // 移動不可能マスなら移動不可を返す
            return false;
        }

        // ここまで来たら移動可能
        return true;
    }

}
