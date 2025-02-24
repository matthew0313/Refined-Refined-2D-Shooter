using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods
{
    private static Dictionary<GameObject, List<GameObject>> pools;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Reset()
    {
        pools = new();
    }
    public static GameObject PoolCreate(this GameObject obj)
    {
        if (!pools.ContainsKey(obj))
        {
            pools.Add(obj, new List<GameObject>());
        }
        GameObject returning = null;
        pools[obj].RemoveAll(tmp => tmp == null);
        if (pools[obj].Count > 0)
        {
            returning = pools[obj][0];
            returning.SetActive(true);
            pools[obj].RemoveAt(0);
        }
        else
        {
            returning = MonoBehaviour.Instantiate(obj);
            returning.GetComponent<PooledPrefab>().SetOrigin(obj);
        }
        return returning;
    }
    public static GameObject PoolCreate(this GameObject obj, Vector2 position, Quaternion rotation)
    {
        GameObject returning = obj.PoolCreate();
        returning.transform.position = position;
        returning.transform.rotation = rotation;
        returning.SetActive(true);
        return returning;
    }
    public static GameObject PoolCreate(this GameObject obj, Vector2 position, Quaternion rotation, Transform parent)
    {
        GameObject returning = obj.PoolCreate(position, rotation);
        returning.transform.parent = parent;
        return returning;
    }
    public static void PoolDestroy(this GameObject obj)
    {
        GameObject origin = obj.GetComponent<PooledPrefab>().origin;
        if (!pools.ContainsKey(origin))
        {
            Debug.LogError(origin.name + " was not created via PoolCreate()");
            return;
        }
        obj.SetActive(false);
        pools[origin].Add(obj);
    }
    private static Dictionary<GameObject, GameObject> examples = new Dictionary<GameObject, GameObject>();
    public static GameObject GetPrefabExample(this GameObject obj)
    {
        if (!examples.ContainsKey(obj))
        {
            examples.Add(obj, MonoBehaviour.Instantiate(obj));
            examples[obj].SetActive(false);
        }
        Debug.Log("EEE");
        return examples[obj];
    }
}
