using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public Transform startPos;
    public PlayerMove player;

    public GameObject[] stages;

    public Image[] UIHealth;
    public TMP_Text UIPoint;
    public TMP_Text UIStage;
    public GameObject RestartBtn;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.initGameData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
