using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    LaserMinigame _laserMinigame;

    [SerializeField] GameObject _asteroidPrefab;
    public List<GameObject> _asteroidPool = new List<GameObject>();
    public int _poolSize = 8;

    Vector2 _previousPos;
    int _positionsGenerated = 0;
    int _currentAsteroids = 0;


    private void Awake()
    {
        _laserMinigame = FindObjectOfType<LaserMinigame>();

        //Generating AsteroidPool
        for (int i = 0; i < _poolSize; i++)
        {
            _asteroidPool.Add(Instantiate(_asteroidPrefab));
            _asteroidPool[i].SetActive(false);
        }
    }

    public bool SpawnAsteroid(int num)
    {
        if (_asteroidPool.Count < num) return false;

        for (int i = 0; i < num; i++)
        {
            GetAsteroidFromPool();
        }
        _positionsGenerated = 0;
        return true;
    }

    public void GetAsteroidFromPool()
    {
        GameObject asteroid = _asteroidPool[_asteroidPool.Count - 1];
        asteroid.transform.position = RandomPosition();
        asteroid.transform.rotation = Quaternion.identity;
        asteroid.gameObject.SetActive(true);

        asteroid.GetComponent<Asteroid>().Launch();
        _asteroidPool.RemoveAt(_asteroidPool.Count - 1);
        _currentAsteroids++;
    }

    public void ReturnObjToPool(GameObject obj)
    {
        obj.SetActive(false);
        _asteroidPool.Add(obj);
        _currentAsteroids--;
        if (_currentAsteroids == 0) _laserMinigame.EndMinigame();
    }

    //TODO MAke this Work
    public Vector2 RandomPosition()
    {
        bool posIsValid = false;
        Vector2 pos = Vector2.zero;

        while (!posIsValid)
        {
            float _randomX = Random.Range(transform.position.x - 4.5f, transform.position.x + 4.5f);
            float _randomY = Random.Range(transform.position.y - 1.75f, transform.position.y + 1.75f);
            pos = new Vector2(_randomX, _randomY);

            if (_positionsGenerated == 0) posIsValid = true;
            if ((_previousPos - pos).magnitude > 0.5f) posIsValid = true;
        }

        _previousPos = pos;
        return pos;
    }
}
