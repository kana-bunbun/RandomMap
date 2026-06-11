using UnityEngine;
using System.Collections.Generic;
// ランダムマップ生成クラス
public class MapCreater
{
    private static MapCreater _instance = null;

    public static MapCreater instance
    {
        get
        {
            if (_instance == null) _instance = new MapCreater();
            return _instance;
        }
    }

    public MapCreater() { }


    /// <summary>
    /// マップ生成に用いるエリア情報
    /// </summary>
    private class AreaData
    {
        // 始点
        public int startX = -1, startY = -1;
        // サイズ
        public int width = -1, height = -1;

        public AreaData(int x, int y, int w, int h)
        {
            startX = x;
            startY = y;
            width = w;
            height = h;
        }

    }

    // エリアのリスト
    private List<AreaData> _areaList = null;
    // 分割線のマスのリスト
    private List<int> _divideLineList = null;
    // エリアの分割回数
    private const int _AREA_DIVIDE_COUNT = 8;
    // 部屋の最小サイズ
    private const int _MIN_ROOM_SIZE = 3;
    public void CreateMap()
    {
        // 最初のエリアを生成
        CreateFirstArea();
        // エリアを分割
        DivideAreaFixCount();
        // 部屋の配置
        // すべての部屋を作る
        CreateAllRoom();
        // 全部屋を連結
        ConnecctAllRoom();
        // 階段を作る

    }

    private void CreateFirstArea()
    {
        _areaList = new List<AreaData>();
        _divideLineList = new List<int>();
        // すべてのマスを壁にする
        MapSquareManager.instance.ExecuteAllSquare(SetFirstWall);

        AreaData firstArea = new AreaData(2, 2, GameConst.MAP_SQUARE_WIDTH_COUNT - 4, GameConst.MAP_SQUARE_HEIGHT_COUNT - 4);
        _areaList.Add(firstArea);
    }

    private void SetFirstWall(SquareObject square)
    {
        square.SetTerrain(eTerrain.Wall);
        // 最初の分割線マスを追加
        int x = square.squareData.posX;
        int y = square.squareData.posY;
        // 外周マスを除去
        if (x == 0 || x == GameConst.MAP_SQUARE_WIDTH_COUNT - 1 ||
            y == 0 || y == GameConst.MAP_SQUARE_HEIGHT_COUNT - 1) return;

        // 外周から1マス離れたマス以外の排除
        if (x != 1 && x != GameConst.MAP_SQUARE_WIDTH_COUNT - 2 &&
            y != 1 && y != GameConst.MAP_SQUARE_HEIGHT_COUNT - 2) return;
        // 分割線の追加
        AddDivideLine(square);
    }
    private void AddDivideLine(SquareObject square)
    {
        _divideLineList.Add(square.squareData.ID);
        square.SetTerrain(eTerrain.Wall);
    }
    /// <summary>
    /// エリアの分割
    /// </summary>
    private void DivideAreaFixCount()
    {
        for (int i = 0; i < _AREA_DIVIDE_COUNT; i++)
        {
            // エリアの分割
            // 幅が最大のエリアを取得
            AreaData divideArea = GetMaxSizeArea();
            bool isVertical = (divideArea.width < divideArea.height);
            // 取得するエリアを調べる
            int maxSize;
            // 三項演算子: 条件 ? trueの際の処理 : falseの時の処理
            maxSize = (isVertical) ? divideArea.height : divideArea.width;
            // 取得したエリアが分割可能か調べる
            if (maxSize < (_MIN_ROOM_SIZE + 2) * 2 + 1) break;

            // 取得したエリアを分割
            DivideArea(divideArea, isVertical);
        }
    }

    private void DivideArea(AreaData divideArea, bool isVertical)
    {
        if (isVertical)
        {
            // 上下に分割
            DivideAreaVertical(divideArea);
        }
        else
        {
            // 左右に分割
            DivideAreaHolizontal(divideArea);
        }

    }

    /// <summary>
    /// エリアを上下に分割
    /// </summary>
    /// <param name="divideArea"></param>
    private void DivideAreaVertical(AreaData divideArea)
    {
        // 分割位置の決定
        int randomMax = divideArea.height - (_MIN_ROOM_SIZE + 2) * 2;
        int dividePos = Random.Range(0, randomMax);
        dividePos += _MIN_ROOM_SIZE + 2 + divideArea.startY;
        // 新しいエリアを生成
        int newAreaHeight = divideArea.startY + divideArea.height - dividePos - 1;
        int newAreaY = dividePos + 1;
        AreaData newArea = new AreaData(divideArea.startX, newAreaY, divideArea.width, newAreaHeight);
        _areaList.Add(newArea);
        // 既存エリアを修正
        divideArea.height = dividePos - divideArea.startY;
        // 分割線マスの追加
        for (int x = 0; x < divideArea.width; x++)
        {
            SquareObject square = MapSquareManager.instance.GetSquare(divideArea.startX + x, dividePos);
            AddDivideLine(square);
        }
    }

    /// <summary>
    /// エリアを左右に分割
    /// </summary>
    /// <param name="divideArea"></param>
    private void DivideAreaHolizontal(AreaData divideArea)
    {
        // 分割位置の決定
        int randomMax = divideArea.width - (_MIN_ROOM_SIZE + 2) * 2;
        int dividePos = Random.Range(0, randomMax);
        dividePos += _MIN_ROOM_SIZE + 2 + divideArea.startX;
        // 新しいエリアを生成
        int newAreaWidth = divideArea.startX + divideArea.width - dividePos - 1;
        int newAreaX = dividePos + 1;
        AreaData newArea = new AreaData(newAreaX, divideArea.startY, newAreaWidth, divideArea.height);
        _areaList.Add(newArea);
        // 既存エリアを修正
        divideArea.width = dividePos - divideArea.startX;
        // 分割線マスの追加
        for (int y = 0; y < divideArea.height; y++)
        {
            SquareObject square = MapSquareManager.instance.GetSquare(dividePos, divideArea.startY + y);
            AddDivideLine(square);
        }
    }

    private AreaData GetMaxSizeArea()
    {
        int maxSize = -1;
        AreaData result = null;

        for (int i = 0; i < _areaList.Count; i++)
        {

            AreaData area = _areaList[i];

            // 横幅の確認
            if (area.width > maxSize)
            {
                maxSize = area.width;
                result = area;
            }
            // 縦幅の確認
            if (area.height > maxSize)
            {
                maxSize = area.height;
                result = area;
            }

        }
        return result;
    }
    private void CreateAllRoom()
    {
        for (int i = 0; i < _areaList.Count; i++)
        {
            CreateRoom(_areaList[i]);
        }
    }
    private void CreateRoom(AreaData area)
    {
        if (area == null) return;

        // 部屋のサイズ決定
        int roomWidth = Random.Range(_MIN_ROOM_SIZE, area.width - 2 + 1);
        int roomHeight = Random.Range(_MIN_ROOM_SIZE, area.height - 2 + 1);
        // 部屋の位置決定
        int xRandomRange = area.width - roomWidth;
        int yRandomRange = area.height - roomHeight;
        int startX = area.startX + Random.Range(1, xRandomRange);
        int startY = area.startY + Random.Range(1, yRandomRange);
        // 部屋の生成
        List<int> roomidList = new List<int>(roomWidth * roomHeight);
        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                SquareObject square = MapSquareManager.instance.GetSquare(startX + x, startY + y);
                if (square == null) continue;
                // マスを部屋地形に変更
                square.SetTerrain(eTerrain.Room);
                roomidList.Add(square.squareData.ID);
            }
        }
        MapSquareManager.instance.AddRoom(roomidList);
    }

    /// <summary>
    /// すべての部屋をつなげる
    /// </summary>
    private void ConnecctAllRoom()
    {
        // 掘削方向の決定
        eDirectionFour digDirection = (eDirectionFour)Random.Range(0, (int)eDirectionFour.Max);
        for (int i = 0; i < _areaList.Count - 1; i++)
        {
            // エリアから分割線まで掘る
            AreaData area1 = _areaList[i];
            int startID = DigToDevideLine(area1, digDirection);

            // 次のエリアを分割線まで掘る
            digDirection = (eDirectionFour)Random.Range(0, (int)eDirectionFour.Max);
            AreaData area2 = _areaList[i + 1];
            int goalID = DigToDevideLine(area2, digDirection);

            // 分割線内で通路をつなげる
            ConnectInDevideLine(startID, goalID);

            // 掘削方向を決定
            int digIndex = (int)digDirection + Random.Range(1, (int)eDirectionFour.Max);
            if (digIndex >= (int)eDirectionFour.Max) digIndex -= (int)eDirectionFour.Max;
            digDirection = (eDirectionFour)digIndex;
        }
    }

    /// <summary>
    /// 指定エリアから指定方向に分割線まで掘る
    /// 掘削方向の後方に部屋マスがあれば掘り進めることが可能
    /// </summary>
    /// <param name="area"></param>
    /// <param name="direction"></param>
    private int DigToDevideLine(AreaData area, eDirectionFour direction)
    {
        // 掘削開始マスを決定
        // 掘削方向の逆方向を取得
        eDirectionFour reverse = direction.ReverseDerection();

        List<SquareObject> targetList = new List<SquareObject>();

        // areaへのアクセスの回数を減らすためキャッシュ
        int startX = area.startX;
        int startY = area.startY;
        // エリアのすべてのマスから壁かつ、掘削方向と逆方向の隣接マスが部屋マスのマスを集約
        for (int y = 0; y < area.height; y++)
        {
            for (int x = 0; x < area.width; x++)
            {
                int squareX = startX + x;
                int squareY = startY + y;
                SquareObject square = MapSquareManager.instance.GetSquare(squareX, squareY);
                // 壁でなければ処理しない
                if (square == null || square.squareData.terrain != eTerrain.Wall) continue;
                // 掘削方向の逆のエリアを取得
                // 走査
                SquareObject dirSquare = MapSquareManager.instance.GetToDirSquare(squareX, squareY, reverse);
                if (dirSquare == null || dirSquare.squareData.terrain != eTerrain.Room) continue;
                targetList.Add(square);
            }

        }
        // 要素がなければreturn
        if (CommonModule.IsEmpty(targetList))
        {
            Debug.Log(startX+","+ startY + "/"+area.width+","+area.height);
            return -1;
        }
        // ↑からランダムに1マス抽選
        SquareObject currentSquare = targetList[Random.Range(0, targetList.Count)];

        // 分割線までの掘削
        while (true)
        {
            // currentSquareを通路マスにする
            currentSquare.SetTerrain(eTerrain.Passage);
            // 分割線マスなら終了
            if (_divideLineList.Exists(squareID => squareID == currentSquare.squareData.ID)) break;
            // currentSquareを掘削方向の隣接マスにする
            currentSquare = MapSquareManager.instance.GetToDirSquare(currentSquare.squareData.posX, currentSquare.squareData.posY, direction);

        }
        return currentSquare.squareData.ID;
    }
    // スタートからゴールまで分割線内を掘る
    private void ConnectInDevideLine(int startID, int goalid)
    {
        // 分割線内の経路探索
        List<ManhattanMoveData> route = RouteSearcher.instance.RouteSearchManhattan(startID, goalid, IsDivideLine);
        if (CommonModule.IsEmpty(route)) return;
        // 経路のマスをすべて通路にする
        for (int i = 0; i < route.Count; i++)
        {
            ManhattanMoveData moveData = route[i];
            SquareObject square = MapSquareManager.instance.GetSquare(moveData.targetSquareID);
            if (square == null) continue;
            square.SetTerrain(eTerrain.Passage);
        }
    }
    private bool IsDivideLine(MapSquare square)
    {
        return _divideLineList.Exists(squareID => squareID == square.ID);
    }
}
