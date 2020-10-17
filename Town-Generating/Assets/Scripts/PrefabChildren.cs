using System.Collections.Generic;
using UnityEngine;

public static class PrefabChildren
{
    private static void ProcessChild<T>(Transform aObj, ref List<T> aList) where T : Component
    {
        T c = aObj.GetComponent<T>();
        if (c != null)
            aList.Add(c);
        foreach (Transform child in aObj)
            ProcessChild<T>(child, ref aList);
    }

    public static T[] GetAllChilds<T>(Transform aObj) where T : Component
    {
        List<T> result = new List<T>();
        ProcessChild<T>(aObj, ref result);
        return result.ToArray();
    }
}
