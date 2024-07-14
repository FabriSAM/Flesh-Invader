using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectable
{
    uint CollectibleID { get; }
    void AddMission();
    void Collect();
}
