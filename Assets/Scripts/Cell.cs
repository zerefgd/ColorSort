using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [HideInInspector] public Color Color;
    [HideInInspector] public Vector2Int Position;

    public bool IsStartTweenPlaying => startAnimation.IsActive();
    public bool IsStartMovePlaying => startMoveAnimation.IsActive();
    public bool hasSelectedMoveFinished => !selectedMoveAnimation.IsActive();
    public bool hasMoveFinished => !moveAnimation.IsActive();

    [SerializeField] private SpriteRenderer _bgSprite;
    [SerializeField] private float _startScaleDelay = 0.04f;
    [SerializeField] private float _startScaleTime = 0.2f;
    [SerializeField] private float _startMoveAnimationTime = 0.32f;
    [SerializeField] private float _selectedMoveAnimationTime = 0.16f;
    [SerializeField] private float _moveAnimationTime = 0.32f;

    private Tween startAnimation;
    private Tween startMoveAnimation;
    private Tween selectedMoveAnimation;
    private Tween moveAnimation;

    private const int FRONT = 1;
    private const int BACK = 0;

    public void Init(Color color, int x, int y)
    {
        Color = color;
        _bgSprite.color = Color;
        Position = new Vector2Int(x, y);
        transform.localPosition = new Vector3(x, y, 0);
        transform.localScale = Vector3.zero;
        float delay = (x + y) * _startScaleDelay;
        startAnimation = transform.DOScale(1f, _startScaleTime);
        startAnimation.SetEase(Ease.OutExpo);
        startAnimation.SetDelay(0.5f + delay);
        startAnimation.Play();
    }

    public void GameFinished()
    {
        transform.localScale = Vector3.one;
        float delay = (Position.x + Position.y) * _startScaleDelay;
        startAnimation = transform.DOScale(0.5f, _startScaleTime);
        startAnimation.SetLoops(2, LoopType.Yoyo);
        startAnimation.SetEase(Ease.InOutExpo);
        startAnimation.SetDelay(0.5f + delay);
        startAnimation.Play();
    }

    public void AnimateStartPosition()
    {
        startMoveAnimation = transform.DOLocalMove(
            new Vector3(Position.x, Position.y, 0), _startMoveAnimationTime);
        startMoveAnimation.SetEase(Ease.InSine);
        startMoveAnimation.Play();
    }

    public void SelectedMoveStart()
    {
        _bgSprite.sortingOrder = FRONT;
        transform.localScale = Vector3.one * 1.2f;
    }

    public void SelectedMove(Vector2 offset)
    {
        transform.localPosition = Position + offset;
        float minX = 0f;
        float maxX = GameManager.Cols - 1;
        float minY = 0f;
        float maxY = GameManager.Rows - 1;
        Vector2 pos = transform.localPosition;
        if (pos.x < minX)
        {
            pos.x = minX;
        }
        if (pos.x > maxX)
        {
            pos.x = maxX;
        }
        if (pos.y < minY)
        {
            pos.y = minY;
        }
        if (pos.y > maxY)
        {
            pos.y = maxY;
        }
        transform.localPosition = pos;
    }

    public void SelectedMoveEnd()
    {
        selectedMoveAnimation = transform.DOLocalMove(
            new Vector3(Position.x, Position.y, 0f),
            _selectedMoveAnimationTime
            );
        selectedMoveAnimation.onComplete = () =>
        {
            _bgSprite.sortingOrder = BACK;
            transform.localScale = Vector3.one;
        };
        selectedMoveAnimation.Play();
    }

    public void MoveEnd()
    {
        _bgSprite.sortingOrder = FRONT;
        moveAnimation = transform.DOLocalMove(
            new Vector3(Position.x, Position.y, 0f),
            _moveAnimationTime
            );
        moveAnimation.onComplete = () =>
        {
            _bgSprite.sortingOrder = BACK;
        };
        moveAnimation.Play();
    }
}
