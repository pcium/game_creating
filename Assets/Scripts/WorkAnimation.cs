using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkAnimation : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()
                == false)   //UI 위에 마우스가 있지 않을 때
            {
                GetComponent<Animator>().SetTrigger("Click");
            }
            
        }
	}
}
