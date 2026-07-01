using UnityEngine;

public class DontDestroy : MonoBehaviour
{

    private static GameObject[] persistentObjects = new GameObject[3];
    public int index;

    void Awake()
    {
        if (persistentObjects[index] == null)
        {
            persistentObjects[index] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[index] != gameObject)
        {
            Destroy(gameObject);
        }
    }


}
