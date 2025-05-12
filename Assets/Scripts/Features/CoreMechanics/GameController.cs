using TMPro;
using UnityEngine;
using UnityEngine.UI; 

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _statusText; 
    [SerializeField] private Button _restartButton; 
    [SerializeField] private BoardManager _boardManager;
    [SerializeField] private KnightController _knightController;
    private int _totalCells;

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (_boardManager == null) Debug.LogError("BoardManager not found!");
        if (_knightController == null) Debug.LogError("KnightController not found!");
        
        _restartButton.onClick.AddListener(RestartGame);
    }

    private void Start()
    {
        StartNewGame();
    }

    private void StartNewGame()
    {
        IsGameOver = false;
        var boardSize = _boardManager.GetBoardSize();
        _totalCells = boardSize * boardSize;
        _boardManager.CreateBoard(); 
        _knightController.InitializeKnight(); 
        _statusText.text = "Click on any cell to place a knight.";
    }

    public void UpdateMoveCount(int movesMade)
    {
        if (IsGameOver) return;
        _statusText.text = $"Moves: {movesMade}/{_totalCells}";
    }

    public void WinGame()
    {
        IsGameOver = true;
        _statusText.text = "VICTORY! All cells are closed!";
        _boardManager.ClearHighlights(); 
    }

    public void LoseGame()
    {
        IsGameOver = true;
        _statusText.text = "DEFEAT! No moves available.";
        _boardManager.ClearHighlights();
    }

    private void RestartGame()
    {
        _boardManager.ResetBoard();
        _knightController.ResetKnight();
        IsGameOver = false;
        _statusText.text = "Click on any cell to place a knight.";
    }
}