using UnityEngine;
using System.Collections.Generic;
using System;

public class MoveEnvironmentPiece1 : MonoBehaviour
{
    public event Action OnMovePiece;

    [SerializeField] Dialogue dialogue;

    [SerializeField] Vector3 Position;
    [SerializeField] Vector3 NewPosition;

    private void OnEnable()
    {
        dialogue.OnDialogueDone += MovePiece;
    }
    private void OnDisable()
    {
        dialogue.OnDialogueDone -= MovePiece;
    }

    private void MovePiece()
    {
        if (NewPosition != transform.position) transform.position = NewPosition;
    }
}