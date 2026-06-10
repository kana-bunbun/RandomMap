using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// マップの1マスの見た目情報
/// </summary>
public class SquareObject : MonoBehaviour
{
    // 座標計算用の係数
    static readonly float _SQUARE_SIZE_RAITO = 0.32f;
    [SerializeField]
    private SpriteRenderer _terrainSprite = null;
    public MapSquare squareData = null;


    public void SetUp(int Id,int posX,int posY)
    {
        squareData = new MapSquare(Id, posX, posY);

        Vector3 position = Vector3.zero;
        position.x = posX * _SQUARE_SIZE_RAITO;
        position.y = posY * _SQUARE_SIZE_RAITO;
        position.z = posY * 0.1f;
        transform.position = position;

    }
    [SerializeField] eTerrain terrain;

    public void SetTerrain(eTerrain terrain,int index=-1)
    {
        // データ上の見た目変更
        //if(squareData!=null)
        //squareData.SetTerrain(terrain);
        // ↓に書き方のほうが短く済む
        squareData?.SetTerrain(terrain);
        this.terrain = terrain;

        // スプライトの読み込み
        //Sprite sprits = TerrainSpriteAssignor.instance.GetTerrainSprite(terrain);

        _terrainSprite.sprite = TerrainSpriteAssignor.instance.GetTerrainSprite(terrain, index);
    }
}
