using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageScene : MonoBehaviour
{
    public SliderController SetX;
    public SliderController SetY;
    public SliderController SetLandMine;

    public int Min = 20;
    public int Max = 200;

    void Start()
    {
        SetX.SetValue(Min);
        SetX.SetMin(Min);
        SetX.SetMax(Max);

        SetY.SetValue(Min);
        SetY.SetMin(Min);
        SetY.SetMax(Max);

        UpdateLandMine();
    }

    public void UpdateLandMine()
    {
        SetLandMine.SetMin(MinLandMine());
        SetLandMine.SetMax(MaxLandMine());

        if (SetLandMine.GetValue() > MaxLandMine() || SetLandMine.GetValue() < MinLandMine())
        {
            SetLandMine.SetValue(MaxLandMine() + MinLandMine() / 2);
        }
    }

    public void GenerateRandom()
    {
        var rand = new System.Random((int)DateTime.Now.Ticks & 0x0000FFFF);
        var x = (int) rand.Next(Min, Max);
        var y = (int) rand.Next(Min, Max);

        SetX.SetValue(x);
        SetY.SetValue(y);

        UpdateLandMine();
        SetLandMine.SetValue((int)rand.Next(MinLandMine(), MaxLandMine()));
    }

    public int MinLandMine()
    {
        var landMine = (int)(SetX.GetValue() * SetY.GetValue() * 0.75 * 0.3);
        landMine = (int)(landMine * 0.5f);
        return landMine;
    }

    public int MaxLandMine()
    {
        var landMine = (int)(SetX.GetValue() * SetY.GetValue() * 0.75 * 0.3);
        landMine = (int)(landMine * 1.5f);
        return landMine;
    }

    public void ChangeScene()
    {
        GameManager.Instance.SetStageInfo((int)SetX.GetValue(), (int)SetY.GetValue(), (int)SetLandMine.GetValue());
        SceneManager.LoadScene("InGame");
    }

    public void Update()
    {
        UpdateLandMine();
    }
}
