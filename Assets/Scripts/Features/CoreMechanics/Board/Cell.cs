using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class Cell : MonoBehaviour, IPointerDownHandler
{
    public bool IsVisited { get; private set; }
    public bool IsPossibleMove { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private BoardManager _boardManager; 
    
    private Color _baseColorLight;
    private Color _baseColorDark;
    private Color _visitedColor;
    private Color _highlightColor;
    private Color _currentBaseColor; 
    
    private int _positionX;
    private int _positionY;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(int x, int y, BoardManager boardManager, Color light, Color dark, 
        Color visited, Color highlight)
    {
        _positionX = x;
        _positionY = y;
        _boardManager = boardManager;
        IsVisited = false;
        IsPossibleMove = false;

        _baseColorLight = light;
        _baseColorDark = dark;
        _visitedColor = visited;
        _highlightColor = highlight;

        _currentBaseColor = (x + y) % 2 == 0 ? _baseColorDark : _baseColorLight;
        _spriteRenderer.color = _currentBaseColor;
    }

    public void SetVisited()
    {
        IsVisited = true;
        IsPossibleMove = false; 
        _spriteRenderer.color = _visitedColor;
        _currentBaseColor = _visitedColor; 
    }

    public void HighlightAsPossibleMove(bool highlight)
    {
        if (IsVisited) return; 

        IsPossibleMove = highlight;
        _spriteRenderer.color = highlight ? _highlightColor : _currentBaseColor;
    }

    public void ResetCell()
    {
        IsVisited = false;
        IsPossibleMove = false;
        _currentBaseColor = (_positionX + _positionY) % 2 == 0 ? _baseColorDark : _baseColorLight;
        _spriteRenderer.color = _currentBaseColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _boardManager.OnCellClicked(this);
    }

    public int GetPositionX()
    {
        return _positionX;
    }
    
    public int GetPositionY()
    {
        return _positionY;
    }
}