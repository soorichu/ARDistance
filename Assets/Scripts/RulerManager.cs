using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class RulerManager : MonoBehaviour
{
    public ARRaycastManager m_RaycastManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    public Vector2 _centerVec;

    public Transform _camPivot;
    public Transform _pivot;
    public Transform _rulerPool;

    public GameObject _rulerObj;
    private RulerObjectST _nowRulerObj;
    private List<RulerObjectST> _rulerObjList = new List<RulerObjectST>
();
    private bool _rulerEnable;
    private Vector3 _rulerPosSave;
    void Start()
    {
        _centerVec = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void Update()
    {
        s_Hits.Clear();
        if(m_RaycastManager.Raycast(_centerVec, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            _rulerEnable = true;
            _rulerPosSave = hitPose.position;
            _pivot.rotation = Quaternion.Lerp(_pivot.rotation, hitPose.rotation, 0.2f);

            if(_nowRulerObj != null)
            {
                _nowRulerObj.SetObj(hitPose.position);
            }
        }
        else
        {
            _rulerEnable = false;
            Quaternion tRot = Quaternion.Euler(90f, 0f, 0f);
            _pivot.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _pivot.rotation = Quaternion.Lerp(_pivot.rotation, tRot, 0.5f);
            _pivot.localPosition = Vector3.Lerp(_pivot.localPosition, Vector3.zero, 0.5f);

        }
    }

    public void MakeRulerObj()
    {
        if (_rulerEnable)
        {
            if(_nowRulerObj == null)
            {
                Debug.Log(_rulerObj);
                GameObject tObj = Instantiate(_rulerObj) as GameObject;
                tObj.transform.SetParent(_rulerPool);
                tObj.transform.position = Vector3.zero;
                tObj.transform.localScale = new Vector3(1, 1, 1);

                RulerObjectST tObjST = tObj.GetComponent<RulerObjectST>();
                tObjST._mainCam = _camPivot;
                tObjST.SetInit(_rulerPosSave);
                _rulerObjList.Add(tObjST);
                _nowRulerObj = tObjST;
            }
            else
            {
                _nowRulerObj = null;
            }
        }
    }
}
