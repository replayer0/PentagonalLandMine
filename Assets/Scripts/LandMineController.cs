using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LandMineController : MonoBehaviour
{
    // inspector
    public SHAPE Shape = SHAPE.PENTAGON;
    public GameObject LandMineImage = null;
    public GameObject Cover = null;
    public GameObject LandMineCheckImage = null;
    public TextMesh Number = null;
    [SerializeField] private float m_width = 1f;
    [SerializeField] private float m_height = 1f;
    [SerializeField] private float m_reverseHeight = 1f;

    //
    public float Width { get { return m_width; } }
    public float Height { get { return m_height; } }
    public float ReverseHeight { get { return m_reverseHeight; } }
    public bool IsMine { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsReverse { get; private set; }
    private Vector3 m_mousePos = Vector3.zero;
    private float m_touchTime = 0;
    private bool m_isTouching = false;
    private bool m_isSuccessCheck = false;

    public LandMineController()
    {
        IsMine = false;
        IsReverse = false;
    }

    public void Initialize(bool isMine, int x, int y, bool isReverse)
    {
        // set mine
        LandMineImage.SetActive(isMine);
        LandMineCheckImage.SetActive(false);
        IsMine = isMine;

        // set pos
        X = x;
        Y = y;

        // set reverse
        IsReverse = isReverse;

        // random color
        var spriteRenderer = Cover.GetComponent<SpriteRenderer>();
        //spriteRenderer.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
    }

    public void Update()
    {
        if (false == IsCovered())
        {
            return;
        }

        if (true == m_isTouching && false == CameraController.Instance.IsDragging())
        {
            m_touchTime += Time.deltaTime;
            if (1.0f < m_touchTime && false == m_isSuccessCheck)
            {
                m_isSuccessCheck = true;
                if (true == LandMineCheckImage.activeInHierarchy)
                {
                    LandMineCheckImage.SetActive(false);
                }
                else
                {
                    LandMineCheckImage.SetActive(true);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (CameraExtension.IsPointerOverUIObject())
        {
            return;
        }

        m_mousePos = Input.mousePosition;
        m_isTouching = true;
        Debug.Log(m_mousePos);
    }

    private void OnMouseUp()
    {
        Debug.Log(m_mousePos + " " + Input.mousePosition);

        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (CameraExtension.IsPointerOverUIObject())
        {
            return;
        }

        if (15.0f > Vector3.Distance(Input.mousePosition, m_mousePos) && false == LandMineCheckImage.activeInHierarchy)
        {
            if (false == m_isSuccessCheck)
            {
                Uncover();
            }
        }

        m_touchTime = 0;
        m_isTouching = false;
        m_isSuccessCheck = false;
    }

    public void SetNumber(int num)
    {
        Number.text = num.ToString();

        Number.gameObject.SetActive(0 != num);
    }

    public int GetNumber()
    {
        return Int32.Parse(Number.text);
    }

    public void Uncover()
    {
        if (Cover.activeInHierarchy)
        {
            Cover.SetActive(false);

            if (IsMine)
            {
                InGameScene.Instance.Lose();
            }
            else
            {
                InGameScene.Instance.UncoverNear();
            }
        }
    }

    public bool IsCovered()
    {
        return Cover.activeInHierarchy;
    }
}
