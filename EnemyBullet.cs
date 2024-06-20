using UnityEngine;

namespace LordOfBullets.Core
{
    public class EnemyBullet : Bullet
    {
        private void OnTriggerEnter(Collider other)
        {
            Player possiblePlayer = other.gameObject.GetComponent<Player>();

            if (possiblePlayer != null) 
            {
                possiblePlayer.GetDamage(1);
            }
        }
    }
}