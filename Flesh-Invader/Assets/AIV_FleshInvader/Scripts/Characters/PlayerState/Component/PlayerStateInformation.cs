using NotserializableEventManager;
using UnityEngine;

public class PlayerStateInformation : MonoBehaviour
{
    Statistics stats = new Statistics();
    // Start is called before the first frame update

    private int totalPossessionBullet = 0;


    #region PublicMethods
    public void OpenPauseMenu()
    {
        GlobalEventSystem.CastEvent(EventName.PauseMenuEvent, EventArgsFactory.PauseMenuEventFactory(GetStats()));
    }

    public Statistics GetStats()
    {
        stats.GameTime = Time.time;
        stats.CollectiblesFound = PlayerState.Get().MissionController.Collectible.collectiblesFound;
        stats.PossessionFailed = totalPossessionBullet - stats.PossessionSuccess;
        return stats;
    }

    public void BulletFired()
    {
        stats.BulletFired++;
    }
    public void PossessionSuccess()
    {
        stats.PossessionSuccess++;
    }
    public void PossessionShoot()
    {
        totalPossessionBullet++;
    }
    public void SetCurrentIndexEnemy(int enemyIndex)
    {
        stats.CurrentIndexEnemy = enemyIndex;
    }
    #endregion
}
