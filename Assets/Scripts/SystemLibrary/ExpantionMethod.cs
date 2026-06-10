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
        if (result > (int)eDirectionFour.Max) result -= (int)eDirectionFour.Max;

        return (eDirectionFour)result;
    }

}
