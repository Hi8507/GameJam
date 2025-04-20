using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public Vector3 endMovePosition;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOLocalMove(endMovePosition, time).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }   
}
