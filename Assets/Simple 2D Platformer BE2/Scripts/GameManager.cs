using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health;
    GameData data;

    /*
    public Transform startPos;
    public PlayerMove player;

    public GameObject[] stages;

    public Image[] UIHealth;
    public TMP_Text UIPoint;
    public TMP_Text UIStage;
    public GameObject RestartBtn; 
    */

    // =========================================================
    private static GameManager instance = null;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        initGameData();
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    // =========================================================

    public void initGameData()
    {
        data = GameObject.Find("GameData").GetComponent<GameData>();

        data.RestartBtn.SetActive(false);
        for (int i = 0; i < data.stages.Length; i++)
            data.stages[i].SetActive(false);
        

        totalPoint = stagePoint = 0;
        stageIndex = 0;
        data.stages[stageIndex].SetActive(true);

        health = 3;
        for(int i=0;i< data.UIHealth.Length;i++)
            data.UIHealth[i].color = new Color(1, 1, 1, 1f);

        PlayerReposition();

    }

    public void NextStage()
    {
        // Change Start
        if(stageIndex < data.stages.Length-1)
        {
            data.stages[stageIndex].SetActive(false);
            stageIndex++;
            data.stages[stageIndex].SetActive(true);
            PlayerReposition();

            data.UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else // Game Clear
        {
            Time.timeScale = 0;
            Debug.Log("Game Clear");
            TMP_Text txt = data.RestartBtn.GetComponentInChildren<TMP_Text>();
            txt.text = "Game Clear!!";
            data.RestartBtn.SetActive(true);
        }

        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;

    }

    public void HealthDown()
    {
        Debug.Log("Health down");
        health--;
        if (health > 0)
        {

            data.UIHealth[health].color = new Color(1, 0, 0, 0.2f);
        }
        else
        {
            Debug.Log("Health:" + health);
            // Player Die Effect
            data.player.OnDie();
            // Result UI

            // Retry Button UI 
            data.RestartBtn.SetActive(true);
        }

    }

    public void PlayerReposition()
    {
        data.player.transform.position = data.startPos.transform.position;
        data.player.VelocityZero();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        data.UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
