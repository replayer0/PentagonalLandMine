using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public enum SHAPE
{
    PENTAGON = 0,
    HEXAGON,
}

public class InGameScene : Singleton<InGameScene>
{
    /// <summary>
    /// GameObject
    /// </summary>
    public LandMineController m_pentagon = null;
    public LandMineController m_hexagon = null;
    public GameObject m_parent = null;
    public GameObject WinPanel = null;
    public GameObject LosePanel = null;
    public GameObject PausePanel = null;

    public int m_countX = 10;
    public int m_countY = 10;
    public int m_maxMineCount = 20;

    private Dictionary<KeyValuePair<int, int>, LandMineController> m_pentagons = new Dictionary<KeyValuePair<int, int>, LandMineController>();
    private Dictionary<KeyValuePair<int, int>, LandMineController> m_hexagons = new Dictionary<KeyValuePair<int, int>, LandMineController>();

    public InGameScene()
    {
        _instance = this;
    }

    private void GenerateMine(out HashSet<KeyValuePair<int, int>> minePos)
    {
        minePos = new HashSet<KeyValuePair<int, int>>();

        Random.InitState((int)System.DateTime.Now.Ticks);

        while (m_maxMineCount > minePos.Count)
        {
            int y = Random.Range(0, m_countY);
            int x = 0;
            x = (0 == y % 2) ? Random.Range(0, m_countX / 2) : Random.Range(0, m_countX);

            if (!minePos.Contains(new KeyValuePair<int, int>(x, y)))
            {
                minePos.Add(new KeyValuePair<int, int>(x, y));
            }
        }

        Debug.Log(minePos.Count);
    }

    private void Start()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        PausePanel.SetActive(false);
        m_pentagons.Clear();
        m_hexagons.Clear();

        var stageInfo = GameManager.Instance.GetStageInfo();
        if (null != stageInfo)
        {
            m_countX = stageInfo.X;
            m_countY = stageInfo.Y;
            m_maxMineCount = stageInfo.LandMine;
        }

        bool isReverse = false;
        bool isIndent = true;

        HashSet<KeyValuePair<int, int>> minePos;
        GenerateMine(out minePos);

        for (int y = 0; y < m_countY; ++y)
        {
            SHAPE shape = (0 == y % 2) ? SHAPE.HEXAGON : SHAPE.PENTAGON;
            int CountX = (SHAPE.HEXAGON == shape) ? (int)((float)m_countX / 2f) : m_countX;
            bool reverse = isReverse;
            for (int x = 0; x < CountX; ++x)
            {
                Vector3 pos = Vector3.zero;
                LandMineController controller = null;

                switch (shape)
                {
                    case SHAPE.HEXAGON:
                        {
                            float posX = (true == isIndent) ? x * m_hexagon.Width : x * m_hexagon.Width + (m_hexagon.Width / 2f);
                            float posY = y * m_hexagon.Height - m_hexagon.ReverseHeight;

                            pos = new Vector3(posX, posY, 0f);
                            controller = GameObject.Instantiate<LandMineController>(m_hexagon, pos, Quaternion.identity, m_parent.transform);
                            m_hexagons.Add(new KeyValuePair<int, int>(x, y), controller);
                        }
                        break;

                    case SHAPE.PENTAGON:
                        {
                            float posX = x * m_pentagon.Width;
                            float posY = (true == reverse)
                                ? y * m_pentagon.Height - m_pentagon.ReverseHeight
                                : y * m_pentagon.Height;

                            pos = new Vector3(posX, posY, 0f);
                            controller = GameObject.Instantiate<LandMineController>(m_pentagon, pos, Quaternion.identity, m_parent.transform);
                            if (true == reverse)
                            {
                                controller.transform.Rotate(new Vector3(0, 0, 180f));
                            }
                            m_pentagons.Add(new KeyValuePair<int, int>(x, y), controller);
                        }
                        break;
                }

                var isMine = minePos.Contains(new KeyValuePair<int, int>(x, y));
                controller.Initialize(isMine, x, y, reverse);

                reverse = !reverse;
            }

            if (1 == y % 2)
            {
                isReverse = !isReverse;
                isIndent = !isIndent;
            }
        }

        foreach (var pair in m_hexagons)
        {
            var hex = pair.Value;
            if (false == hex.IsMine)
            {
                var near = GetNearLandMine(hex);
                int num = 0;

                foreach (var mine in near)
                {
                    if (true == mine.IsMine)
                    {
                        ++num;
                    }
                }

                hex.SetNumber(num);
            }
            else
            {
                hex.SetNumber(0);
            }
        }

        foreach (var pair in m_pentagons)
        {
            var penta = pair.Value;
            if (false == penta.IsMine)
            {
                var near = GetNearLandMine(penta);
                int num = 0;

                foreach (var mine in near)
                {
                    if (true == mine.IsMine)
                    {
                        ++num;
                    }
                }

                penta.SetNumber(num);
            }
            else
            {
                penta.SetNumber(0);
            }
        }
    }

    public List<LandMineController> GetNearLandMine(LandMineController controller)
    {
        var list = new List<LandMineController>();

        if (SHAPE.HEXAGON == controller.Shape)
        {
            int startX = (0 == controller.Y % 4) ? controller.X * 2 - 1 : controller.X * 2;
            for (int x = startX; x < startX + 3; ++x)
            {
                if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(x, controller.Y + 1)))
                {
                    list.Add(m_pentagons[new KeyValuePair<int, int>(x, controller.Y + 1)]);
                }

                if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(x, controller.Y - 1)))
                {
                    list.Add(m_pentagons[new KeyValuePair<int, int>(x, controller.Y - 1)]);
                }
            }
        }
        else
        {
            int myX = controller.X;
            int myY = controller.Y;
            if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(myX - 1, myY)))
            {
                list.Add(m_pentagons[new KeyValuePair<int, int>(myX - 1, myY)]);
            }

            if (m_pentagons.ContainsKey(new KeyValuePair<int, int>(myX + 1, myY)))
            {
                list.Add(m_pentagons[new KeyValuePair<int, int>(myX + 1, myY)]);
            }

            int hexStartX = (0 == myX % 2) ? (myX / 2) - 1 : myX / 2;
            if (true == controller.IsReverse)
            {
                if (m_hexagons.ContainsKey(new KeyValuePair<int, int>(myX / 2, myY + 1)))
                {
                    list.Add(m_hexagons[new KeyValuePair<int, int>(myX / 2, myY + 1)]);
                }

                for (int x = hexStartX; x < hexStartX + 2; ++x)
                {
                    if (m_hexagons.ContainsKey(new KeyValuePair<int, int>(x, myY - 1)))
                    {
                        list.Add(m_hexagons[new KeyValuePair<int, int>(x, myY - 1)]);
                    }
                }
            }
            else
            {
                if (m_hexagons.ContainsKey(new KeyValuePair<int, int>(myX / 2, myY - 1)))
                {
                    list.Add(m_hexagons[new KeyValuePair<int, int>(myX / 2, myY - 1)]);
                }

                for (int x = hexStartX; x < hexStartX + 2; ++x)
                {
                    if (m_hexagons.ContainsKey(new KeyValuePair<int, int>(x, myY + 1)))
                    {
                        list.Add(m_hexagons[new KeyValuePair<int, int>(x, myY + 1)]);
                    }
                }
            }
        }

        return list;
    }

    private void Update()
    {
        // uncover  hex tile
        foreach (var pair in m_hexagons)
        {
            var hex = pair.Value;
            if (false == hex.IsMine && false == hex.IsCovered() && 0 == hex.GetNumber())
            {
                var near = GetNearLandMine(hex);
                foreach (var nearController in near)
                {
                    nearController.Uncover();
                }
            }
        }

        // uncover  penta tile
        foreach (var pair in m_pentagons)
        {
            var penta = pair.Value;
            if (false == penta.IsMine && false == penta.IsCovered() && 0 == penta.GetNumber())
            {
                var near = GetNearLandMine(penta);
                foreach (var nearController in near)
                {
                    nearController.Uncover();
                }
            }
        }

        // check win
        int checkCount = 0;
        foreach (var pair in m_hexagons)
        {
            var hex = pair.Value;
            if (false == hex.IsMine && false == hex.IsCovered())
            {
                ++checkCount;
            }
        }

        foreach (var pair in m_pentagons)
        {
            var penta = pair.Value;
            if (false == penta.IsMine && false == penta.IsCovered())
            {
                ++checkCount;
            }
        }

        if (m_maxMineCount + checkCount == m_hexagons.Count + m_pentagons.Count)
        {
            Win();
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("Title");
    }

    public void Win()
    {
        WinPanel.SetActive(true);
    }

    public void Lose()
    {
        LosePanel.SetActive(true);
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
    }

    public void Restart()
    {
        Start();
        ShowAds();
    }

    public void ShowAds()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                // 보상지급
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
