using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Drawing;
/// <summary>
/// 地形スプライト割り当てモジュール
/// </summary>
public class TerrainSpriteAssignor 
{
    // シングルトンインスタンス
    private static TerrainSpriteAssignor _instance = null;

    /// <summary>
    /// シングルトンなのでコンストラクタをpivateに
    /// クラス外から new (インスタンス化)できなくする
    /// </summary>
    public static TerrainSpriteAssignor instance
    {
        get{
            if (_instance == null) _instance = new TerrainSpriteAssignor();
            return _instance;
        }
    }
    // スプライトリソースフォルダへのパス
    private const string _MAP_SPRITE_PATH = "Design/Sprites/Map/";
    private readonly string[] _MAP_SPRITE_NAME_LIST = new string[] {
        "rogue_map_sand_floor",
        "rogue_map_sand_wall",
        "rogue_map_sand_stair",
    };

    private List<Sprite[]> _terrainSpriteList;
    private TerrainSpriteAssignor()
    {
        int spriteCount= _MAP_SPRITE_NAME_LIST.Length;
        _terrainSpriteList=new List<Sprite[]>(spriteCount);

        // すべての使用スプライトを読み込み
        for(int i = 0; i < spriteCount; i++)
        {
            Sprite[] terrainSprite = Resources.LoadAll<Sprite>(_MAP_SPRITE_PATH + _MAP_SPRITE_NAME_LIST[i]);
            // リストに追加
            _terrainSpriteList.Add(terrainSprite);
        }

    }
    public Sprite GetTerrainSprite(eTerrain terrain,int index=-1)
    {

        Sprite[] terrainSprite = _terrainSpriteList[GetTerrainSpriteIndex(terrain)];

        if (CommonModule.IsEmpty(terrainSprite)) return null;
        // 無効なインデックスを返す
        if (!CommonModule.IsEnableIndex(terrainSprite, index))
            index = Random.Range(0, terrainSprite.Length);

        return terrainSprite[index];
    }
    private int GetTerrainSpriteIndex(eTerrain terrain)
    {
        switch (terrain)
        {
            case eTerrain.Invalid:
                return 2;
                break;
            case eTerrain.Passage:
                return 0;
            case eTerrain.Room:
                return 0;
            case eTerrain.Wall:
                return 1;
            case eTerrain.Stair:
                break;
            default: break;
        }
        return 0;
    }
}