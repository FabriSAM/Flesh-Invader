using System;

[Serializable]
public struct Statistics
{
    private float gameTime;
    private CollectiblesFound collectiblesFound;
    private int possessionSuccess;
    private int possessionFailed;
    private int bulletFired;

    public float GameTime { get; set; }
    public CollectiblesFound CollectiblesFound { get; set; }
    public int PossessionSuccess { get; set; }
    public int PossessionFailed { get; set; }
    public int BulletFired { get; set; }

}