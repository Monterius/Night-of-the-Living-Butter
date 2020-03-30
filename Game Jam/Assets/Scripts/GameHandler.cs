using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, WAVE, COOLDOWN, END }

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;

    public int countdownTime;
    public GameObject TimerHUD;
    public GameObject DeathHUD;
    public GameObject WinHUD;
    public GameObject Canvas;

    public TextMeshProUGUI RoundsLasted;
    public TextMeshProUGUI countdownDisplay;
    public TextMeshProUGUI waveNumber;

    public CameraFollow cameraFollow;
    public Transform playerTransform;
    public Transform bossTransform;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    public Unit playerUnit;
    public Unit enemyUnit;
    public Unit bossUnit;

    CharacterAnim Wonder;
    DoorAnim Door;
    public EnemyAI Butter;

    public BattleHUDBattle playerHUD;

    // This is all for spawning enemies
    public Transform[] spawnPoints;
    public Transform[] spawnPoints2;

    int randomSpawn;
    public static bool spawnAllowed;
    GameObject spawn;

    public int Wave;
    public int numSpawns = 0;
    public int maxSpawns;
    public int spawnRate = 3;
    public int EnemiesLeft;
    int postBoss = 0;
    public bool Dead;

    public BattleState state;

    private void Start()
    {
        instance = this;

        DeathHUD.SetActive(false);
        WinHUD.SetActive(false);
        TimerHUD.SetActive(false);
        bossPrefab.SetActive(false);
   
        Wave = 1;
        Dead = false;
        //spawnPoints = new List<Transform>(4);

        Wonder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterAnim>();
        Door = GameObject.FindGameObjectWithTag("Door").GetComponent<DoorAnim>();

        state = BattleState.START;
        StartCoroutine(SetupGame());
    }

    public SoundAudioClip[] soundAudioClipArray;
    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    private void Update()
    {
        if (Dead)
        {
            StartCoroutine(Death());

        }
    }

    IEnumerator SetupGame()
    {
        cameraFollow.Setup(() => playerTransform.position);
        playerHUD.setHUD(playerUnit);

        SoundManager.PlaySound(SoundManager.Sound.Start);

        waveNumber.text = "WAVE: " + Wave;

        enemyUnit.currentHP = enemyUnit.maxHP;

        Butter.moveSpeed = 3f;

        yield return new WaitForSeconds(2f);
        state = BattleState.WAVE;
        StartCoroutine(SetupWave(Wave));
    }

    IEnumerator SetupWave(int wave)
    {
        waveNumber.text = "WAVE: " + Wave;
        EnemiesLeft = 0;

        spawnAllowed = true;
        InvokeRepeating("SpawnButter", 0f, spawnRate);

        yield return new WaitForSeconds(1f);
    }

    IEnumerator Death()
    {
        SoundManager.PlaySound(SoundManager.Sound.Dead);

        yield return new WaitForSeconds(0.5f);

        Canvas.SetActive(false);
        DeathHUD.SetActive(true);
        RoundsLasted.text = "You survived " + (Wave - 1) + " Rounds!";

        spawnAllowed = false;
        Wonder.isDead = true;

        yield return new WaitForSeconds(1f);

        playerPrefab = null;
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(10f);
        TimerHUD.SetActive(true);

        while (countdownTime > 0)
        {
            countdownDisplay.text = "Time Until Next Wave: " + countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }
        countdownTime = 10;
        TimerHUD.SetActive(false);
        if (Wave == 3)
        {
            Door.isOpen = true;
            SoundManager.PlaySound(SoundManager.Sound.Door);
            state = BattleState.WAVE;
            StartCoroutine(BossBattle());
        }
        else
        {
            Wave += 1;
            maxSpawns += Wave * 2;
            Butter.moveSpeed *= 1.25f;
            state = BattleState.WAVE;
            StartCoroutine(SetupWave(Wave));
        }
    }

    IEnumerator BossBattle()
    {
        yield return new WaitForSeconds(10f);

        waveNumber.text = "WAVE: BOSS";

        while (playerTransform.position.y < 101)
        {
            yield return new WaitForSeconds(0.5f);
        }

        bossPrefab.SetActive(true);
        EnemiesLeft = 1;

        while (EnemiesLeft > 0)
        {
            yield return new WaitForSeconds(1f);
            // play boss music
        }

        SoundManager.PlaySound(SoundManager.Sound.Won);

        yield return new WaitForSeconds(1f);

        WinHUD.SetActive(true);      
    }

    void SpawnButter()
    {
        if (spawnAllowed)
        {
            switch (postBoss)
            {
                case 0:
                    randomSpawn = Random.Range(0, spawnPoints.Length);
                    Instantiate(enemyPrefab, spawnPoints[randomSpawn].position, Quaternion.identity);
                    numSpawns += 1;
                    EnemiesLeft += 1;
                    if (numSpawns == maxSpawns)
                    {
                        spawnAllowed = false;
                        numSpawns = 0;

                        state = BattleState.COOLDOWN;
                        StartCoroutine(Countdown());
                    }
                    break;

                case 1:
                    randomSpawn = Random.Range(0, spawnPoints2.Length);
                    Instantiate(enemyPrefab, spawnPoints2[randomSpawn].position, Quaternion.identity);
                    numSpawns += 1;
                    EnemiesLeft += 1;
                    if (numSpawns == maxSpawns)
                    {
                        spawnAllowed = false;
                        numSpawns = 0;

                        state = BattleState.COOLDOWN;
                        StartCoroutine(Countdown());
                    }
                    break;
            }
        }
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnContinueButton()
    {
        WinHUD.SetActive(false);
        postBoss = 1;
        Wave += 1;
        StartCoroutine(SetupWave(Wave));
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
