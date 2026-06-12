using Mono.Cecil;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class MapSquareManager : MonoBehaviour
{
    public static MapSquareManager instance = null;

    [SerializeField]
    private SquareObject _originObject;

    // 生成された管理中のマスオブジェクト
    private List<SquareObject> _squareList = null;

    private List<RoomData> _roomList = null;
    private List<RoomData> _unuseRoomList = null;

    // オブジェクトプーリング
    // 役割を終えたオブジェクトを破棄せずに再利用する

    public void Initialize()
    {
        instance = this;
        // マスオブジェクトを必要数生成
        int squareCount = GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT;
        _squareList = new List<SquareObject>(squareCount);
        for (int i = 0; i < squareCount; i++)
        {
            // オブジェクトを生成
            SquareObject squareObject = Instantiate(_originObject, transform);
            // IDをもとに座標を設定
            GetPositionFromID(i, out int posX, out int posY);
            // セットアップ
            squareObject.SetUp(i, posX, posY);
            _squareList.Add(squareObject);
        }
        // 部屋情報の初期化
        _roomList = new List<RoomData>();
        _unuseRoomList = new List<RoomData>();
    }
    /// <summary>
    /// IDからマス座標を取得
    /// </summary>
    /// <param name="ID"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    private void GetPositionFromID(int ID, out int posX, out int posY)
    {
        posX = ID % GameConst.MAP_SQUARE_WIDTH_COUNT;
        posY = ID / GameConst.MAP_SQUARE_HEIGHT_COUNT;
    }

    /// <summary>
    /// マスの座標からIDに変換
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    private int GetIDFromPosition(int posX, int posY)
    {
        // マップの範囲内かどうかチェック
        if (posX < 0 || posX >= GameConst.MAP_SQUARE_WIDTH_COUNT ||
            posY < 0 || posY >= GameConst.MAP_SQUARE_HEIGHT_COUNT) return -1;


        return posY * GameConst.MAP_SQUARE_WIDTH_COUNT + posX;
    }

    /// <summary>
    /// ID指定のマス情報
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public SquareObject GetSquare(int ID)
    {
        if (!CommonModule.IsEnableIndex(_squareList, ID)) return null;

        return _squareList[ID];

    }

    /// <summary>
    /// 指定座標から指定方向の隣接マスを取得(四方向)
    /// </summary>
    public SquareObject GetToDirSquare(int x, int y, eDirectionFour direction)
    {
        // 隣接マス取得
        ToDirPosition(ref x, ref y, direction);
        // 座標指定のマス取得
        return GetSquare(x, y);
    }
    /// <summary>
    /// 指定座標から指定方向の隣接マスを取得(八方向)
    /// </summary>
    public SquareObject GetToDirSquare(int x, int y, eDirectionEight direction)
    {
        // 隣接マス取得
        ToDirPosition(ref x, ref y, direction);
        // 座標指定のマス取得
        return GetSquare(x, y);
    }

    /// <summary>
    /// 指定座標の指定方向への座標取得(四方向)
    /// </summary>
    private void ToDirPosition(ref int x, ref int y, eDirectionFour direction)
    {
        switch (direction)
        {
            case eDirectionFour.Up:
                y++;
                break;
            case eDirectionFour.Right:
                x++;
                break;
            case eDirectionFour.Down:
                y--;
                break;
            case eDirectionFour.Left:
                x--;
                break;
        }
    }
    /// <summary>
    /// 指定座標の指定方向への座標取得(八方向)
    /// </summary>
    private void ToDirPosition(ref int x, ref int y, eDirectionEight direction)
    {
        switch (direction)
        {
            case eDirectionEight.Up:
                y++;
                break;
            case eDirectionEight.Right:
                x++;
                break;
            case eDirectionEight.Down:
                y--;
                break;
            case eDirectionEight.Left:
                x--;
                break;
            case eDirectionEight.UpRight:
                x++;
                y++;
                break;
            case eDirectionEight.UpLeft:
                x--;
                y++;
                break;
            case eDirectionEight.DownRight:
                x++;
                y--;
                break;
            case eDirectionEight.DownLeft:
                x--;
                y--;
                break;

        }
    }

    /// <summary>
    /// 座標指定のマス情報
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public SquareObject GetSquare(int x, int y)
    {
        return GetSquare(GetIDFromPosition(x, y));
    }

    /// <summary>
    /// すべてのマスに対して指定の処理を実行
    /// System.Action : 関数を引数のようにして扱うことができる
    ///                 引数の指定はできるが
    ///                 戻り値の指定はできず、voidのものしかできない
    /// </summary>
    /// <param name="action">実行する処理(SquareObjectを引数にする)</param>
    public void ExecuteAllSquare(System.Action<SquareObject> action)
    {
        if (action == null || CommonModule.IsEmpty(_squareList)) return;
        for (int i = 0; i < _squareList.Count; i++)
        {
            action(_squareList[i]);
        }
    }


    /// <summary>
    /// 部屋の追加
    /// </summary>
    public void AddRoom(List<int> squareIDList)
    {
        // 使用可能な部屋を取得
        RoomData addRoom = GetCanUseRoom();
        // 使用リストに追加
        int addID = _roomList.Count;
        addRoom.SetUp(addID, squareIDList);
        _roomList.Add(addRoom);
    }

    /// <summary>
    /// 使用可能な部屋取得
    /// </summary>
    /// <returns></returns>
    private RoomData GetCanUseRoom()
    {
        // 未使用のものがなければインスタンスを生成
        // 配列に何もなければ未使用がない
        if (CommonModule.IsEmpty(_unuseRoomList)) return new RoomData();
        RoomData result = _unuseRoomList[0];
        _unuseRoomList.RemoveAt(0);
        // 未使用のものがあればそれを返す
        return result;
    }
    public void RemoveAllRoom()
    {
        if (CommonModule.IsEmpty(_roomList)) return;
        for (int i = 0; i < _roomList.Count; i++)
        {
            RoomData roomData = _roomList[i];
            if (roomData == null) continue;

            roomData.Teardown();
            _unuseRoomList.Clear();

        }
    }
    private void DeleteWall(SquareObject square)
    {

        int posX = square.squareData.posX;
        int posY = square.squareData.posY;
        SquareObject check;
        bool isDelete = true;
        for (int i = 0; i < (int)eDirectionFour.Max; i++)
        {
            eDirectionFour direction = (eDirectionFour)i;
                check = GetToDirSquare(posX, posY, direction);

            if (check != null && check.squareData.terrain != eTerrain.Wall) break;
            isDelete = false;
        }
        if (isDelete)
            square.gameObject.SetActive(false);
    }
}
