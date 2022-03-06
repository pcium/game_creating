using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWork : MonoBehaviour
{


	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GetMoney()); //함수 실행 (코루틴 전용)
	}
	
	IEnumerator GetMoney()
    {
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>(); //단축

        //1초마다 무한 반복하는 구조
        while (true)
        {
            gm.money += gm.personPrice; //직원의 단가만큼 소지금 증가

            yield return new WaitForSeconds(1); //1초 대기 
        }
    }
}
