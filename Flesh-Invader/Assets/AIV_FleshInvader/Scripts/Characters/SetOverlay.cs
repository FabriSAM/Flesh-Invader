using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem.XR;

public class SetOverlay : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer mesh;


    public void AddOverlay(Material materialToAdd)
    {
        List<Material> unpossessableMaterials = mesh.materials.ToList();
        unpossessableMaterials.Add(materialToAdd);
        mesh.SetMaterials(unpossessableMaterials);
    }

    public void RemoveOverlay(Material materialToRemove)
    {
        List<Material> unpossessableMaterials = mesh.materials.ToList();

        if (unpossessableMaterials.Count == 1) return;

        foreach (Material material in unpossessableMaterials)
        {
            string materialName = material.name.Replace(" (Instance)", "");
            if (materialName == materialToRemove.name)
            {
                unpossessableMaterials.Remove(material);
                mesh.SetMaterials(unpossessableMaterials);
                break;
            }
        }
    }
}
