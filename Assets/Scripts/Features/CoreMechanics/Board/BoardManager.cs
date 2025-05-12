using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private Transform _boardHolder; 
    [SerializeField] private int _boardSize = 8;
    [SerializeField] private float _cellSize = 1.0f; 
    
    [SerializeField] private Color _colorLight = Color.white;
    [SerializeField] private Color _colorDark = new Color(0.8f, 0.8f, 0.8f);
    [SerializeField] private Color _colorVisited = Color.gray;
    [SerializeField] private Color _colorHighlight = Color.green;
    
    [SerializeField] private KnightController _knightController;
    [SerializeField] private GameController _gameManager;
    
    private Cell[,] _cells;

    private void Awake()
    {
        if (_knightController == null) Debug.LogError("KnightController not found!");
        if (_gameManager == null) Debug.LogError("GameManager not found!");
    }

    public void CreateBoard()
    {
        if (_cells != null)
        {
            foreach (Cell cell in _cells)
            {
                if (cell != null) Destroy(cell.gameObject);
            }
        }

        _cells = new Cell[_boardSize, _boardSize];
        
        if (_boardHolder == null)
        {
            _boardHolder = new GameObject("BoardHolder").transform;
            _boardHolder.SetParent(this.transform);
        }
        else
        {
            foreach (Transform child in _boardHolder)
            {
                Destroy(child.gameObject);
            }
        }
        
        for (int x = 0; x < _boardSize; x++)
        {
            for (int y = 0; y < _boardSize; y++)
            {
                GameObject cellGO = Instantiate(_cellPrefab, _boardHolder);
                cellGO.transform.position = GetWorldPosition(x, y);
                Cell cellComponent = cellGO.GetComponent<Cell>();
                cellComponent.Initialize(x, y, this, _colorLight, _colorDark, _colorVisited, _colorHighlight);
                _cells[x, y] = cellComponent;
            }
        }
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        float boardOffset = (_boardSize - 1) * _cellSize / 2.0f;
        return new Vector2(x * _cellSize - boardOffset, y * _cellSize - boardOffset);
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < _boardSize && y >= 0 && y < _boardSize)
        {
            return _cells[x, y];
        }
        return null;
    }

    public void OnCellClicked(Cell clickedCell)
    {
        if (_gameManager.IsGameOver) return;

        _knightController.HandleCellClick(clickedCell);
    }

    public void HighlightPossibleMoves(List<Vector2Int> moves)
    {
        ClearHighlights(); 
        
        foreach (Vector2Int move in moves)
        {
            Cell cell = GetCell(move.x, move.y);
            
            if (cell != null && !cell.IsVisited)
            {
                cell.HighlightAsPossibleMove(true);
            }
        }
    }

    public void ClearHighlights()
    {
        foreach (Cell cell in _cells)
        {
            if (cell != null)
            {
                cell.HighlightAsPossibleMove(false); 
            }
        }
    }

    public void ResetBoard()
    {
        foreach (Cell cell in _cells)
        {
            if (cell != null) cell.ResetCell();
        }
    }

    public int GetBoardSize()
    {
        return _boardSize;
    }
    
    public Transform GetBoardHoalder()
    {
        return _boardHolder;
    }
}