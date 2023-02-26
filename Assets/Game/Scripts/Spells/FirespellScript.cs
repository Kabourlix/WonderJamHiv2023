using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirespellScript : MonoBehaviour
{
    [SerializeField] private GameObject _childPrefab;
    [Header("Property of spawner")]
    [SerializeField] private float _minTimeBetweenSpawn;
    [SerializeField] private float _maxTimeBetweenSpawn;

    [SerializeField] private float _initialDelay = 3f;

    [SerializeField] private float _minSpawnDistance;
    [SerializeField] private float _maxSpawnDistance;
    [Header("The radius of the ring of fire, where the child will end its course")]
    [SerializeField] private float _spellRadius;

    [Header("Property of child")]
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _maxSpeed;

    private void Start()
    {
        StartCoroutine(TimedSpawn(_initialDelay));
    }

    private void Spawn()
    {
        //First we generate a random angle
        float angle = Random.Range(-Mathf.PI,Mathf.PI);
        float distance=Random.Range(_minSpawnDistance, _maxSpawnDistance);
        float timeUntilNextSpawn=Random.Range(_minTimeBetweenSpawn, _maxTimeBetweenSpawn);
        Vector3 spawnPos = transform.position;
        spawnPos += new Vector3(distance * Mathf.Cos(angle), 0, distance * Mathf.Sin(angle));
        Vector3 targetPos=transform.position;
        targetPos += new Vector3(_spellRadius * Mathf.Cos(angle), 0, _spellRadius * Mathf.Sin(angle));
        GameObject newGo = GameObject.Instantiate(_childPrefab);
        newGo.transform.position = spawnPos;
        AIChild aIChild=newGo.GetComponent<AIChild>();
        aIChild.TargetPosition = targetPos;
        aIChild.Speed=Random.Range(_minSpeed,_maxSpeed);
        StartCoroutine(TimedSpawn(timeUntilNextSpawn));
    }

    private IEnumerator TimedSpawn(float time)
    {
        yield return new WaitForSeconds(time);
        Spawn();
    }
}
