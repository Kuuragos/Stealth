using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class EnemyIA : MonoBehaviour
{
    [SerializeField] private float sightRange = 10;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject detect=null;
    [SerializeField] private GameObject warning=null;
    private Transform enemyTransform;

    void Awake()
    {
        enemyTransform = transform;
    }

    private void Update()
    {
        DetectPlayer(IsLookingThePlayer(playerTransform.position));
    }

    private float timeOnSight;
    private void DetectPlayer(bool isLookingThePlayer)
    {
        if (isLookingThePlayer)
        {
            warning.SetActive(true);
            detect.SetActive(false);
            if (timeOnSight<=3)
            {
                timeOnSight += Time.deltaTime;
            }
            if (!(timeOnSight >= 3)) return;
            warning.SetActive(false);
            detect.SetActive(true);
            EditorSceneManager.LoadScene(0);
        }
        else
        {
            if (timeOnSight>0)
            {
                timeOnSight -= Time.deltaTime;
            }
            if (!(timeOnSight <= 0)) return;
            warning.SetActive(false);
            detect.SetActive(false);
        }
    }
    private bool IsLookingThePlayer(Vector3 playerPosition)
    {
        var displacement = playerPosition - enemyTransform.position;
        var distanceToPlayer = displacement.magnitude;
        if (distanceToPlayer <= sightRange)
        {
            var dot = Vector3.Dot(enemyTransform.forward, displacement.normalized);

            if (!(dot >= 0.5)) return false;

            var layerMask = 1 << 2;

            layerMask = ~layerMask;

            if (Physics.Raycast(enemyTransform.position, displacement.normalized, out var hit, sightRange, layerMask))
            {
                Debug.DrawRay(enemyTransform.position, displacement.normalized * hit.distance, Color.red);
                Debug.Log("Did Hit");

                if (!hit.collider.GetComponent<Player>()) return false;

                Debug.DrawRay(enemyTransform.position, displacement.normalized * hit.distance, Color.green);
                return true;

            }
        }
            return false;
    }

}
