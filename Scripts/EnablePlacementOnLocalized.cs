using Niantic.Lightship.AR.LocationAR;
using Niantic.Lightship.AR.PersistentAnchors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EnablePlacementOnLocalized : MonoBehaviour
{
    private ARLocationManager _arLocationManager;
    private ARPlaneManager _arPlaneManagaer;
    private ARPlacements _arPlacements;
  
    void Start()
    {
        _arLocationManager = FindObjectOfType<ARLocationManager>();
        _arPlaneManagaer = FindObjectOfType<ARPlaneManager>();
        _arPlacements = FindObjectOfType<ARPlacements>();

        _arPlaneManagaer.enabled = false;
        _arPlacements.enabled = false;

        _arLocationManager.locationTrackingStateChanged += OnLocalized;
    }
    private void OnLocalized(ARLocationTrackedEventArgs eventArgs)
    {
        if(eventArgs.Tracking)
        {
            _arPlaneManagaer.enabled = true;
            _arPlacements.enabled = true;
        } else
        {
            _arPlaneManagaer.enabled = false;
            _arPlacements.enabled = false;
        }

    }
    // Update is called once per frame

}
