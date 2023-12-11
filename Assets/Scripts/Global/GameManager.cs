//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player {  get; private set; }
    [SerializeField] private string playerTag = "Player";
    private HealthSystem playerHealthSystem;

    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Slider hpGaugeSlider;
    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private int currentWaveIndex = 0;  //몬스터 wav
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = .5f;   //0.5초마다 생성하게 할 것.
    public List<GameObject> enemyPrefebs = new List<GameObject>();  //랜덤으로 찾아오게.

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPostions = new List<Transform>();

    public List<GameObject> rewards = new List<GameObject>();   //아이템

    [SerializeField] private CharacterStats defaultStats;    // 캐릭터 스탯
    [SerializeField] private CharacterStats rangedStats;

    private void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform; //FindGameObjectWithTag 딱 한번만 사용하기.

        //UI
        playerHealthSystem = Player.GetComponent<HealthSystem>();
        playerHealthSystem.OnDamage += UpdateHealthUI;
        playerHealthSystem.OnHeal += UpdateHealthUI;
        playerHealthSystem.OnDeath += GameOver;

        gameOverUI.SetActive(false);

        for (int i = 0; i < spawnPositionsRoot.childCount; i++)
        {
            spawnPostions.Add(spawnPositionsRoot.GetChild(i));
        }
    }

    private void Start()
    {
        UpgradeStatInit();
        StartCoroutine("StartNextWave");
    }

 

    IEnumerator StartNextWave() //실행에 대한 순서를 감안했다가 돌아와서 다시.
    {
        while(true) //무한 루프
        {
            if(currentSpawnCount == 0)  //첫 시작이거나, 다 잡았거나.
            {
                UpdateWaveUI();
                yield return new WaitForSeconds(2f);

                if (currentWaveIndex % 20 == 0) //20번마다 업그레이드 되게.
                {
                    RandomUpgrade();
                }

                if (currentWaveIndex % 10 == 0)
                {
                    waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPostions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
                    waveSpawnCount = 0;
                }

                if (currentWaveIndex % 5 == 0)  //5번째마다 리워드 생성.
                {
                    CreateReward();
                }

                if (currentWaveIndex % 3 == 0)
                {
                    waveSpawnCount += 1;
                }


                for (int i = 0; i < waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, spawnPostions.Count);  //위치 계수
                    for (int j = 0; j < waveSpawnCount; j++)    //만드는 계수
                    {
                        int prefabIdx = Random.Range(0, enemyPrefebs.Count);
                        GameObject enemy = Instantiate(enemyPrefebs[prefabIdx], spawnPostions[posIdx].position, Quaternion.identity);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath; //죽었을 때 이거 실행해.

                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStats); // 몬스터를 생성할 때 지워주어야 함.
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStats);

                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }
                currentWaveIndex++; //웨이브(Wav) 카운트
            }
            yield return null;
        }
    }

    private void OnEnemyDeath()
    {
        currentSpawnCount--;
    }


    private void UpdateHealthUI()
    {
        hpGaugeSlider.value = playerHealthSystem.CurrentHealth / playerHealthSystem.MaxHealth;
    }

    private void GameOver()
    {
        gameOverUI.SetActive(true);
        StopAllCoroutines();    //다 멈춰라.
    }

    private void UpdateWaveUI() //
    {
        waveText.text = (currentWaveIndex + 1).ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit(); 
    }


    private void UpgradeStatInit()  //스탯을 초기화하는 코드.
    {
        defaultStats.statsChangeType = StatsChangeType.Add;
        defaultStats.attackSO = Instantiate(defaultStats.attackSO);

        rangedStats.statsChangeType = StatsChangeType.Add;
        rangedStats.attackSO = Instantiate(rangedStats.attackSO);
    }


    void CreateReward()
    {
        int idx = Random.Range(0, rewards.Count);
        int posIdx = Random.Range(0, spawnPostions.Count);

        GameObject obj = rewards[idx];
        Instantiate(obj, spawnPostions[posIdx].position, Quaternion.identity);
    }

    void RandomUpgrade()    //랜덤으로 업그레이드 되는. 적이 똑같으니까 조금씩 발전할 수 있는.
    {
        switch (Random.Range(0,6))
        {
            case 0:
                defaultStats.maxHealth += 2;
                break;
            case 1:
                defaultStats.attackSO.power += 1;
                break;
            case 2:
                defaultStats.speed += 0.1f;
                break;
            case 3:
                defaultStats.attackSO.isOnKnockback = true; //넉백
                defaultStats.attackSO.knockbackPower += 1;
                defaultStats.attackSO.knockbackTime = 0.1f;
                break;
            case 4:
                defaultStats.attackSO.delay -= 0.5f;
                break;
            case 5:
                RangedAttackData rangedAttackData = rangedStats.attackSO as RangedAttackData;
                rangedAttackData.numberofProjectilesPerShot += 1;
                break;

            default:
                break;
        }
    }
}
