using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject highlightTile;

    private void Awake()
    {
        Deselect();
    }

    public void Select()
    {
        highlightTile.SetActive(true);
    }

    public void Deselect()
    {
        highlightTile.SetActive(false);
    }
}