using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hospital.Construction
{
    public class HospitalBuilder : MonoBehaviour
    {
        [SerializeField] public Material selectedGroundMaterial;
        [SerializeField] private RectTransform selectionBox;


        private Vector2 _startPos;
        private Camera _camera;
        private Tile[] _tiles;


        private void Awake()
        {
            _camera = Camera.main;
            _tiles = FindObjectsOfType<Tile>();
            Debug.Log("Tile: " + _tiles.Length);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                UpdateSelectionBox(Input.mousePosition);
                UpdateSelectedTiles();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ReleaseSelectionBox();
            }
        }

        private void UpdateSelectedTiles()
        {
            // bottom left of selection box
            Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
            // top left
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

            foreach (Tile t in _tiles)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(t.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    t.Select();
                }
                else
                {
                    t.Deselect();
                }
            }
        }

        private void UpdateSelectionBox(Vector2 currentMousePos)
        {
            selectionBox.gameObject.SetActive(true);

            float width = currentMousePos.x - _startPos.x;
            float height = currentMousePos.y - _startPos.y;

            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = _startPos + new Vector2(width / 2, height / 2);
        }

        private void ReleaseSelectionBox()
        {
            selectionBox.gameObject.SetActive(false);

            // bottom left of selection box
            Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
            // top left
            Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

            foreach (Tile t in _tiles)
            {
                Vector3 screenPos = _camera.WorldToScreenPoint(t.transform.position);
                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    t.GetComponentInChildren<MeshRenderer>().material = selectedGroundMaterial;
                    t.Deselect();
                }
            }
        }
    }

}