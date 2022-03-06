using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Ads : MonoBehaviour
{
    string id;

    // Use this for initialization
    void Start()
    {
        id = "2915227";

        //Advertisement.Initialize(id);
    }

    public void ShowRewardedVideo()
    {
        Advertisement.Initialize(id);

        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;      //광고 재생이 끝나고 실행될 함수

        Advertisement.Show("rewardedVideo", options);   //광고 재생
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            //광고 재생이 스킵 없이 완료되었을 때 (보상 지급)

            GetComponent<GameManager>().money += 100000;
        }
        else if (result == ShowResult.Skipped)
        {
            //스킵되었을 때

        }
        else if (result == ShowResult.Failed)
        {
            //네트워크 등의 이유로 광고 재생에 실패했을 때
        }
    }
}