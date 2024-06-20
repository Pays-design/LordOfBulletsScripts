using System.Collections.Generic;
using UnityEngine;

namespace LordOfBullets.Core
{
    public class PlayerGun : Gun
    {
        public PlayerBullet Shoot(LinkedList<Vector3> path, PlayerBullet bullet) 
        {
            PlayerBullet bulletToShoot = MakeBullet(bullet) as PlayerBullet;

            bulletToShoot.StartFlightOnTrajectory(path, m_shootingAcceleration);

            return bulletToShoot;
        }
    }
}