using Niantic.Lightship.AR.LocationAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacements : MonoBehaviour
{
    // Start is called before the first frame update
    private ARLocationManager arLocationManager;
    private ARRaycastManager arRaycastManager;
    [SerializeField]private Button button1; // fire
    [SerializeField]private Button button2; // water
    [SerializeField] private List<GameObject> instantiatedObject;
    private List<GameObject> instantiatedObjects = new();
    private Camera mainCam;
    private GameObject target;

    public static  Action selectOb;

    private void Start()
    {
        mainCam = FindObjectOfType<Camera>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        arLocationManager = FindObjectOfType<ARLocationManager>();
        if(button1 != null)
        {
            button1.onClick.AddListener(setObject1);
        }
        if(button2 != null)
        {
            button2.onClick.AddListener(setObject2);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI Hit was recognized");
                return;
            }
            TouchToRay(Input.mousePosition);
        }
#endif
#if UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0 && Input.touchCount < 2 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = touch.position;

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                // We hit a UI element
                Debug.Log("We hit an UI Element");
                return;
            }

            Debug.Log("Touch detected, fingerId: " + touch.fingerId);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                Debug.Log("Is Pointer Over GOJ, No placement ");
                return;
            }
            TouchToRay(touch.position);
        }
#endif
    }
    void setObject1()
    {
        target = instantiatedObject[0];
        Debug.Log("Fire object selected");
    }

    void setObject2()
    {
        target = instantiatedObject[1];
        Debug.Log("Water object selected");
    }
    void TouchToRay(Vector3 touch)
    {
        Ray ray = mainCam.ScreenPointToRay(touch);
        List<ARRaycastHit> hits = new();

        arRaycastManager.raycastPrefab = target;
        arRaycastManager.Raycast(ray, hits, TrackableType.PlaneEstimated);

        Debug.Log("ShootingRay");
        //
        if (hits.Count > 0)
        {
            Vector3 hitPosition = hits[0].pose.position;

            if (!IsPositionOccupied(hitPosition))
            {
                GameObject placeObject = Instantiate(target);
                placeObject.transform.position = hits[0].pose.position;
                placeObject.transform.rotation = hits[0].pose.rotation;
                instantiatedObjects.Add(placeObject);
            }
            else
            {
                Debug.Log("overlapping detected");
            }
        }
    }
    bool IsPositionOccupied(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f);
        foreach (var collider in colliders)
        {
            if (instantiatedObjects.Contains(collider.gameObject))
            {
                return true;
            }
        }
        return false;
    }
}