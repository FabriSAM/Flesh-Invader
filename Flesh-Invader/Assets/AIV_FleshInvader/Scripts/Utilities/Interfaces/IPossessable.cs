using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPossessable
{
    public EnemyInfo CharacterInfo { get; }

    public bool UnPossessable { get; set; }

    public void Possess();
    public void UnPossess();
}
