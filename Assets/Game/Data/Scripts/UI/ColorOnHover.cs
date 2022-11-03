using System.Linq;
using UnityEngine;

/* Tint the object when hovered. */

public class ColorOnHover : MonoBehaviour
{
    public Color color;
    public Renderer meshRenderer;

    private Color[] originalColours;

    private void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        originalColours = meshRenderer.materials.Select(x => x.color).ToArray();
    }

    private void OnMouseEnter()
    {
        foreach (Material mat in meshRenderer.materials)
        {
            mat.color *= color;
        }
    }

    private void OnMouseExit()
    {
        for (int i = 0; i < originalColours.Length; i++)
        {
            meshRenderer.materials[i].color = originalColours[i];
        }
    }
}