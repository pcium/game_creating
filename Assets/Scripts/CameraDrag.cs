using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Transform tr;

    private Vector2 firstTouch;
    private Vector2 currentTouch;

    public float limitMinY;
    public float limitMaxY;

	// Use this for initialization
	void Start ()
    {
        tr = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        if(Input.GetMouseButtonDown(0)) //마우스를 눌렀을 때 한 순간
        {
            firstTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        
        if(Input.GetMouseButton(0))     //마우스를 누르고 있을 때
        {
            currentTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if(Vector2.Distance(firstTouch, currentTouch) > 0.4f)   //터치의 뭉툭함 고려
        {
            if(firstTouch.y < currentTouch.y)
            {
                if(tr.position.y > limitMinY)      //카메라 화면이 최소제한값보다 클때만 움직이도록 함
                    tr.Translate(Vector2.down * 0.05f); //해당 방향으로 움직이는 함수
            }
            else if (firstTouch.y > currentTouch.y)
            {
                if (tr.position.y < limitMaxY)
                    tr.Translate(Vector2.up * 0.05f);
            }
        }
	}

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(transform.position.x, limitMinY), 0.2f);
        Gizmos.DrawSphere(new Vector2(transform.position.x, limitMaxY), 0.2f);
    }
    
    
}
