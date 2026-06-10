using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 四方向用1マス分の移動データ
/// </summary>
public class ManhattanMoveData
{
    public int sourceSquareId = -1;
    public int targetSquareId = -1;
    eDirectionFour dir = eDirectionFour.Invalid;
    public ManhattanMoveData(int sourceSquareId, int targetSquareId, eDirectionFour dir)
    {
        this.sourceSquareId=sourceSquareId;
        this.targetSquareId=targetSquareId;
        this.dir = dir;
    }

}
