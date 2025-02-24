using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int startWave = 0;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] List<Wave> waves;
    [SerializeField] Text waveNum, enemyNum;
    List<Enemy> enemies = new List<Enemy>();
    private void Awake()
    {
        StartCoroutine(Spawning(startWave));
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().gameover += OnGameOver;
    }
    IEnumerator Spawning(int wave)
    {
        if(wave >= waves.Count)
        {
            waveNum.text = "게임 종료";
            yield break;
        }
        waveNum.text = "웨이브 " + (wave + 1) + ((wave == waves.Count - 1) ? "(최종 웨이브)" : (waves[wave].boss != null) ? "(보스 웨이브)" : "");
        float spawnCounter = 0.0f;
        if (waves[wave].enemies.Count > 0)
        {
            while (true)
            {
                if (spawnCounter < waves[wave].spawnRate) spawnCounter += Time.deltaTime;
                else
                {
                    spawnCounter -= waves[wave].spawnRate;
                    if (waves[wave].enemies[0].remaining == 0)
                    {
                        waves[wave].enemies.RemoveAt(0);
                    }
                    if (waves[wave].enemies.Count == 0)
                    {
                        break;
                    }
                    Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
                    Enemy tmp = Instantiate(waves[wave].enemies[0].enemy, spawn.position, Quaternion.identity).GetComponent<Enemy>();
                    enemies.Add(tmp);
                    tmp.onDeath.AddListener(delegate
                    {
                        enemies.Remove(tmp);
                        EnemyNumTextUpdate();
                    });
                    EnemyNumTextUpdate();
                    waves[wave].enemies[0].remaining--;
                }
                yield return null;
            }
        }
        if (waves[wave].boss != null)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Enemy tmp = Instantiate(waves[wave].boss, spawn.position, Quaternion.identity).GetComponent<Enemy>();
            enemies.Add(tmp);
            tmp.onDeath.AddListener(delegate
            {
                enemies.Remove(tmp);
                EnemyNumTextUpdate();
            });
            EnemyNumTextUpdate();
        }
        while (true)
        {
            if (enemies.Count == 0) break;
            yield return null;
        }
        GameManager.Instance.EarnMoney(waves[wave].waveReward);
        yield return new WaitForSeconds(waves[wave].endTime);
        StartCoroutine(Spawning(wave + 1));
    }
    public void EnemyNumTextUpdate()
    {
        enemyNum.text = (enemies.Count == 0) ? "웨이브 완료" : "남은 적: " + enemies.Count;
    }
    public void OnGameOver()
    {
        StopAllCoroutines();
        foreach (var i in enemies)
        {
            i.gameObject.SetActive(false);
        }
    }
}
[System.Serializable]
public class Wave
{
    public List<EnemyCount> enemies = new List<EnemyCount>();
    [SerializeField] GameObject m_boss = null;
    public GameObject boss { get { return m_boss; } }
    [SerializeField] float m_spawnRate = 1.0f;
    public float spawnRate { get { return m_spawnRate; } }
    [SerializeField] float m_endTime = 3.0f;
    public float endTime { get { return m_endTime; } }
    [SerializeField] int m_waveReward = 0;
    public int waveReward { get { return m_waveReward; } }
}
[System.Serializable]
public class EnemyCount
{
    [SerializeField] GameObject m_enemy;
    public GameObject enemy { get { return m_enemy; } }
    public int remaining;
}
