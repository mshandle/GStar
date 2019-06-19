using UnityEngine;

public class AwakeWithName : MonoBehaviour
{
    //Start is called before the first frame update
    [SerializeField]
    public string name = string.Empty;

    private void Awake()
    {
        Debug.LogFormat("Awake With:{0}", name);
    }
    
}
