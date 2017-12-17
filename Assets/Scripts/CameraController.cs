using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    // inspector
    public float m_panSpeed = 15f;

    //
    private Vector3 m_oldPos = Vector3.zero;
    private Vector3 m_panOrigin = Vector3.zero;

    public CameraController()
    {
        _instance = this;
    }

    public void Update()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_oldPos = transform.position;
            m_panOrigin =  Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - m_panOrigin;
            transform.position = m_oldPos + -1 * pos * m_panSpeed;
        }
    }
}
