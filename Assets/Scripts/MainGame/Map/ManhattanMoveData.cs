using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 四方向用1マス分の移動データ
/// </summary>
public class ManhattanMoveData
{
    public int sourceSquareID = -1;
    public int targetSquareID = -1;
    eDirectionFour dir = eDirectionFour.Invalid;
    public ManhattanMoveData(int sourceSquareID, int targetSquareID, eDirectionFour dir)
    {
        this.sourceSquareID=sourceSquareID;
        this.targetSquareID=targetSquareID;
        this.dir = dir;
    }

}
