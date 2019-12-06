using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private float sightRange = 10;
    [SerializeField] private Transform playerTransform;
    private Transform _enemyTransform;

    void Awake()
    {
        _enemyTransform = transform;
    }

    private void Update()
    {
        IsLookingThePlayer(playerTransform.position);
    }

    private bool IsLookingThePlayer(Vector3 playerPosition)
    {
        var displacement = playerPosition - _enemyTransform.position;
        var distanceToPlayer = displacement.magnitude;
        if (distanceToPlayer <= sightRange)
        {
            var dot = Vector3.Dot(_enemyTransform.forward, displacement.normalized);

            if (!(dot >= 0.5)) return false;

            var layerMask = 1 << 2;

            layerMask = ~layerMask;

            if (Physics.Raycast(_enemyTransform.position, displacement.normalized, out var hit, sightRange, layerMask))
            {
                Debug.DrawRay(_enemyTransform.position, displacement.normalized * hit.distance, Color.red);
                Debug.Log("Did Hit");

                if (!hit.collider.GetComponent<Player>()) return false;

                Debug.DrawRay(_enemyTransform.position, displacement.normalized * hit.distance, Color.green);
                return true;

            }
            return false;
        }
        return true;
    }

}
