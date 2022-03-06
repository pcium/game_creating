using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public bool isEnable;

    public string sentence;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Timer());

        SimpleSave();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            //SceneManager.LoadScene("SampleScene");    //이름으로 씬 로드
            if (isEnable == true)
            {
                SceneManager.LoadScene(1);                  //번호로 씬 로드
            }

            /*      //다음 씬으로 넘어가기
            int nextSceneNumber = SceneManager.GetActiveScene().buildIndex + 1;

            SceneManager.LoadScene(nextSceneNumber);
            */
        }

        if (Input.GetKeyDown(KeyCode.S))
            SimpleSave();

        if (Input.GetKeyDown(KeyCode.L))
            SimpleLoad();
	}

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(2); //2초 동안 이 줄에서 대기
        isEnable = true;
    }

    void SimpleSave()
    {
        //Set == 저장
        //PlayerPrefs.SetInt("NUMBER", 5);
        //Get == 로드
        //int load = PlayerPrefs.GetInt("NUMBER", 0);

        PlayerPrefs.SetString("SENTENCE", sentence);

        //* long형을 저장하는 방법
        long money = 1231231231231233123;
        PlayerPrefs.SetString("LONG", money.ToString());
    }
    void SimpleLoad()
    {
        sentence = PlayerPrefs.GetString("SENTENCE", "");

        //* long형을 다시 로드하는 방법
        string moneyStr = PlayerPrefs.GetString("LONG");
        long money = long.Parse(moneyStr);

    }
}
