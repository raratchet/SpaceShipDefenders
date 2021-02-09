using UnityEngine;

public class BulletFactory: MonoBehaviour
{
    public GameObject bulletBase;

    private List<Bullet> bulletPool = new List<Bullet>();

    public float defaultSpeed = 15;
    public float defaultDamage = 10;
    public int maxBulletCount = 100;


    public void PreInstantiateBullets(int quantity)
    {
        if(quantity > bulletPool.Size)
        {
            for(int i = 0; i < quantity - bulletPool.Size; i++)
            {
                Bullet instance = Instantiate(bulletBase).GetComponent<Bullet>();
                bulletPool.Push_back(instance);
                instance.gameObject.SetActive(false);
            }
        }
    }
    //TODO mas metodos para personalizar la bala
    public void InstantiateBullet(GameObject shooter, Vector3 position, Vector3 direction)
    {
        InstantiateBullet(shooter, position, direction, -1, -1);
    }

    public void InstantiateBullet(GameObject shooter, Vector3 position, Vector3 direction, float speed)
    {
        InstantiateBullet(shooter, position, direction, speed, -1);
    }

    public void InstantiateBullet(GameObject shooter, Vector3 position, Vector3 direction, float speed, float damage)
    {
        for (int i = 0; i < bulletPool.Size; i++)
        {
            var bulleto = bulletPool.Get_at(i);
            if (!bulleto.gameObject.activeInHierarchy)
            {
                bulleto.transform.position = position;
                bulleto.direction = direction;
                bulleto.shooter = shooter;
                bulleto.speed = (speed > 0) ? speed : defaultSpeed;
                bulleto.damage = (damage > 0) ? damage : defaultDamage;
                bulleto.gameObject.SetActive(true);
                return;
            }
        }
        if(maxBulletCount >= bulletPool.Size)
        {
            Bullet bullet = Instantiate(bulletBase).GetComponent<Bullet>();
            bullet.transform.position = position;
            bullet.direction = direction;
            bullet.shooter = shooter;
            bullet.speed = (speed > 0) ? speed : defaultSpeed;
            bullet.damage = (damage > 0) ? damage : defaultDamage;
            bulletPool.Push_back(bullet);
            return;
        }
    }
}
