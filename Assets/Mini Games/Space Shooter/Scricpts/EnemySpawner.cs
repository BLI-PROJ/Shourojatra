using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<EnemyWaveConfig> enemyWaveConfigs;
    [SerializeField] bool looping = false;
    [SerializeField] int startingWave = 0;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < enemyWaveConfigs.Count; waveIndex++)
        {
            var currentWave = enemyWaveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnimiesInWave(currentWave));
        }
    }

    private IEnumerator SpawnAllEnimiesInWave(EnemyWaveConfig enemyWaveConfig)
    {
        for (int i = 0; i < enemyWaveConfig.GetNumberOfEnimies(); i++)
        {
            var newEnemy = Instantiate(enemyWaveConfig.GetEnemyPrefab(),
            enemyWaveConfig.GetWayPoints()[0].transform.position,
            Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(enemyWaveConfig);
            yield return new WaitForSeconds(enemyWaveConfig.GetTimeBetweenSpawn());
        }

    }
}
