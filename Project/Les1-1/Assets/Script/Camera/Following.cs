using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Following : MonoBehaviour
{
    public Transform target = null;

    private Camera CurCamera = null;

    public float fDistance = 10.0f;

    public float fAngle = 45.0f;

    private Vector3 ForWard = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        CurCamera = GetComponent<Camera>();
        if(null != target && CurCamera != null)
        {
            CurCamera.transform.LookAt(target, Vector3.up);
            ForWard = target.forward.normalized;
            float value = Mathf.Cos(Mathf.Deg2Rad * fAngle);
            
        }
       
    }

    private void LateUpdate()
    {
        UpdateCameraPosition(target, CurCamera.transform);
    }

    #region FUN
    private void UpdateCameraPosition(Transform Target,Transform Cam)
    {
        
    }
    #endregion

}
