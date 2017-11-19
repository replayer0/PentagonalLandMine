using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
	void Start()
    {
        GameManager.Instance.Load();
	}
	
	public void ChangeScene()
    {
        SceneManager.LoadScene("InGame");
    }
}
