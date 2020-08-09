using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hospital.Construction
{
    public class HospitalBuilder : MonoBehaviour
    {
        [SerializeField] private Material selectedMaterial;

        private Vector2 _startPos;
        private Camera _camera;

        private Tile _startTile;
        private Tile _currTile;

        private Tile[,] _tileArray;
        private HashSet<Tile> _tilesSelected;
        private HashSet<Tile> _allTiles;

        private void Awake()
        {
            _camera = Camera.main;
            _tilesSelected = new HashSet<Tile>();
            _allTiles = new HashSet<Tile>();

            Tile[] tiles = FindObjectsOfType<Tile>();
            Array.ForEach(tiles, t => _allTiles.Add(t));
            _tileArray = BuildTilesArray(_allTiles);
        }

        private static Tile[,] BuildTilesArray(IEnumerable<Tile> tiles)
        {
            Tile[,] arr = new Tile[10, 10];

            foreach (Tile t in tiles)
            {
                Vector3 pos = t.transform.position;
                arr[(int) pos.x, (int) pos.z] = t;
            }

            return arr;
        }

        private void Update()
        {
            Debug.Log("HeRE");
            if (Input.GetMouseButtonDown(0))
            {
                _startTile = GetTileMouseIsOver();
            }
            else if (Input.GetMouseButton(0))
            {
                UpdateSelectedTiles();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                DeselectTiles();
            }
        }

        private void DeselectTiles()
        {
            foreach (Tile t in _tilesSelected)
            {
                t.Deselect();
                t.UpdateMaterial(selectedMaterial);
            }

            _tilesSelected.Clear();
        }

        private Tile GetTileMouseIsOver()
        {
            Vector2 mousePos = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return hit.collider.GetComponentInParent<Tile>();
            }

            return null;
        }


        private void UpdateSelectedTiles()
        {
            _currTile = GetTileMouseIsOver();
            if (_currTile == null)
            {
                return;
            }

            int startX = (int) _startTile.transform.position.x;
            int startZ = (int) _startTile.transform.position.z;

            int endX = (int) _currTile.transform.position.x;
            int endZ = (int) _currTile.transform.position.z;

            HashSet<Tile> tilesThatWereSelected = HighlightTilesInRange(startX, endX, startZ, endZ);
            foreach (Tile t in _allTiles)
            {
                if (!tilesThatWereSelected.Contains(t))
                {
                    t.Deselect();
                }
            }
        }

        private HashSet<Tile> HighlightTilesInRange(int startX, int endX, int startZ, int endZ)
        {
            _tilesSelected.Clear();
            if (startX < endX)
            {
                for (int i = startX; i <= endX; i++)
                {
                    if (startZ < endZ)
                    {
                        for (int j = startZ; j <= endZ; j++)
                        {
                            Tile t = _tileArray[i, j];
                            t.Select();
                            _tilesSelected.Add(t);
                        }
                    }
                    else
                    {
                        for (int j = endZ; j <= startZ; j++)
                        {
                            Tile t = _tileArray[i, j];
                            t.Select();
                            _tilesSelected.Add(t);
                        }
                    }
                }
            }
            else
            {
                for (int i = endX; i <= startX; i++)
                {
                    if (startZ < endZ)
                    {
                        for (int j = startZ; j <= endZ; j++)
                        {
                            Tile t = _tileArray[i, j];
                            t.Select();
                            _tilesSelected.Add(t);
                        }
                    }
                    else
                    {
                        for (int j = endZ; j <= startZ; j++)
                        {
                            Tile t = _tileArray[i, j];
                            t.Select();
                            _tilesSelected.Add(t);
                        }
                    }
                }
            }

            return _tilesSelected;
        }
    }
}