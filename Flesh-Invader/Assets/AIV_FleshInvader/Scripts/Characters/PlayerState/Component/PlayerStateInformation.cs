using NotserializableEventManager;
using UnityEngine;

public class PlayerStateInformation : MonoBehaviour
{
    Statistics stats = new Statistics();
    // Start is called before the first frame update

    //private int totalPossessionBullet = 0;


    #region PublicMethods
    public void OpenPauseMenu()
    {
        GlobalEventSystem.CastEvent(EventName.PauseMenuEvent, EventArgsFactory.PauseMenuEventFactory(GetStats()));
    }

    public Statistics GetStats()
    {
        stats.GameTime = Time.timeSinceLevelLoad;
        stats.CollectiblesFound = PlayerState.Get().MissionController.Collectible.collectiblesFound;
        stats.PossessionFailed = stats.TotalPossessionBullet - stats.PossessionSuccess;
        return stats;
    }

    public void SetStats(Statistics newStats)
    {
        stats = newStats;
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
        stats.TotalPossessionBullet++;
    }
    public void SetCurrentIndexEnemy(int enemyIndex)
    {
        stats.CurrentIndexEnemy = enemyIndex;
    }
    #endregion
}
