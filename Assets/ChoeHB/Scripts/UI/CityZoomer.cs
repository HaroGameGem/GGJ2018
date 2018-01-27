using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using BitBenderGames;

public class CityZoomer : StaticComponent<CityZoomer> {

    [SerializeField] float zoomDuration;
    [SerializeField] float zoomEndValue;
    [SerializeField] Vector2 position;

    private float originZoomSize;

    private void Awake()
    {
        originZoomSize = Camera.main.orthographicSize;
    }

    
    public void Zoom(Vector2 dst)
    {
        var camera = Camera.main;

        // Zoom Size
        camera.DOOrthoSize(zoomEndValue, zoomDuration);

        // Zoom Position

        Vector3 dstZ = dst;
            dstZ.z = camera.transform.position.z;

        camera.transform.DOMove(dstZ, zoomDuration);
    }

    public void ZoomOut()
    {
        var camera = Camera.main;

        // Zoom Size
        camera.DOOrthoSize(originZoomSize, zoomDuration);

        // Zoom Position
        Vector3 dst = Vector3.zero;
            dst.z = -10;

        camera.transform.DOMove(dst, zoomDuration);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ZoomOut();
    }
}
