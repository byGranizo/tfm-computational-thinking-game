using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private MeshRenderer tileMeshRenderer;

    private TileType tileType;

    private bool isGrabbed = false;

    public TileType TileType
    {
        get { return tileType; }
    }

    private GameManager gameManager;

    void Awake()
    {
        tileMeshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (isGrabbed)
        {
            Vector3 mousePosition = Mouse.GetWorldPosition(5);
            transform.position = new Vector3(mousePosition.x, 5, mousePosition.z);

            if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1))
            {
                transform.Rotate(0, 60, 0);
                tileType.RotateAntiClockwise();
            }
        }
    }

    public void SetTileType(TileType tileType)
    {
        Material[] materials = tileMeshRenderer.materials;
        materials[1] = new Material(tileType.Material);
        tileMeshRenderer.materials = materials;

        this.tileType = Instantiate(tileType);
    }

    public void Grab()
    {
        isGrabbed = true;
    }

    private void OnMouseDown()
    {
        if (isGrabbed)
        {
            HandlePlacing();
        }
    }


    private void HandlePlacing()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent<CellCollider>(out var cellCollider))
            {
                GameObject cell = cellCollider.gameObject.transform.parent.gameObject;

                HexagonalCell hexagonalCell = cell.GetComponent<HexagonalCell>();

                bool canPlace = hexagonalCell.CanPlaceTile(tileType);
                if (!canPlace) return;

                this.transform.parent = cell.transform;
                this.transform.position = cell.transform.position;

                hexagonalCell.PlaceTile(this);
                isGrabbed = false;

                //End turn and start next
            }
        }

    }
}
