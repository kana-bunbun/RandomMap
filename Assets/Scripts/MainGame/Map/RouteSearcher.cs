using System.Collections.Generic;
using UnityEngine;

public class RouteSearcher
{
    private static RouteSearcher _instance = null;
    public static RouteSearcher instance
    {
        get
        {
            if (_instance == null) _instance = new RouteSearcher();
            return _instance;
        }
    }
    private RouteSearcher()
    {

    }

    // A*経路探索
    private abstract class DistanceNode
    {
        // 実コスト　スタートから何マス離れているか

        public int distance { get; private set; } = -1;
        // マスID
        public int squareID { get; private set; } = -1;
        public DistanceNode(int distance, int squareID)
        {
            this.distance = distance;
            this.squareID = squareID;
        }
        // スコア取得処理
        public abstract int GetScore(int goalX, int goalY);
    }

    // 四方向のA*経路探索
    private class DistanceNodeManhattan : DistanceNode
    {
        // 親ノードからの移動方向
        public eDirectionFour prevdir { get; private set; } = eDirectionFour.Invalid;
        public DistanceNodeManhattan rootNode { get; private set; } = null;
        public DistanceNodeManhattan(int distance, int squareID, eDirectionFour direction, DistanceNodeManhattan rootNode) : base(distance, squareID)
        {
            prevdir = direction;
            this.rootNode = rootNode;
        }
        public override int GetScore(int goalX, int goalY)
        {
            SquareObject square = MapSquareManager.instance.GetSquare(squareID);
            if (square == null || square.squareData == null) return int.MaxValue;
            // ゴールからの差
            int dx = Mathf.Abs(goalX - square.squareData.posX);
            int dy = Mathf.Abs(goalY - square.squareData.posY);
            return dx + dy + distance;
        }
    }
    // 四方向のA*経路探索ノードテーブル
    private class DistanceNodeManhattanTable
    {
        // ゴールノード
        public DistanceNodeManhattan goalNode = null;
        // オープン済みのノード
        public List<DistanceNodeManhattan> openList = null;
        // クローズ済みのノード
        public List<DistanceNodeManhattan> closeList = null;

        public DistanceNodeManhattanTable()
        {
            openList = new List<DistanceNodeManhattan>();
            closeList = new List<DistanceNodeManhattan>();
        }

        public void Clear()
        {
            goalNode = null;
            openList.Clear();
            closeList.Clear();
        }

    }

    // 四方向用のノードテーブル
    private DistanceNodeManhattanTable _manhattanTable = null;

    /// <summary>
    /// 四方向経路探索
    /// </summary>
    /// <param name="startSquareID">開始マスID</param>
    /// <param name="goalSquareID">終了マスID</param>
    /// <param name="CanPass">通行可否判定</param>
    /// <returns></returns>
    public List<ManhattanMoveData> RouteSearchManhattan(int startSquareID, int goalSquareID, System.Func<MapSquare, bool> CanPass)
    {
        // ゴールノードにたどり着くまでノードをオープンする
        OpenNodeToGoalManhattan(startSquareID, goalSquareID, CanPass);
        // ゴールノードからスタートまで親ノードをたどって経路を生成

        return CreateRouteManhattan();
    }

    /// <summary>
    /// スタートからゴールノードに辿り着くまでノードをオープンする
    /// </summary>
    private void OpenNodeToGoalManhattan(int startSquareID, int goalSquareID, System.Func<MapSquare, bool> CanPass)
    {
        // テーブル初期化
        if (_manhattanTable == null)
        {
            _manhattanTable = new DistanceNodeManhattanTable();
        }
        else
        {
            _manhattanTable.Clear();
        }
        // スタートノードを生成
        _manhattanTable.openList.Add(new DistanceNodeManhattan(0, startSquareID, eDirectionFour.Invalid, null));
        // ゴールマスの取得
        SquareObject goalSquare = MapSquareManager.instance.GetSquare(goalSquareID);
        if (goalSquare == null || goalSquare.squareData == null) return;
        // ゴール座標の取得
        int goalX = goalSquare.squareData.posX, goalY = goalSquare.squareData.posY;
        // ゴールが見つかったらnullでなくなるのでwhileを抜けられる
        while (_manhattanTable.goalNode == null)
        {
            // スコアの最小ノードを探す
            DistanceNodeManhattan minScoreNode = GetMinScoreNode(goalX, goalY);
            // スコア最小ノードが見つからなければ終わる
            if (minScoreNode == null) break;
            // 周りをオープン
            OpenNodeAroundManhattan(minScoreNode, goalSquareID, CanPass);
            // クローズする
            _manhattanTable.openList.Remove(minScoreNode);  // オープン済みリストから取り除き
            _manhattanTable.closeList.Add(minScoreNode);    // クローズのリストに入れる
        }
    }
    // 最少スコアのノードを返す
    private DistanceNodeManhattan GetMinScoreNode(int goalX, int goalY)
    {
        if (CommonModule.IsEmpty(_manhattanTable.openList)) return null;

        int minScore = -1;
        DistanceNodeManhattan result = null;
        List<DistanceNodeManhattan> openList = _manhattanTable.openList;
        for (int i = 0; i < openList.Count; i++)
        {
            DistanceNodeManhattan node = openList[i];
            if (node == null) continue;
            int nodeScore = node.GetScore(goalX, goalY);
            if (result != null && minScore <= nodeScore) continue;
            result = node;
            minScore = nodeScore;
        }

        return result;
    }

    // 指定のノードの周囲4マスをオープンする
    private void OpenNodeAroundManhattan(DistanceNodeManhattan baseNode, int goalSquareID, System.Func<MapSquare, bool> CanPass)
    {
        if (baseNode == null) return;
        // オープンするノードの実コスト決定
        int distance = baseNode.distance + 1;
        SquareObject square = MapSquareManager.instance.GetSquare(baseNode.squareID);
        int baseX = square.squareData.posX, baseY = square.squareData.posY;
        // 周囲4マスをオープンする
        for (int i = 0; i < (int)eDirectionFour.Max; i++)
        {
            eDirectionFour direction = (eDirectionFour)i;
            // 指定ノードからして―ゴールに隣接するマスを取得
            SquareObject openSquare = MapSquareManager.instance.GetToDirSquare(baseX, baseY, direction);
            if (openSquare == null) continue;
            // すでにクローズしたマスなら処理しない
            if (_manhattanTable.closeList.Exists(node => node.squareID == openSquare.squareData.ID)) continue;
            // オープン済みのマスなら処理しない
            if (_manhattanTable.openList.Exists(node => node.squareID == openSquare.squareData.ID)) continue;
            // 通行不可のマスなら処理しない
            if (!CanPass(openSquare.squareData)) continue;

            // ノードのオープン
            DistanceNodeManhattan addNode = new DistanceNodeManhattan(distance, openSquare.squareData.ID, direction, baseNode);
            _manhattanTable.openList.Add(addNode);
            // ゴール判定
            if (openSquare.squareData.ID != goalSquareID) continue;
            // ゴールをオープンしたら終わり
            _manhattanTable.goalNode = addNode;
            return;
        }
    }
    // 経路生成
    private List<ManhattanMoveData> CreateRouteManhattan()
    {
        // ゴールにたどり着く道筋があるかどうか判定
        if (_manhattanTable == null || _manhattanTable.goalNode == null) return null;

        // 経路用のリストを生成
        int routeCount = _manhattanTable.goalNode.distance;
        List<ManhattanMoveData> route = new List<ManhattanMoveData>();
        for (int i = 0; i < routeCount; i++)
        {
            route.Add(null);
        }
        // ゴールを遡って経路を生成
        DistanceNodeManhattan currentNode = _manhattanTable.goalNode;
        for (int i = routeCount - 1; i >= 0; i--)
        {
            ManhattanMoveData moveData = new ManhattanMoveData(currentNode.rootNode.squareID, currentNode.squareID, currentNode.prevdir);
            route[i] = moveData;
            // 親ノードを現在のノードにする
            currentNode = currentNode.rootNode;
        }
        return route;
    }
}
