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

    [SerializeField] private int currentWaveIndex = 0;  //���� wav
    private int currentSpawnCount = 0;
    private int waveSpawnCount = 0;
    private int waveSpawnPosCount = 0;

    public float spawnInterval = .5f;   //0.5�ʸ��� �����ϰ� �� ��.
    public List<GameObject> enemyPrefebs = new List<GameObject>();  //�������� ã�ƿ���.

    [SerializeField] private Transform spawnPositionsRoot;
    private List<Transform> spawnPostions = new List<Transform>();

    public List<GameObject> rewards = new List<GameObject>();   //������

    [SerializeField] private CharacterStats defaultStats;    // ĳ���� ����
    [SerializeField] private CharacterStats rangedStats;

    private void Awake()
    {
        Instance = this;
        Player = GameObject.FindGameObjectWithTag(playerTag).transform; //FindGameObjectWithTag �� �ѹ��� ����ϱ�.

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

 

    IEnumerator StartNextWave() //���࿡ ���� ������ �����ߴٰ� ���ƿͼ� �ٽ�.
    {
        while(true) //���� ����
        {
            if(currentSpawnCount == 0)  //ù �����̰ų�, �� ��Ұų�.
            {
                UpdateWaveUI();
                yield return new WaitForSeconds(2f);

                if (currentWaveIndex % 20 == 0) //20������ ���׷��̵� �ǰ�.
                {
                    RandomUpgrade();
                }

                if (currentWaveIndex % 10 == 0)
                {
                    waveSpawnPosCount = waveSpawnPosCount + 1 > spawnPostions.Count ? waveSpawnPosCount : waveSpawnPosCount + 1;
                    waveSpawnCount = 0;
                }

                if (currentWaveIndex % 5 == 0)  //5��°���� ������ ����.
                {
                    CreateReward();
                }

                if (currentWaveIndex % 3 == 0)
                {
                    waveSpawnCount += 1;
                }


                for (int i = 0; i < waveSpawnPosCount; i++)
                {
                    int posIdx = Random.Range(0, spawnPostions.Count);  //��ġ ���
                    for (int j = 0; j < waveSpawnCount; j++)    //����� ���
                    {
                        int prefabIdx = Random.Range(0, enemyPrefebs.Count);
                        GameObject enemy = Instantiate(enemyPrefebs[prefabIdx], spawnPostions[posIdx].position, Quaternion.identity);
                        enemy.GetComponent<HealthSystem>().OnDeath += OnEnemyDeath; //�׾��� �� �̰� ������.

                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(defaultStats); // ���͸� ������ �� �����־�� ��.
                        enemy.GetComponent<CharacterStatsHandler>().AddStatModifier(rangedStats);

                        currentSpawnCount++;
                        yield return new WaitForSeconds(spawnInterval);
                    }
                }
                currentWaveIndex++; //���̺�(Wav) ī��Ʈ
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
        StopAllCoroutines();    //�� �����.
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


    private void UpgradeStatInit()  //������ �ʱ�ȭ�ϴ� �ڵ�.
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

    void RandomUpgrade()    //�������� ���׷��̵� �Ǵ�. ���� �Ȱ����ϱ� ���ݾ� ������ �� �ִ�.
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
                defaultStats.attackSO.isOnKnockback = true; //�˹�
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
