using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPossessable
{
    public EnemyInfo CharacterInfo { get; }

    public void Possess();
    public void UnPossess();
}
