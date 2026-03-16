using UnityEngine;
using System.Collections.Generic;
using System;

public class MoveEnvironmentPiece : MonoBehaviour
{
    public event Action OnMovePiece;

    /// <summary>
    /// true for Start, false for End.
    /// </summary>
    [SerializeField] private bool EndOrStart;

    [SerializeField] Vector3 Position;
    [SerializeField] Vector3 NewPosition;

    private void OnEnable()
    {
        if (EndOrStart) FightManager.instance.OnFightStarted += MovePiece;
        else FightManager.instance.OnFightCompleted += MovePiece;
    }
    private void OnDisable()
    {
        if (EndOrStart) FightManager.instance.OnFightStarted -= MovePiece;
        else FightManager.instance.OnFightCompleted -= MovePiece;
    }

    private void MovePiece()
    {
        if (NewPosition != transform.position) transform.position = NewPosition;
    }
}