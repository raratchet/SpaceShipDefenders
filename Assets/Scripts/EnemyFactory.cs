using UnityEngine;
using Photon.Pun;

public enum EnemyType
{
    Blob, Shell
}
public class EnemyFactory : MonoBehaviour
{
    public Enemy[] enemiesBase;
    public int maxEnemyCount;
    private List<Enemy> enemyPool = new List<Enemy>();
    private List<GameObject> spawners = new List<GameObject>();

    void Start()
    {
        foreach(var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            spawners.Push_back(spawner);
        }
    }

    public void PreInstantiateEnemies<Type>(int quantity) where Type : Enemy
    {
        if(quantity > enemyPool.Size)
        {
            var baseInstance = GetBaseInstance<Type>();

            for (int i = 0; i < quantity - enemyPool.Size; i++)
            {
                var instanceBlob = GameManager.isOnline 
                    ? PhotonNetwork.Instantiate(baseInstance.name, Vector3.zero, Quaternion.identity).GetComponent<Type>()
                    : Instantiate(baseInstance).GetComponent<Type>();
                instanceBlob.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnEnemyRandom(EnemyType type, float health, float range)
    {
        if(type.Equals(EnemyType.Blob))
        {
            SpawnEnemy<BlobEnemy>(health,range);
        }

        if(type.Equals(EnemyType.Shell))
        {
            SpawnEnemy<ShellEnemy>(health,range);
        }
    }

    public void SpawnRandomEnemy(float health, float range)
    {
        int random = Random.Range(0, 2);

        switch(random)
        {
            case 0:
                SpawnEnemyRandom(EnemyType.Blob, health, range);
                break;
            case 1:
                SpawnEnemyRandom(EnemyType.Shell, health, range);
                break;
        }
    }

    private Type GetBaseInstance<Type>() where Type: Enemy
    {
        foreach(var eBase in enemiesBase)
        {
            if(eBase.GetType() == typeof(Type))
            {
                return (Type)eBase;
            }
        }
        return null;
    }

    void SpawnEnemy<Type> (float health, float range) where Type : Enemy
    {
        for (int i = 0; i < enemyPool.Size; i++)
        {
            var blob = enemyPool.Get_at(i);
            if (blob.GetType() == typeof(Type))
            {
                if (!blob.gameObject.activeInHierarchy)
                {
                    int index = Random.Range(0, spawners.Size);
                    var spawner = spawners.Get_at(index);
                    blob.gameObject.SetActive(true);
                    blob.transform.position = spawner.transform.position;
                    blob.health = (health > 0) ? health : blob.baseHealth;
                    blob.range = (range > 0) ? range : blob.baseRange;
                    return;
                }
            }
        }
        if (maxEnemyCount >= enemyPool.Size)
        {
            var blobo = GameManager.isOnline 
                ? PhotonNetwork.Instantiate(GetBaseInstance<Type>().name, Vector3.zero, Quaternion.identity).GetComponent<Type>()
                : Instantiate(GetBaseInstance<Type>()).GetComponent<Type>(); ; 
            int indexo = Random.Range(0, spawners.Size);
            var spawnero = spawners.Get_at(indexo);
            blobo.transform.position = spawnero.transform.position;
            blobo.health = (health > 0) ? health : blobo.baseHealth;
            blobo.range = (range > 0) ? range : blobo.baseRange;
            enemyPool.Push_back(blobo);
            return;
        }
    }


}
