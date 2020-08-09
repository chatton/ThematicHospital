using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject highlightTile;

    private MeshRenderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        highlightTile.SetActive(true);
    }

    public void UpdateMaterial(Material material)
    {
        _renderer.material = material;
    }

    public void Deselect()
    {
        highlightTile.SetActive(false);
    }
}