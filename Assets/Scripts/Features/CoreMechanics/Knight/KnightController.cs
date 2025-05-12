using UnityEngine;
using System.Collections.Generic;

public class KnightController : MonoBehaviour
{
    [SerializeField] private GameObject _knightPrefab;
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private GameController _gameManager;
    
    private GameObject _knightInstance;
    private Vector2Int _currentPosition;
    
    private bool _isKnightPlaced = false;
    private int _movesMade = 0;
    private int _boardSize;
    
    private void Awake()
    {
        if (_boardManager == null) Debug.LogError("BoardManager not found!");
        if (_gameManager == null) Debug.LogError("GameController not found!");
        
        _boardSize = _boardManager.GetBoardSize();
    }
    
    // Possible displacements for the knight's move
    private readonly Vector2Int[] _knightMoves = new Vector2Int[]
    {
        new Vector2Int(1, 2), new Vector2Int(2, 1),
        new Vector2Int(2, -1), new Vector2Int(1, -2),
        new Vector2Int(-1, -2), new Vector2Int(-2, -1),
        new Vector2Int(-2, 1), new Vector2Int(-1, 2)
    };
    
    public void InitializeKnight()
    {
        if (_knightInstance != null) Destroy(_knightInstance);

        var boardHolder = _boardManager.GetBoardHoalder();
        _knightInstance = Instantiate(_knightPrefab, boardHolder); 
        _knightInstance.SetActive(false); 
        _isKnightPlaced = false;
        _movesMade = 0;
    }

    public void HandleCellClick(Cell clickedCell)
    {
        if (!_isKnightPlaced) 
        {
            PlaceKnight(clickedCell.GetPositionX(), clickedCell.GetPositionY());
        }
        else if (clickedCell.IsPossibleMove && !clickedCell.IsVisited) 
        {
            MoveKnight(clickedCell.GetPositionX(), clickedCell.GetPositionY());
        }
    }

    private void PlaceKnight(int x, int y)
    {
        _currentPosition = new Vector2Int(x, y);
        _knightInstance.transform.position = _boardManager.GetWorldPosition(x, y);
        _knightInstance.SetActive(true);
        _isKnightPlaced = true;
        _movesMade = 1;

        Cell startingCell = _boardManager.GetCell(x, y);
        startingCell.SetVisited();
        _gameManager.UpdateMoveCount(_movesMade);

        ShowPossibleMoves();
    }

    private void MoveKnight(int x, int y)
    {
        _currentPosition = new Vector2Int(x, y);
        _knightInstance.transform.position = _boardManager.GetWorldPosition(x, y);
        _movesMade++;

        Cell targetCell = _boardManager.GetCell(x, y);
        targetCell.SetVisited();
        _gameManager.UpdateMoveCount(_movesMade);
        _boardManager.ClearHighlights(); 

        if (_movesMade == _boardSize * _boardSize)
        {
            _gameManager.WinGame();
            return;
        }

        List<Vector2Int> possibleMoves = GetPossibleMoves(_currentPosition);
        
        if (possibleMoves.Count == 0)
        {
            _gameManager.LoseGame();
        }
        else
        {
            _boardManager.HighlightPossibleMoves(possibleMoves);
        }
    }

    private void ShowPossibleMoves()
    {
        List<Vector2Int> possibleMoves = GetPossibleMoves(_currentPosition);
        
        if (possibleMoves.Count == 0 && _movesMade > 0 && 
            _movesMade < _boardSize * _boardSize) 
        {
             _gameManager.LoseGame(); 
        }
        else
        {
            _boardManager.HighlightPossibleMoves(possibleMoves);
        }
    }

    private List<Vector2Int> GetPossibleMoves(Vector2Int fromPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        
        foreach (Vector2Int offset in _knightMoves)
        {
            Vector2Int targetPos = fromPosition + offset;
            Cell targetCell = _boardManager.GetCell(targetPos.x, targetPos.y);
            
            if (targetCell != null && !targetCell.IsVisited)
            {
                moves.Add(targetPos);
            }
        }
        
        return moves;
    }

    public void ResetKnight()
    {
        if(_knightInstance != null) _knightInstance.SetActive(false);
        _isKnightPlaced = false;
        _movesMade = 0;
        _boardManager.ClearHighlights();
    }
}