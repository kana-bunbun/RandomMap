using UnityEngine;

// キャラクターの見た目情報
public class CharacterObject : MonoBehaviour
{
    // キャラクターの見た目スプライト
    [SerializeField]
    private SpriteRenderer _characterSprite = null;
    // キャラクターの情報
    public CharacterBase characterData { get; private set; } = null;

    // 初期化
    public void Initialize(CharacterBase character)
    {
        characterData = character;
        gameObject.SetActive(false);
    }
    // 使用前準備
    public void SetUp(int ID)
    {
        characterData.SetUp(ID);
        gameObject.SetActive(true);
    }

    // 使用後片付け
    public void TearDown()
    {
        characterData.Teardown();
        gameObject.SetActive(false);
    }

    public void SetSquare(SquareObject square)
    {
        // 見た目上の処理
        SetPosition(square.GetCharacterRoot().position);
        // 内部的な情報の処理
        characterData.SetSquare(square);
    }

    // 3D座標設定
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
