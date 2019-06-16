
using UnityEngine;

public class AwakeTest : MonoBehaviour
{
    static int AwakeCount = 0;
    private int ID = 0;
    private void Awake()
    {
        ID = AwakeCount;
        Debug.LogFormat("Awake Index:{0}", AwakeCount++);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogFormat("Update ID：{0}", ID);
    }
}
