using System.Data.SqlTypes;
using UnityEngine;

/// <summary>
/// 拡張メソッド
/// </summary>
public static class ExpantionMethod
{
    /// <summary>
    /// 逆方向を取得
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static eDirectionFour ReverseDerection(this eDirectionFour direction)
    {
        int result = (int)direction+2;
        if (result >= (int)eDirectionFour.Max) result -= (int)eDirectionFour.Max;

        return (eDirectionFour)result;
    }
    public static bool CanMove(this eTerrain terrain)
    {
        return terrain != eTerrain.Wall;
    }
    /// <summary>
    /// 斜め方向の場合trueを返す関数
    /// </summary>
    public static bool IsSlant(this eDirectionEight dir)
    {
        switch (dir)
        {
            case eDirectionEight.UpRight:
            case eDirectionEight.UpLeft:
            case eDirectionEight.DownRight:
            case eDirectionEight.DownLeft:
                return true;
        }
        return false;
    }

    /// <summary>
    /// 斜め方向の分割
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static eDirectionFour[] SeparateDirecttion(this eDirectionEight direction)
    {
        eDirectionFour[] result = new eDirectionFour[2];

        switch (direction)
        {
            case eDirectionEight.UpRight:
                result[0] = eDirectionFour.Up;
                result[1] = eDirectionFour.Right;
                break;
            case eDirectionEight.DownRight:
                result[0] = eDirectionFour.Down;
                result[1] = eDirectionFour.Right;
                break;
            case eDirectionEight.UpLeft:
                result[0] = eDirectionFour.Up;
                result[1] = eDirectionFour.Left;
                break;
            case eDirectionEight.DownLeft:
                result[0] = eDirectionFour.Down;
                result[1] = eDirectionFour.Left;
                break;
            //    // 斜め方向でなければ不正値を設定
            //default:
            //    result[0] = eDirectionFour.Invalid;
            //    result[1] = eDirectionFour.Invalid;
            //    break;
        }
        return result;
    }
}
