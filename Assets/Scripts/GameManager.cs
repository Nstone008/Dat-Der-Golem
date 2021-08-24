using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject player;
    public List<AI> enemies;
    public List<AI> allies;
    public List<SpawnPoint> enemySpawns;
    public List<SpawnPoint> allySpawns;
    public SpawnPoint playerSpawn;
    public GameObject projectileHolder;

    public List<GameObject> enemyPrefabs;
    public List<GameObject> allyPrefabs;

    public bool startGame;
    public bool gameOver = false;

    public int level;


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (startGame)
        {
            if (enemies.Count == 0)
            {
                startGame = false;
            }
        }
        else
        {
            StartCoroutine(SetNewLevel());
        }
        
        if (!SoundManager.Instance.isIntro)
        {
            if (enemies.Count <=1 && !SoundManager.Instance.bgmAdditionalLayers[0].isPlaying)
            {
                SoundManager.Instance.bgmAdditionalLayers[0].time = SoundManager.Instance.GetBGMTimeIndex();
                SoundManager.Instance.bgmAdditionalLayers[0].Play();
            }
            else if (enemies.Count > 1)
            {
                SoundManager.Instance.bgmAdditionalLayers[0].Stop();
            }

            if (allies.Count <= 1 && !SoundManager.Instance.bgmAdditionalLayers[0].isPlaying)
            {
                SoundManager.Instance.bgmAdditionalLayers[1].time = SoundManager.Instance.GetBGMTimeIndex();
                SoundManager.Instance.bgmAdditionalLayers[1].Play();
            }
            else if (allies.Count > 1)
            {
                SoundManager.Instance.bgmAdditionalLayers[1].Stop();
            }
        }
    }

    public IEnumerator SetNewLevel()
    {
        level++;
        //ClearExistingSpawns();

        allies.Clear();
        enemies.Clear();
        if (player is null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        player.GetComponent<CharacterControl>().health = 100;

        int numEnemies = 0;
        int numAllies = 0;

        switch (level)
        {
            case 1:
                numEnemies = 2;
                numAllies = 2;
                ActivateSpawn(numEnemies, EntityType.Enemy);
                ActivateSpawn(numAllies, EntityType.Ally);

                foreach (SpawnPoint sp in enemySpawns.Where(x=>x.isSpawnEnabled).ToList())
                {
                    AI enemy = Instantiate(enemyPrefabs[0], sp.transform).GetComponent<AI>();
                    sp.spawnedEntity = enemy;
                    enemies.Add(enemy);
                }

                foreach (SpawnPoint sp in allySpawns.Where(x => x.isSpawnEnabled).ToList())
                {
                    AI ally = Instantiate(allyPrefabs[0], sp.transform).GetComponent<AI>();
                    sp.spawnedEntity = ally;

                    allies.Add(ally);
                }


                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;

        }
        startGame = true;
        yield return new WaitForSeconds(2);

        foreach(AI ai in enemies)
        {
            ai.canMove = true;
            ai.canAct = true;
        }

        foreach (AI ai in allies)
        {
            ai.canMove = true;
            ai.canAct = true;
        }

        Entity p = player.GetComponent<Entity>();
        p.canAct = true;
        p.canMove = true;
        
    }

    void ActivateSpawn(int numberofSpawns, EntityType type)
    {
        List<SpawnPoint> spawnlist = (type == EntityType.Enemy ? enemySpawns : allySpawns);

        for (int i = 0;  i < spawnlist.Count; i++)
        {
            spawnlist[i].isSpawnEnabled = (i < numberofSpawns);
        }
    }

}
