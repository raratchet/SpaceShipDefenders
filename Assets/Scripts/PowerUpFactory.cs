using UnityEngine;

public enum PowerUpType {
    Repair, QuickShot, StunShot
}

public class PowerUpFactory : MonoBehaviour
{
    List<PowerUp> powerUpPool = new List<PowerUp>();

    public void InstantiatePowerUp(PowerUpType type, Vector3 position)
    {

    }
}
