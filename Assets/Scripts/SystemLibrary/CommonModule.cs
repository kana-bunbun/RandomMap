using System;
using System.Collections.Generic;
using UnityEngine;

public class CommonModule
{
    /// <summary>
    /// 配列が空か否か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(T[] array)
    {
        if (array == null) return true;
        return array.Length < 1;
    }

    /// <summary>
    /// リストが空か否か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(List<T> list)
    {
        if (list == null) return true;
        return list.Count < 1;
    }
    /// <summary>
    /// 配列に対して有効な数字か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(T[] array,int index)
    {
        if(IsEmpty(array))return false;
        return (index >= 0) && (index < array.Length);
    }


    /// <summary>
    /// リストに対して有効な数字か判定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static bool IsEnableIndex<T>(List<T> list, int index)
    {
        if (IsEmpty(list)) return false;
        return (index >= 0) && (list.Count > index);
    }

}
