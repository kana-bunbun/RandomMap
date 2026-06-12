using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 八方向用1マス分の移動データ
/// </summary>
public class ChebyshevMoveData
{
    public int sourceSquareID = -1;
    public int targetSquareID = -1;
    public eDirectionEight dir = eDirectionEight.Invalid;
    public ChebyshevMoveData(int sourceSquareID, int targetSquareID, eDirectionEight dir)
    {
        this.sourceSquareID = sourceSquareID;
        this.targetSquareID = targetSquareID;
        this.dir = dir;
    }

}
