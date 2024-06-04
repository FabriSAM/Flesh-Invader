using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_ChangeMaterialAction : StateAction
{
    private SkinnedMeshRenderer testMesh;
    private Material prevMateiral;
    private Material testMaterial;
    public TEST_ChangeMaterialAction(SkinnedMeshRenderer meshToChange,Material material)
    {
       testMesh = meshToChange;
       prevMateiral = meshToChange.material;
       testMaterial = material;
    }

    public override void OnEnter()
    {
        testMesh.material = testMaterial;
    }

    public override void OnExit()
    {
        testMesh.material = prevMateiral;
    }
}
