//using NUnit.Framework;
//using System.Collections.Generic;
//using UnityEngine;


//public class MapSquareManager : MonoBehaviour
//{
//    public static MapSquareManager instance = null;

//    [SerializeField]
//    private SquareObject _originObject;
    
//    // 生成された管理中のマスオブジェクト
//    private List<SquareObject> _squareList = null;

//    private List<RoomData> _roomList = null;
//    private List<RoomData> _unuseRoomList = null;

//    // オブジェクトプーリング
//    // 役割を終えたオブジェクトを破棄せずに再利用する

//    public void Initialize()
//    {
//        instance = this;
//        // マスオブジェクトを必要数生成
//        int squareCount = GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT;
//     _squareList= new List<SquareObject>(squareCount);
//        for(int i = 0; i < squareCount; i++)
//        {
//            // オブジェクトを生成
//            SquareObject squareObject = Instantiate(_originObject, transform);
//            // IDをもとに座標を設定
//            GetPositionFromId(i, out int posX, out int posY);
//            // セットアップ
//            squareObject.SetUp(i, posX, posY);
//            _squareList.Add(squareObject);
//        }
//        // 部屋情報の初期化
//        _roomList = new List<RoomData>();
//        _unuseRoomList = new List<RoomData>();
//    }
//    /// <summary>
//    /// IDからマス座標を取得
//    /// </summary>
//    /// <param name="Id"></param>
//    /// <param name="posX"></param>
//    /// <param name="posY"></param>
//    private void GetPositionFromId(int Id,out int posX,out int posY)
//    {
//        posX = Id % GameConst.MAP_SQUARE_WIDTH_COUNT;
//        posY = Id / GameConst.MAP_SQUARE_HEIGHT_COUNT;
//    }

//    /// <summary>
//    /// マスの座標からIDに変換
//    /// </summary>
//    /// <param name="posX"></param>
//    /// <param name="posY"></param>
//    /// <returns></returns>
//    private int GetIdFromPosition(int posX, int posY)
//    {
//        // マップの範囲内かどうかチェック
//        if (posX < 0 || posX >= GameConst.MAP_SQUARE_WIDTH_COUNT ||
//            posY < 0 || posY >= GameConst.MAP_SQUARE_HEIGHT_COUNT) return -1;


//        return posY * GameConst.MAP_SQUARE_WIDTH_COUNT + posX;
//    }

//    /// <summary>
//    /// ID指定のマス情報
//    /// </summary>
//    /// <param name="Id"></param>
//    /// <returns></returns>
//    public SquareObject GetSquare(int Id)
//    {
//        if (!CommonModule.IsEnableIndex(_squareList, Id)) return null;

//        return _squareList[Id];
    
//    }

//    public SquareObject GetToDirSquare(int x,int y,eDirectionFour direction)
//    {
//        // 隣接マス取得
//        ToDirPosition(ref x,ref y,direction);
//        // 座標指定のマス取得


//        return GetSquare(x, y);
//    }

//    /// <summary>
//    /// 指定座標の指定方向への座標取得
//    /// </summary>
//    /// <param name="x"></param>
//    /// <param name="y"></param>
//    /// <param name="direction"></param>
//    private void ToDirPosition(ref int x, ref int y,eDirectionFour direction)
//    {
//        switch (direction)
//        {
//            case eDirectionFour.Up:
//                y++;
//                break;
//            case eDirectionFour.Right:
//                x++;
//                break;
//            case eDirectionFour.Down:
//                y--;
//                break;
//            case eDirectionFour.Left:
//                x--;
//                break;
//        }
//    }

//    /// <summary>
//    /// 座標指定のマス情報
//    /// </summary>
//    /// <param name="x"></param>
//    /// <param name="y"></param>
//    /// <returns></returns>
//    public SquareObject GetSquare(int x,int y)
//    {
//        return GetSquare(GetIdFromPosition(x,y));
//    }

//    /// <summary>
//    /// すべてのマスに対して指定の処理を実行
//    /// System.Action : 関数を引数のようにして扱うことができる
//    ///                 引数の指定はできるが
//    ///                 戻り値の指定はできず、voidのものしかできない
//    /// </summary>
//    /// <param name="action">実行する処理(SquareObjectを引数にする)</param>
//    public void ExecuteAllSquare(System.Action<SquareObject> action)
//    {
//        if (action == null || CommonModule.IsEmpty(_squareList)) return;
//        for (int i = 0; i < _squareList.Count; i++)
//        {
//            action(_squareList[i]);
//        }
//    }


//    /// <summary>
//    /// 部屋の追加
//    /// </summary>
//    public void AddRoom(List<int> squareIdList)
//    {
//        // 使用可能な部屋を取得
//        RoomData addRoom = GetCanUseRoom();
//        // 使用リストに追加
//        int addId = _roomList.Count;
//        addRoom.SetUp(addId,squareIdList);
//        _roomList.Add(addRoom);
//    }

//    /// <summary>
//    /// 使用可能な部屋取得
//    /// </summary>
//    /// <returns></returns>
//    private RoomData GetCanUseRoom()
//    {
//        // 未使用のものがなければインスタンスを生成
//        // 配列に何もなければ未使用がない
//        if (CommonModule.IsEmpty(_unuseRoomList)) return new RoomData();
//        RoomData result = _unuseRoomList[0];
//        _unuseRoomList.RemoveAt(0);
//        // 未使用のものがあればそれを返す
//        return result;
//    }
//    public void RemoveAllRoom()
//    {
//        if(CommonModule.IsEmpty(_roomList)) return;
//        for(int i = 0; i < _roomList.Count; i++)
//        {
//            RoomData roomData = _roomList[i];
//            if (roomData == null) continue;

//            roomData.Teardown();
//            _unuseRoomList.Clear();

//        }
//    }
//}

using System.Collections.Generic;
using UnityEngine;

public class MapSquareManager : MonoBehaviour {
	public static MapSquareManager instance = null;

	// マスオブジェクトのオリジナル
	[SerializeField]
	private SquareObject _originObject;

	/// <summary>
	/// 生成された管理中のマスオブジェクトリスト
	/// </summary>
	private List<SquareObject> _squareList = null;

	// オブジェクトプーリング：役割を終えたオブジェクトを破棄せずに、プールしておき、再利用する。
	// 使用中の部屋リスト
	private List<RoomData> _roomList = null;
	// 未使用状態の部屋リスト
	private List<RoomData> _unuseRoomList = null;

	public void Initialize() {
		instance = this;
		// マスオブジェクトを必要数生成
		int squareCount = GameConst.MAP_SQUARE_HEIGHT_COUNT * GameConst.MAP_SQUARE_WIDTH_COUNT;
		_squareList = new List<SquareObject>(squareCount);
		for (int i = 0; i < squareCount; i++) {
			// オブジェクト生成
			SquareObject squareObject = Instantiate(_originObject, transform);
			// セットアップ
			int posX, posY;
			GetPositionFromID(i, out posX, out posY);
			squareObject.SetUp(i, posX, posY);
			_squareList.Add(squareObject);
		}
		// 部屋情報の初期化
		_roomList = new List<RoomData>();
		_unuseRoomList = new List<RoomData>();
	}

	/// <summary>
	/// IDからマス座標取得
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="posX"></param>
	/// <param name="posY"></param>
	private void GetPositionFromID(int ID, out int posX, out int posY) {
		posX = ID % GameConst.MAP_SQUARE_WIDTH_COUNT;
		posY = ID / GameConst.MAP_SQUARE_WIDTH_COUNT;
	}

	/// <summary>
	/// マスの座標をIDに変換
	/// </summary>
	/// <param name="posX"></param>
	/// <param name="posY"></param>
	/// <returns></returns>
	private int GetIDFromPosition(int posX, int posY) {
		// マップの範囲内か否かチェック
		if (posX < 0 || posX >= GameConst.MAP_SQUARE_WIDTH_COUNT ||
			posY < 0 || posY >= GameConst.MAP_SQUARE_HEIGHT_COUNT) return -1;

		return posY * GameConst.MAP_SQUARE_WIDTH_COUNT + posX;
	}

	/// <summary>
	/// ID指定のマス情報取得
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	public SquareObject GetSquare(int ID) {
		if (!CommonModule.IsEnableIndex(_squareList, ID)) return null;

		return _squareList[ID];
	}

	/// <summary>
	/// 指定座標から指定方向の隣接マスを取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	/// <returns></returns>
	public SquareObject GetToDirSquare(int x, int y, eDirectionFour dir) {
		// 隣接座標取得
		ToDirPosition(ref x, ref y, dir);
		// 座標指定のマス取得
		return GetSquare(x, y);
	}

	/// <summary>
	/// 指定座標の指定方向への隣接座標取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="dir"></param>
	private void ToDirPosition(ref int x, ref int y, eDirectionFour dir) {
		switch (dir) {
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
	/// 座標指定のマス取得
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public SquareObject GetSquare(int x, int y) {
		return GetSquare(GetIDFromPosition(x, y));
	}

	/// <summary>
	/// 全てのマスに対して指定処理実行
	/// </summary>
	/// <param name="action">実行する処理(SquareObjectを引数に取る)</param>
	public void ExecuteAllSquare(System.Action<SquareObject> action) {
		if (action == null || CommonModule.IsEmpty(_squareList)) return;

		for (int i = 0; i < _squareList.Count; i++) {
			action(_squareList[i]);
		}
	}

	/// <summary>
	/// 部屋の追加
	/// </summary>
	/// <param name="squareIDList"></param>
	public void AddRoom(List<int> squareIDList) {
		// 使用可能な部屋を取得
		RoomData addRoom = GetUsableRoom();
		// 使用リストに追加
		int roomID = _roomList.Count;
		addRoom.SetUp(roomID, squareIDList);
		_roomList.Add(addRoom);
	}

	/// <summary>
	/// 使用可能な部屋取得
	/// </summary>
	/// <returns></returns>
	private RoomData GetUsableRoom() {
		// 未使用がなければインスタンスを生成
		if (CommonModule.IsEmpty(_unuseRoomList)) return new RoomData();
		// 未使用のものがあればそれを返す
		RoomData result = _unuseRoomList[0];
		_unuseRoomList.RemoveAt(0);
		return result;
	}

	/// <summary>
	/// 全ての部屋の削除
	/// </summary>
	public void RemoveAllRoom() {
		if (CommonModule.IsEmpty(_roomList)) return;

		for (int i = 0; i < _roomList.Count; i++) {
			RoomData roomData = _roomList[i];
			if (roomData == null) continue;

			roomData.Teardown();
			_unuseRoomList.Add(roomData);
		}
		_roomList.Clear();
	}

}

