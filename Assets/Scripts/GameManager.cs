using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    public long money;  //소지금
    public int person;  //직원 수
    public int priceLevel;  //단가 업그레이드 레벨
    public long price;      //클릭 당 단가
    public long priceCost;  //단가 업그레이드 가격

    public long personPrice; //직원이 버는 단가
    public long personCost; //직원 채용 가격

    public Text textMoney;  //소지금 표시 텍스트
    public Text textPerson; //직원 수 표시 텍스트
    public Text textUpgradePrice;   //단가 업그레이드 정보
    public Text textRecruit;    //직원 고용 업그레이드 정보

    public GameObject prefabMoneyGet;   //돈 상승 프리팹
    public GameObject prefabPerson;     //직원 프리팹
    public GameObject prefabFloor;      //바닥 프리팹

    public Vector2 targetPoint;         //돈 상승 프리팹의 목적지
    public Vector2[] createSpots;       //직원 생성 지점

    public int personOrigin = 1;        //세이브랑 직원 수가 다른 경우를 위함


    public AudioClip clip;  //음원을 드래그 앤 드롭할 경우 음원을 스크립트로 가져올 수 있음

    public int floorNumber;         //바닥 갯수
    public float floorSpacing = 12.47f; //바닥 스페이싱

	// Use this for initialization
	void Start ()
    {
        //세이브 파일이 있는지 검사
		if(XmlManager.IsExist(Application.persistentDataPath + "/save.xml"))
        {
            Load();
            LoadPerson();
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        ShowInfo(); //ShowInfo() 함수를 실행
        MoneyIncrease();
        CreateFloor();

        if (Input.GetKeyDown(KeyCode.S))
            Save();
        if (Input.GetKeyDown(KeyCode.Escape))   //안드로이드에서 뒤로가기에 해당
            Application.Quit();         //게임종료

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //세이브 파일 삭제
            System.IO.File.Delete(Application.persistentDataPath + "/save.xml");
        }
	}

    //텍스트로 정보를 표시하는 것을 담당
    void ShowInfo()
    {
        if (money == 0)
            textMoney.text = "0원";
        else
            textMoney.text = money.ToString("###,###") + "원";

        textPerson.text = person + "명";


        CheckUpgradePrice();
        CheckUpgradeRecruit();
    }

    //단가 업그레이드 텍스트 수정 및 버튼 활성화/비활성화
    void CheckUpgradePrice()
    {
        if (GameObject.Find("Panel_PriceUpgrade") != null)   //해당 오브젝트가 존재할 때
        {
            textUpgradePrice.text = "Lv." + priceLevel + "단가 상승\n" +
                                    "클릭 당 단가 :\n" +
                                    price.ToString("###,###") + " 원\n" +
                                    "업그레이드 가격 :\n" +
                                    priceCost.ToString("###,###") + " 원\n";

            if (money < priceCost)   //업그레이드 가격보다 작다면
            {
                GameObject.Find("Panel_PriceUpgrade").transform.Find("Button_Upgrade").
                    GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("Panel_PriceUpgrade").transform.Find("Button_Upgrade").
                    GetComponent<Button>().interactable = true;
            }

        }
    }
    //직원 고용 텍스트 수정 및 버튼 활성화/비활성화
    void CheckUpgradeRecruit()
    {
        if (GameObject.Find("Panel_Recruit") != null)   //해당 오브젝트가 존재할 때
        {
            if(personPrice == 0)
            {
                textRecruit.text = "Lv." + person + "직원 고용\n" +
                                    "초 당 단가 :\n" +
                                    "0 원\n" +
                                    "업그레이드 가격 :\n" +
                                    personCost.ToString("###,###") + " 원\n";
            }
            else
            {
                textRecruit.text = "Lv." + person + "직원 고용\n" +
                                    "초 당 단가 :\n" +
                                    personPrice.ToString("###,###") + " 원\n" +
                                    "업그레이드 가격 :\n" +
                                    personCost.ToString("###,###") + " 원\n";
            }
            

            if (money < personCost)   //업그레이드 가격보다 작다면
            {
                GameObject.Find("Panel_Recruit").transform.Find("Button_Upgrade").
                    GetComponent<Button>().interactable = false;
            }
            else
            {
                GameObject.Find("Panel_Recruit").transform.Find("Button_Upgrade").
                    GetComponent<Button>().interactable = true;
            }

        }
    }

    void MoneyIncrease()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() == false)
            //마우스가 UI 위에 있지 않을 때
            {
                money += price; //소지금을 현재 단가만큼 증가

                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Input.mousePosition -->>> 기기 상에서 입력된 좌표를 날 것으로 가져옴
                // 오른쪽 구석을 클릭한다고 가정했을 때

                //돈 상승 프리팹 오브젝트 생성
                GameObject obj;
                obj = Instantiate(prefabMoneyGet, mousePosition, Quaternion.identity);

                Destroy(obj, 5);    //오브젝트 파괴, ',' 뒤는 몇 초 뒤에 사라질지

                //소리 재생 (효과음 한 번만 실행,      (클립, 볼륨)
                //GameObject.Find("오브젝트이름").
                GetComponent<AudioSource>().PlayOneShot(clip, 1);
            }
            
        }

    }
    
    //업그레이드 패널 열기
    public void OpenPanel(GameObject obj)
    {
        obj.SetActive(true);
    }
    //업그레이드 패널 닫기
    public void ClosePanel(GameObject obj)
    {
        obj.SetActive(false);
    }

    //단가 업그레이드 진행
    public void UpgradePrice()
    {
        if(money >= priceCost)  //가진 돈이 업그레이드 가격보다 같거나 클 때
        {
            money -= priceCost; //소지금 차감

            //price += price / 10 * 12;
            price += 100 * priceLevel;            //단가 업그레이드 진행
            priceCost += 500 * priceLevel;    //업그레이드 가격 올림
            priceLevel += 1;                        //레벨 상승
        }
    }

    //직원 구매
    public void CheckRecruit()
    {
        if(money >= personCost)
        {
            money -= personCost;

            personPrice += 10 * person;
            personCost += 700 * person;

            Recruit();

            person += 1;
        }
    }

    //직원을 정렬 및 생성
    void Recruit()
    {
        int row = person % 3;

        Vector2 spot = new Vector2(createSpots[row].x ,
                                   createSpots[row].y - (person / 3) * 3.88f);

        GameObject obj = Instantiate(prefabPerson, spot, Quaternion.identity);
    }

    //바닥 생성
    void CreateFloor()
    {
        int capacity = person / 9 + 1;

        if(capacity > floorNumber)
        {
            Vector2 spot = GameObject.Find("Background").transform.position;

            spot += Vector2.down * floorSpacing * floorNumber;

            Instantiate(prefabFloor, spot, Quaternion.identity);

            Camera.main.GetComponent<CameraDrag>().limitMinY -= floorSpacing;

            floorNumber += 1;


        }
    }

    //기즈모 == 어떤 아이콘을 뜻함 ex) 씬 뷰의 카메라 아이콘, 조명 아이콘, 볼륨 아이콘
    private void OnDrawGizmos()//자동으로 씬 뷰에 기즈모를 그림
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPoint, 0.2f);   //해당 포인트에 구(球)를 그림, ',' 뒤는 구의 반지름

        Gizmos.color = Color.blue;
        for(int i = 0; i < createSpots.Length; i++)
        {
            Gizmos.DrawSphere(createSpots[i], 0.2f);
        }
    }

    void Save()
    {
        SaveData sd = new SaveData();

        sd.money = money;
        sd.person = person;
        sd.personCost = personCost;
        sd.personPrice = personPrice;
        sd.price = price;
        sd.priceCost = priceCost;
        sd.priceLevel = priceLevel;

        //저장
        XmlManager.XMLSerialize<SaveData>
            (sd, Application.persistentDataPath + "/save.xml");
    }

    void Load()
    {
        //로드
        SaveData sd = new SaveData();
        sd = XmlManager.XMLDeserialize<SaveData>
            (Application.persistentDataPath + "/save.xml");
        //원래 변수로 분배
        money = sd.money;
        person = sd.person;
        personCost = sd.personCost;
        personPrice = sd.personPrice;
        price = sd.price;
        priceCost = sd.priceCost;
        priceLevel = sd.priceLevel;
    }

    void LoadPerson()
    {
        for(int i = personOrigin; i < person; i++)
        {
            Recruit2();
            personOrigin += 1;
        }

    }

    void Recruit2()
    {
        int row = personOrigin % 3;

        Vector2 spot = new Vector2(createSpots[row].x,
                                   createSpots[row].y - (personOrigin / 3) * 3.88f);

        GameObject obj = Instantiate(prefabPerson, spot, Quaternion.identity);
    }

    //게임이 꺼질 때 자동으로 실행되는 함수
    private void OnApplicationQuit()
    {
        Save();
    }
    //게임이 일시정지(홈버튼 등) 되었을 때 실행되는 함수
    private void OnApplicationPause(bool pause)
    {
        //Save();
    }

    //인앱 결제 완료 후 보상 지급 함수
    public void Purchase()
    {
        money += 200000;
    }
}

[System.Serializable]
public class SaveData
{
    public long money;  //소지금
    public int person;  //직원 수
    public int priceLevel;  //단가 업그레이드 레벨
    public long price;      //클릭 당 단가
    public long priceCost;  //단가 업그레이드 가격

    public long personPrice; //직원이 버는 단가
    public long personCost; //직원 채용 가격
}