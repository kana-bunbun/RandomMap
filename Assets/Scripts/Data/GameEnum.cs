// 列挙体定義


/// <summary>
/// ゲームのパート
/// </summary>
public enum eGamePart
{
    Invalid = -1,   // 使われない想定の値
    Title,
    Main,
    Ending,

}

/// <summary>
/// マスの地形属性
/// </summary>
public enum eTerrain { 
    Invalid = -1,   // 使われない想定の値
    Passage,        // 通路
    Room,           // 部屋
    Wall,           // 壁
    Stair,          // 階段
}

public enum eDirectionFour
{
    Invalid = -1,   // 使われない想定の値
    Up,
    Right,
    Down,
    Left,
    Max
}