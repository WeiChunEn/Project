﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class Ass
{
    private GameObject m_Pin;    //射出的針
    private GameObject m_Spawn; //針的射出點
    public GameObject _gGameCanvas; //遊戲關卡的Canvas
    public GameObject _gGameOver_Panel; //GameOver的Panel
    public GameObject _gGameClear_Panel; //GameClear的Panel
    public GameObject _gLimitOver_Panel;    //極限失敗的Panel
    public GameObject _gLimitClear_Panel;    //極限通關的Panel
    public GameObject _gCan_Shoot;  //判斷能不能發射的
    public GameObject _gPoint_Text; //關卡分數
    public GameObject _gCountDown; //倒數計時
    public GameObject _gGame_Time; //遊戲時間物件
    public GameObject _gSet_Btn; //設定的按鈕
    public GameObject _gSet_Menu; //設定的介面
    public Rotater_System m_Rotater_System = null;
    private PlayerSystem m_Player_Ststem = null;
    private LevelSystem m_Level_System = null;
    public bool _bGame_Start;
    public bool _bCountDown_Start;
    public int _iPin_Num;
    public int _iNow_Level;
    public string[] _sClear_Level = new string[15];     //關卡Array
    public string[,] _sLevel_Star = new string[15, 3];  //關卡星星Array
    public string _sClear_Type;         //通關狀態
    public float _fCount_Down;          //倒數秒數
    public int _iPlayer_Point;      //玩家分數
    public int _iLevel_Point;     //關卡分數
    public float _fGame_Time;        //遊戲時間
    private int[] m_Skill_Time; //技能發動的時間
    private int[] m_Skill_Over_Time; //技能結束的時間
    private bool[] m_14_Skill = new bool[2];  //  14關技能\
    private bool[] m_15_Skill = new bool[3];  // 15關技能


    //Singleton
    private static Ass _instance;
    public static Ass Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Ass();
            return _instance;
        }
    }
    private Ass() { }

    public void Initinal()
    {
        _bGame_Start = false;
        _bCountDown_Start = false;
        _fCount_Down = 0.0f;
        _fGame_Time = 60;
        _gSet_Btn = GameObject.Find("Set_Btn");
        if (_iNow_Level <= 5)
        {
            if (_gSet_Btn)
            {
                _gSet_Btn.transform.GetChild(0).gameObject.SetActive(true);
            }

            m_Pin = Resources.Load<GameObject>("Prefebs/Pin1-5");
        }
        else if (_iNow_Level <= 10)
        {
            _fGame_Time = 80;
            if (_gSet_Btn)
            {
                _gSet_Btn.transform.GetChild(1).gameObject.SetActive(true);
            }
            m_Pin = Resources.Load<GameObject>("Prefebs/Pin6-10");
        }
        else if (_iNow_Level <= 15)
        {
            _fGame_Time = 100;
            if (_gSet_Btn)
            {
                _gSet_Btn.transform.GetChild(2).gameObject.SetActive(true);
            }
            m_Pin = Resources.Load<GameObject>("Prefebs/Pin11-15");
        }
        if (_iNow_Level == 14)
        {
            m_Skill_Time = new int[2];
            m_Skill_Over_Time = new int[2];
            for (int i = 0; i < 2; i++)
            {
                m_Skill_Time[i] = UnityEngine.Random.Range(10, (int)_fGame_Time);
                Debug.Log(m_Skill_Time[i]);
            }
        }
        else if (_iNow_Level == 15)
        {
            m_Skill_Time = new int[3];
            m_Skill_Over_Time = new int[3];
            for (int i = 0; i < 3; i++)
            {
                m_Skill_Time[i] = UnityEngine.Random.Range(10, (int)_fGame_Time);
                Debug.Log(m_Skill_Time[i]);
            }
        }
        _gGameCanvas = GameObject.Find("GameCanvas");
        _gGameOver_Panel = GameObject.Find("GameOver");
        _gGameClear_Panel = GameObject.Find("GameClear");
        _gSet_Menu = GameObject.Find("Set_Menu");
        if (_gGameOver_Panel && _gGameClear_Panel)
        {
            _gGameOver_Panel.SetActive(false);
            _gGameClear_Panel.SetActive(false);
            _gSet_Menu.SetActive(false);
            m_Pin.tag = "Pin";
        }
        m_Level_System = new LevelSystem(this);
        m_Player_Ststem = new PlayerSystem(this);
        m_Rotater_System = new Rotater_System(this);
        _gPoint_Text = GameObject.Find("PointText");
        m_Spawn = GameObject.Find("Spawn");
        _gCountDown = GameObject.Find("CountDown");
        _gCan_Shoot = GameObject.Find("CanShoot");
        _gGame_Time = GameObject.Find("GameTime");

        if(_iNow_Level==-1)
        {
            _fGame_Time = 180;
            m_Pin = Resources.Load<GameObject>("Prefebs/PinLimit");
            _gLimitClear_Panel= GameObject.Find("LimitClear");
            _gLimitOver_Panel = GameObject.Find("LimitOver");
            _gLimitOver_Panel.SetActive(false);
            _gLimitClear_Panel.SetActive(false);
            _gSet_Menu.SetActive(false);
        }

    }
    public void Update()
    {
        m_Player_Ststem.Update();
        if (_bCountDown_Start == true)
        {
            _fCount_Down += Time.deltaTime;
            if (_iNow_Level >= 6 && _iNow_Level <= 15)
            {

                if (_fGame_Time > 60 && _fGame_Time <= 70 || _fGame_Time <= 10)
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + "0" + ((int)_fGame_Time % 60).ToString();

                }
                else if (_fGame_Time == 60)
                {

                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + "0" + "0";
                }
                else
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + ((int)_fGame_Time % 60).ToString();

                }
                if (_fGame_Time <= 20)
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().color = new Color(229f / 255f, 30f / 255f, 38f / 255f);
                }
            }
            if (_fCount_Down <= 1.0f)
            {

                _gCountDown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "3";
            }
            else if (_fCount_Down <= 2.0f)
            {
                _gCountDown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "2";
            }
            else if (_fCount_Down <= 3.0f)
            {
                _gCountDown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
            }
            else
            {
                _gCountDown.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "GO~";
            }
            if (_fCount_Down >= 4.0f)
            {
                Count_Down();
            }
        }



        if (_bGame_Start == true)
        {

            InputProcess();
            
            m_Rotater_System.Update();


            if (_iNow_Level >= 6 && _iNow_Level <= 15)
            {
                _fGame_Time -= Time.deltaTime;
                if (_fGame_Time > 60 && _fGame_Time <= 70 || _fGame_Time <= 10)
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + "0" + ((int)_fGame_Time % 60).ToString();

                }
                else if (_fGame_Time == 60)
                {

                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + "0" + "0";
                }
                else
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().text = "0" + ((int)_fGame_Time / 60).ToString() + ":" + ((int)_fGame_Time % 60).ToString();

                }
                if (_fGame_Time <= 20)
                {
                    _gGame_Time.GetComponent<TextMeshProUGUI>().color = new Color(229f / 255f, 30f / 255f, 38f / 255f);
                }
                if (_iNow_Level == 14)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        
                        if (m_Skill_Time[i] == Convert.ToInt32(_fGame_Time))
                        {
                            m_Skill_Over_Time[i] = m_Skill_Time[i] - 10;
                            m_Skill_Time[i] = -1;
                            Skill_Start();
                        }
                    }
                }
                else if (_iNow_Level == 15)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (m_Skill_Time[i] == (int)_fGame_Time)
                        {
                            m_Skill_Over_Time[i] = m_Skill_Time[i] - 10;
                            m_Skill_Time[i] = -1;
                            Skill_Start();
                        }
                    }
                }
            }
            if (_fGame_Time <= 0)
            {
                GameOver();
            }


        }





    }
    /// <summary>
    /// End
    /// </summary>
    public void Release()
    {

        m_Spawn = null;
        _gGameCanvas = null;
        _gGameOver_Panel = null;
        _gGameClear_Panel = null;
        _gPoint_Text = null;
    }
    /// <summary>
    /// 偵測輸入
    /// </summary>
    private void InputProcess()
    {

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
        
		  if (Input.touchCount == 1 && _gCan_Shoot.tag == "true")
          {
            Touch touch;
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                 if(!EventSystem.current.currentSelectedGameObject)
                 {
                    Create_Pin();
                    if (_iPin_Num == 1)
                    {

                      m_Pin.tag = "Last";
                     }   

                  }
            }

         }

#else

        if (Input.GetButtonDown("Fire1") && _gCan_Shoot.tag == "true")
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);
            if (!EventSystem.current.currentSelectedGameObject)
            {
                Create_Pin();
                if (_iPin_Num == 1)
                {

                    m_Pin.tag = "Last";
                }

            }




        }

#endif

    }

    /// <summary>
    /// 生成針
    /// </summary>
    /// <returns></returns>
    public GameObject Create_Pin()
    {
        _iPin_Num--;

        _gCan_Shoot.tag = "false";
        return UnityEngine.Object.Instantiate(m_Pin, m_Spawn.transform.position, m_Spawn.transform.rotation, m_Spawn.transform);

    }

    /// <summary>
    /// 設定每關針的數量
    /// </summary>
    public void Set_Pin_Num()
    {
        switch (_iNow_Level)
        {
            case 1:
                _iPin_Num = 8;
                break;
            case 2:
                _iPin_Num = 10;
                break;
            case 3:
                _iPin_Num = 10;
                break;
            case 4:
                _iPin_Num = 12;
                break;
            case 5:
                _iPin_Num = 15;
                break;
            case 6:
                _iPin_Num = 10;
                break;
            case 7:
                _iPin_Num = 12;
                break;
            case 8:
                _iPin_Num = 12;
                break;
            case 9:
                _iPin_Num = 15;
                break;
            case 10:
                _iPin_Num = 15;
                break;
            case 11:
                _iPin_Num = 15;
                break;
            case 12:
                _iPin_Num = 15;
                break;
            case 13:
                _iPin_Num = 15;
                break;
            case 14:
                _iPin_Num = 15;
                break;
            case 15:
                _iPin_Num = 15;
                break;
        }
    }

    /// <summary>
    /// 設定關卡通過以及星星
    /// </summary>
    /// <param name="_sClear_Type">通過的狀態</param>
    public void Level_End(string _sClear_Type)
    {

        switch (_sClear_Type)
        {
            case "fail":
                break;
            case "Clear":
                _sClear_Level[_iNow_Level] = "true";
                break;
            case "one":
                _sLevel_Star[_iNow_Level - 1, 0] = "true";
                _sClear_Level[_iNow_Level] = "true";
                break;
            case "two":
                _sLevel_Star[_iNow_Level - 1, 0] = "true";
                _sLevel_Star[_iNow_Level - 1, 1] = "true";
                _sClear_Level[_iNow_Level] = "true";
                break;
            case "three":
                _sLevel_Star[_iNow_Level - 1, 0] = "true";
                _sLevel_Star[_iNow_Level - 1, 1] = "true";
                _sLevel_Star[_iNow_Level - 1, 2] = "true";
                _sClear_Level[_iNow_Level] = "true";
                break;
        }
    }

    /// <summary>
    /// 讀取關卡通過的狀態
    /// </summary>
    public void Load_Level()
    {
        Level_End(_sClear_Type);


    }

    /// <summary>
    /// 失敗介面
    /// </summary>
    public void GameOver()
    {
        _gGameOver_Panel.SetActive(true);
        _sClear_Type = "fail";
        _gGameCanvas.GetComponent<Canvas>().sortingOrder = 1;
        _bGame_Start = false;




    }
    /// <summary>
    /// 通關介面
    /// </summary>
    public void GameClear()
    {

        _gGameClear_Panel.SetActive(true);
        _gGameCanvas.GetComponent<Canvas>().sortingOrder = 1;
        _bGame_Start = false;

    }
    /// <summary>
    /// 重玩一次
    /// </summary>
    public void Game_Restart()
    {

    }
    /// <summary>
    /// 倒數計時
    /// </summary>
    public void Count_Down()
    {


        _gCountDown.SetActive(false);
        _bGame_Start = true;
        _gGameCanvas.GetComponent<Canvas>().sortingOrder = -1;
        Set_Pin_Num();
        _bCountDown_Start = false;
        _fCount_Down = 0.0f;


    }
    /// <summary>
    /// 存取關卡星星
    /// </summary>
    public void Set_Star()
    {
        _iLevel_Point = Int32.Parse(_gPoint_Text.GetComponent<TextMeshProUGUI>().text);

        switch (_iNow_Level)
        {
            case 1:
                if (_iLevel_Point < 50)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 60)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 70)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 2:
                if (_iLevel_Point < 50)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 70)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 100)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 3:
                if (_iLevel_Point < 50)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 80)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 110)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 4:
                if (_iLevel_Point < 120)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 150)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 180)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 5:
                if (_iLevel_Point < 150)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 180)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 200)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 6:
                if (_iLevel_Point < 70)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 90)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 120)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 7:
                if (_iLevel_Point < 120)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 150)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 180)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 8:
                if (_iLevel_Point < 240)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 270)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 300)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 9:
                if (_iLevel_Point < 300)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 330)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 360)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;
            case 10:
                if (_iLevel_Point < 330)
                {
                    _sClear_Type = "Clear";
                }
                else if (_iLevel_Point < 360)
                {
                    _sClear_Type = "one";
                }
                else if (_iLevel_Point < 400)
                {
                    _sClear_Type = "two";
                }
                else
                {
                    _sClear_Type = "three";
                }
                break;


        }
    }

    public void Set_Open_Menu()
    {
        if (_bGame_Start == true)
        {
            _gSet_Menu.SetActive(true);
            _bGame_Start = false;
            _gGameCanvas.GetComponent<Canvas>().sortingOrder = 4;
        }

    }
    public void Set_Close_Menu()
    {
        if (_bGame_Start == false && _gSet_Menu.activeSelf == true)
        {
            _gSet_Menu.SetActive(false);
            _bGame_Start = true;
            _gGameCanvas.GetComponent<Canvas>().sortingOrder = -1;
        }
    }


    public void Skill_Start()
    {
        if (_iNow_Level == 14)
        {

            int tmp = UnityEngine.Random.Range(0, 2);
            if (tmp == 0 && m_14_Skill[tmp] == true)
            {
                m_14_Skill[tmp + 1] = true;
            }
            else if (tmp == 1 && m_14_Skill[tmp] == true)
            {
                m_14_Skill[tmp - 1] = true;
            }
            else
            {
                m_14_Skill[tmp] = true;
            }


            Debug.Log(m_14_Skill[0]);
            Debug.Log(m_14_Skill[1]);


        }
        else if (_iNow_Level == 15)
        {

            int tmp = UnityEngine.Random.Range(0, 3);
            if (tmp == 0 && m_15_Skill[tmp] == true)
            {
                tmp = UnityEngine.Random.Range(1, 3);
                if (tmp == 1 && m_15_Skill[tmp] == true)
                {
                    m_15_Skill[tmp + 1] = true;
                }
                else if(tmp==2&&m_15_Skill[tmp]==true)
                {
                    m_15_Skill[tmp - 1] = true;
                }

            }
            else if (tmp == 1 && m_15_Skill[tmp] == true)
            {
                if (m_15_Skill[tmp - 1]==true)
                {
                    m_15_Skill[tmp + 1] = true;
                }
                else if(m_15_Skill[tmp + 1])
                {
                    m_15_Skill[tmp - 1] = true;
                }
                
            }
            else if(tmp == 2 && m_15_Skill[tmp] == true)
            {
                tmp = UnityEngine.Random.Range(0, 2);
                if (tmp == 1 && m_15_Skill[tmp] == true)
                {
                    m_15_Skill[tmp - 1] = true;
                }
                else if (tmp == 0 && m_15_Skill[tmp] == true)
                {
                    m_15_Skill[tmp + 1] = true;
                }
            }
            else
            {
                m_15_Skill[tmp] = true;
            }
            Debug.Log(m_15_Skill[0]);
            Debug.Log(m_15_Skill[1]);
            Debug.Log(m_15_Skill[2]);

        }
    }
    /// <summary>
    /// 存取資料
    /// </summary>
    public void Save_Data()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                //PlayerPrefs.SetString("PlayerStar" + i.ToString()+"_" + j.ToString(), _sLevel_Star[i, j]);

            }
            //  PlayerPrefs.SetString("PlayerLevel" + i.ToString(), _sClear_Level[i]);

        }
    }
    /// <summary>
    /// 讀取資料
    /// </summary>
    public void Load_Data()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _sLevel_Star[i, j] = PlayerPrefs.GetString("PlayerStar" + i.ToString() + "_" + j.ToString());

            }
            _sClear_Level[i] = PlayerPrefs.GetString("PlayerLevel" + i.ToString());

        }
    }
}
