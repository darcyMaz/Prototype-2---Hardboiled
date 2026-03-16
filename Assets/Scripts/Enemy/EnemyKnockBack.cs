using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockBack : MonoBehaviour
{
    private static Vector3 Jab = new Vector3(8, 1, 0);
    private static Vector3 Cross = new Vector3(9, 2, 0);
    private static Vector3 Hook = new Vector3(6, 6, 0);

    Dictionary<PunchType, Vector3> AccessPunches = new Dictionary<PunchType, Vector3>();

    public event Action <Knockback> OnEnemyKnockback;

    private void Awake()
    {
        AccessPunches.Add(PunchType.Jab, Jab);
        AccessPunches.Add(PunchType.Cross, Cross);
        AccessPunches.Add(PunchType.Hook, Hook);
    }

    public void KnockbackEnemy(PunchType pt, float direction)
    {
        Vector3 input;
        if (!AccessPunches.TryGetValue((pt), out input)) Debug.Log("The KnockbackEnemy function in EnemyKnockBack got an invalid PunchType as input.");
        else
        {
            Knockback kb_struct = new Knockback() { velocity = input, time = 0.4f };
            kb_struct.velocity.x *= direction;
            OnEnemyKnockback.Invoke(kb_struct);
        }
    }
}

public struct Knockback
{ 
    public Vector3 velocity;
    public float time;
}

