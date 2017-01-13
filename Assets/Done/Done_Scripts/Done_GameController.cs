using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class Done_GameController : MonoBehaviour
{


    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);
    [DllImport("user32.dll", EntryPoint = "GetForegroundWindow")]
    public static extern System.IntPtr GetForegroundWindow();
    [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
    public static extern int SetForegroundWindow(int hwnd);  
	///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   ///   
    [DllImport("User32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
    private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);



    public GameObject[] hazards;
	public Vector3 spawnValues;
    //
	public int hazardCount=1;
	//public float spawnWait=1;
	public float startWait=2;
    //
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	
	private bool gameOver;
	private bool restart;
    private int score = 0;
    private int Total = 0;
    private int Miss =0;
	//
	private bool gamestartModel1=false;
    private bool gamestartModel2=false;
    private bool gameselect=false;

    private bool gameReset = false;
//
    public static bool practiceModel = false;

	void Start ()
	{

        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        scoreText.text = "";
        score = 0;
        Total = 0;
        Miss = 0;

        //游戏在这里开始；
        //设置 “窗口大小的分辨率”的数值
        //Screen.SetResolution(ReadWriteXmlFile.width, ReadWriteXmlFile.height, false);
        //设置为 “最大窗口大小”

        OpenMain();
        //  Screen.SetResolution(800, 600,false);
        // Screen.SetResolution(Screen.width, Screen.height, false);

        gameselect = true;
//
        practiceModel = false;
    
    }

    void Update ()
	{
		if (gameselect) {
            if (AdjustSpeed.ModeSliderValue==1) {
                // if (Input.GetKeyDown(KeyCode.Alpha1)) {
                //if (DataConversion.FistOrPlam() == 1) { 
                gameOverText.text = "生存模式";
                gameselect = false;
                Done_Mover_asteroid.asteroidSpeed = (-1) * (AdjustSpeed.SpeedSliderValue);
                Done_Mover_enemy.enemySpeed = (-1) * (AdjustSpeed.SpeedSliderValue) - 1;
                hazardCount = AdjustSpeed.HardSliderValue;

                waveWait = 6 - AdjustSpeed.SpeedSliderValue;
                gamestartModel1 = true;
            }

            else if (AdjustSpeed.ModeSliderValue == 2) {
                // else if (Input.GetKeyDown(KeyCode.Alpha2)){
                // else if (DataConversion.FistOrPlam() == 2)  {   
                gameOverText.text = "练习模式";
                gameselect = false;
                Done_Mover_asteroid.asteroidSpeed = (-1) * (AdjustSpeed.SpeedSliderValue);
                Done_Mover_enemy.enemySpeed = (-1) * (AdjustSpeed.SpeedSliderValue) - 1;
                waveWait = 6 - AdjustSpeed.SpeedSliderValue;
                gamestartModel2 = true;
            }

            if (gamestartModel1) {             
                gamestartModel1 = false;
                restartText.text = "";
                UpdateScore();
                StartCoroutine(SpawnWaves());
                Client.GetInstance();         
            }
            if (gamestartModel2){
                //
                practiceModel = true;             
                gamestartModel2 = false;
                restartText.text = "";
                UpdateScore();
                StartCoroutine(SpawnPractice());
                Client.GetInstance();             
            }
        }
        
		if (restart)
		{
            if (Input.GetKeyDown(KeyCode.R)|| DataConversion.FistOrPlam() == 1) {
                Application.LoadLevel(Application.loadedLevel);
                Client.GetInstance().ReStart();
            }
              
            if (Input.GetKeyDown(KeyCode.E))
            {
                Client.GetInstance().Destroy();
				Application.Quit();
			}
		 }
	}

    IEnumerator SpawnPractice() {
        yield return new WaitForSeconds(startWait);
        gameOverText.text = "";
        while (true) {
            GameObject hazard1 = hazards[0];
            Vector3 spawnPosition1 = new Vector3(6, spawnValues.y, spawnValues.z);
            Quaternion spawnRotation1 = Quaternion.identity;
            Instantiate(hazard1, spawnPosition1, spawnRotation1);
            yield return new WaitForSeconds(waveWait);
            GameObject hazard2 = hazards[0];
            Vector3 spawnPosition2 = new Vector3(0, spawnValues.y, spawnValues.z);
            Quaternion spawnRotation2 = Quaternion.identity;
            Instantiate(hazard2, spawnPosition2, spawnRotation2);
            yield return new WaitForSeconds(waveWait);
            GameObject hazard3 = hazards[0];
            Vector3 spawnPosition3 = new Vector3(-6, spawnValues.y, spawnValues.z);
            Quaternion spawnRotation3 = Quaternion.identity;
            Instantiate(hazard3, spawnPosition3, spawnRotation3);
            yield return new WaitForSeconds(waveWait);
            GameObject hazard4 = hazards[0];
            Vector3 spawnPosition4 = new Vector3(0, spawnValues.y, spawnValues.z);
            Quaternion spawnRotation4 = Quaternion.identity;
            Instantiate(hazard4, spawnPosition4, spawnRotation4);
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                restartText.text = "握一下拳重新开始";
                restart = true;
                break;
            }
        }
    }


    IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds (startWait);
        gameOverText.text = "";
        while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
                GameObject hazard = hazards[UnityEngine.Random.Range(0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (UnityEngine.Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (0.75f);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				restartText.text = "握拳重新开始 " + "手掌退出游戏";
				restart = true;
				break;
			}
		}
	}

    public void AddScore (int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

    public void AddTotal(int newTotalValue) {
        Total += newTotalValue;
        UpdateScore();
    }

    public void AddMiss(int newMissValue) {
        Miss += newMissValue;
        UpdateScore();
    }

	void UpdateScore ()
	{
		scoreText.text =  "总数 :" +Total + "  失误:" +Miss + "  分数: " + score;
    }
	
	public void GameOver ()
	{
		gameOverText.text = "游戏结束!";
		gameOver = true;
	}

    void OnApplicationQuit()
    {
        Client.GetInstance().Destroy();
        Application.Quit();
    }

	//使窗口最大化
    private void OpenMain()
    {
        ShowWindow(GetForegroundWindow(), 3);
        IntPtr hWnd = FindWindow(null, "MaxTest");
        ShowWindow(hWnd, 3);
    }

}