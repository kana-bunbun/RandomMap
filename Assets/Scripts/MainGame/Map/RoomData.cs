using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Analytics;
public class RoomData
{

    // 識別IDリスト
    public int ID { get; private set; } = -1;

    //部屋マスのIDリスト
    public List<int> squareIDList { get; private set; } = null;

    /// <summary>
    /// 使用前の準備
    /// </summary>
    public void SetUp(int ID, List<int> squareIDList)
    {
        this.ID = ID;
        this.squareIDList = squareIDList;

        SetRoomIDAll(ID);

    }

    /// <summary>
    /// 使用後の片付け
    /// </summary>
    public void Teardown()
    {
        ID = -1;

        SetRoomIDAll(ID);
        squareIDList = null;
    }
    private void SetRoomIDAll(int ID)
    {
        // リストがnullならreturn
        if (CommonModule.IsEmpty(this.squareIDList)) return;
        for (int i = 0; i < this.squareIDList.Count; i++)
        {
            SquareObject square = MapSquareManager.instance.GetSquare(this.squareIDList[i]);
            if (square == null) continue;
            square.squareData.SetRoomID(ID);
        }
    }
}
