using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private static Vector3 Jab = new Vector3(3, 1, 0);
    private static Vector3 Cross = new Vector3(3, 1, 0);

    public event Action<Knockback> OnPlayerKnockback;

    Dictionary<PunchType, Vector3> AccessPunches = new Dictionary<PunchType, Vector3>();

    private void Awake()
    {
        AccessPunches.Add(PunchType.Jab, Jab);
        AccessPunches.Add(PunchType.Cross, Cross);
    }

    public void KnockbackPlayer(PunchType pt, float direction)
    {
        Vector3 input;
        if (!AccessPunches.TryGetValue((pt), out input)) Debug.Log("The KnockbackPlayer function in PlayerKnockBack got an invalid PunchType as input.");
        else
        {
            Knockback kb_struct = new Knockback() { velocity = input, time = 0.2f };
            kb_struct.velocity.x *= direction;
            OnPlayerKnockback.Invoke(kb_struct);
        }
    }

}
