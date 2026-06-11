using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

/// <summary>
/// キャラクター管理
/// </summary>
public class CharacterManager:MonoBehaviour
{
    public static CharacterManager _instance = null;
    // 使用中キャラリスト
    private List<CharacterObject> _useList = null;
    // 未使用キャラリスト
    private List<CharacterObject> _unusePlayerList = null;
    private List<CharacterObject> _unuseEnemyList = null;

    // キャラクタープレハブへの参照
    [SerializeField]
    private CharacterObject _characterOrigin = null;

    /// <summary>
    /// キャラクターオブジェクトを事前に生成して確保しておく
    /// </summary>
    public void Initialize()
    {
        _instance = this;

        _useList = new List<CharacterObject>(GameConst.ENEMY_MAX_COUNT + GameConst.PLAYER_MAX_COUNT);
        // プレイヤーオブジェクトを必要数生成して未使用状態にする
        // 生成
        _unusePlayerList = new List<CharacterObject>(GameConst.PLAYER_MAX_COUNT);
        // キャラクターの初期化
        CharacterObject Player = Instantiate(_characterOrigin, transform);
        // 配列追加
        Player.Initialize(new PlayerCharater());

        // エネミーオブジェクトを必要数生成して未使用状態にしておく
        _unuseEnemyList = new List<CharacterObject>(GameConst.ENEMY_MAX_COUNT);
        for (int i = 0; i < GameConst.ENEMY_MAX_COUNT; i++)
        {
            // 生成
            CharacterObject enemy = Instantiate(_characterOrigin, transform);
            // キャラクターの初期化
            enemy.Initialize(new EnemyCharacter());
            // 配列追加
            _unuseEnemyList.Add(enemy);
        }
    }

    public void CreatePlayer(int squareID ,int masterID)
    {
        // 未使用オブジェクトから使用可能なプレイヤーオブジェクトを取得
        CharacterObject player;
        if (CommonModule.IsEmpty(_unusePlayerList))
        {
            // 生成して使う
            player = Instantiate(_characterOrigin, transform);
            player.Initialize(new PlayerCharater());
        }
        else
        {
            // 未使用リストから使う
            player = _unusePlayerList[0];
            _unusePlayerList.RemoveAt(0);
        }
        // 使用可能なIDを取得して使用リストに追加
        int useID = -1;

        for (int i = 0; i < _useList.Count; i++)
        {
            if (_useList[i] != null) continue;

            useID = i;
            _useList[i] = player;
            // 一か所見つかったらループを抜ける
            break;
        }
        if (useID < 0)
        {
            useID = _useList.Count;
            _useList.Add(player);
        }

        // セットアップ
        player.SetUp(useID);
        // 指定マスに置く
        player.SetSquare(MapSquareManager.instance.GetSquare(squareID));
    }


}
