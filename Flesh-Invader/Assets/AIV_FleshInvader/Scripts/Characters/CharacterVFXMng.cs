using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct VFXIndexer
{
    [SerializeField] 
    public string VFXName;
    [SerializeField] 
    public ParticleSystem VFXparticleSystem;

    public VFXIndexer(string vFXName, ParticleSystem vFXparticleSystem)
    {
        VFXName = vFXName;
        VFXparticleSystem = vFXparticleSystem;
    }
}


public class CharacterVFXMng : MonoBehaviour
{
    [SerializeField] List<VFXIndexer> visualEffects;

    public void ActivateEffect(string vfxName)
    {
        foreach (VFXIndexer effect in visualEffects)
        {
            if(effect.VFXName == vfxName)
            {
                effect.VFXparticleSystem.Play();
            }
        }
    }

    public void StopEffect(string vfxName)
    {
        foreach (VFXIndexer effect in visualEffects)
        {
            if (effect.VFXName == vfxName)
            {
                effect.VFXparticleSystem.Stop();
            }
        }
    }
}
