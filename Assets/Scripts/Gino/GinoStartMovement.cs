using System;
using UnityEngine;

public class GinoStartMovement : MonoBehaviour
{
    public event Action OnGinoStartMovement;

    protected void GSMInvoke()
    {
        OnGinoStartMovement?.Invoke();
    }
}
