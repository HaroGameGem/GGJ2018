using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class CityZoomer : StaticComponent<CityZoomer> {

    [SerializeField] float zoomDuration;
    [SerializeField] float zoomEndValue;

    private float originZoomSize;

    private void Awake()
    {
        originZoomSize = Camera.main.orthographicSize;
    }

    public static void ZoomIn(CityUI city)
    {
        var camera = Camera.main;
        
        // Zoom Size
        camera.DOOrthoSize(instance.zoomEndValue, instance.zoomDuration);

        // Zoom Position
        Vector3 dst = city.transform.position;
            dst.z = camera.transform.position.z;
        camera.transform.DOMove(dst, instance.zoomDuration);
    }

    public static void ZoomOut()
    {
        var camera = Camera.main;

        // Zoom Size
        camera.DOOrthoSize(instance.originZoomSize, instance.zoomDuration);

        // Zoom Position
        Vector3 dst = Vector2.zero;
            dst.z = camera.transform.position.z;

        Camera.main.transform.DOMove(dst, instance.zoomDuration);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ZoomOut();
    }
}
