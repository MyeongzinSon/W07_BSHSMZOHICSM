using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerMove move;
    PlayerRoll roll;
    ShurikenShooter attack;

    [SerializeField] Transform currnetTarget;
    [SerializeField] float targetPositionOffset;

    private void Awake()
    {
        TryGetComponent(out move);
        TryGetComponent(out roll);
        TryGetComponent(out attack);
    }

    private void Update()
    {
        if (currnetTarget != null)
        {
            var diff = currnetTarget.position - transform.position;
            move.direction = diff.magnitude > targetPositionOffset ? diff : Vector3.zero;
        }
    }

}
