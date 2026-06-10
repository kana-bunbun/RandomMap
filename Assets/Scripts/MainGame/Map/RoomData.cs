using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
public class RoomData
{

    // 識別IDリスト
    public int Id { get; private set; } = -1;

    //部屋マスのIDリスト
    public List<int> squareIdList { get; private set; } = null;

    /// <summary>
    /// 使用前の準備
    /// </summary>
    public void SetUp(int Id, List<int> squareIdList)
    {
        this.Id = Id;
        this.squareIdList = squareIdList;

        SetRoomIdAll(Id);

    }

    /// <summary>
    /// 使用後の片付け
    /// </summary>
    public void Teardown()
    {
        Id = -1;

        SetRoomIdAll(Id);
        squareIdList = null;
    }
    private void SetRoomIdAll(int Id)
    {
        // リストがnullならreturn
        if (CommonModule.IsEmpty(this.squareIdList)) return;
        for (int i = 0; i < this.squareIdList.Count; i++)
        {
            SquareObject square = MapSquareManager.instance.GetSquare(this.squareIdList[i]);
            if (square == null) continue;
            square.squareData.SetRoomID(Id);
        }
    }
}
