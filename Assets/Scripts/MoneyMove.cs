using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyMove : MonoBehaviour
{


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //----------------게임오브젝트찾기------------>해당컴포넌트--------------->변수
        Vector2 target =  GameObject.Find("GameManager").GetComponent<GameManager>().targetPoint;
        long    price  =  GameObject.Find("GameManager").GetComponent<GameManager>().price;

        transform.position = Vector2.MoveTowards(transform.position, target, 0.02f);

        //색 변경
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,
            GetComponent<SpriteRenderer>().color.a - 0.02f);
        
        //차일드 오브젝트인 텍스트 컴포넌트의 색 변경
        GetComponentInChildren<Text>().color = new Color(0, 0, 0,
            GetComponentInChildren<Text>().color.a - 0.02f);

        //단가랑 텍스트 연동
        GetComponentInChildren<Text>().text = "+" + price.ToString("###,###");

        //특정한 자식 오브젝트 찾기
        //GameObject.Find("오브젝트").transform.Find("차일드오브젝트").GetComponent<컴포넌트>()
    }
}
