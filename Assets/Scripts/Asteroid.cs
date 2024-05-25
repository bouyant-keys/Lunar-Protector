using UnityEngine;

public class Asteroid : MonoBehaviour
{
    AsteroidSpawner _spawner;
    SFXManager _sfxManager;
    [SerializeField] GameObject _asteroidVisual;
    [SerializeField] Rigidbody2D _rB;
    float _currentHealth = 25f;
    int _maxHealth = 25;
    float _laserDPS = 2f;
    float _rotationPerSec;
    bool _active = false;

    private void Start()
    {
        _spawner = FindObjectOfType<AsteroidSpawner>();
        _sfxManager = FindObjectOfType<SFXManager>();
    }

    private void Update()
    {
        if (_active) _rB.rotation = transform.rotation.eulerAngles.z + _rotationPerSec; //Asteroid spins in place
    }

    public void Launch()
    {
        _active = true;
        _rotationPerSec = Random.Range(0.25f, 1f);
    }

    private void Die()
    {
        _currentHealth = _maxHealth;
        _rB.velocity = Vector2.zero;

        _sfxManager.PlayClip(_sfxManager._asteroidHit);
        //Play explosion effect

        GameManager._totalActiveAsteroids--;
        _spawner.ReturnObjToPool(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laser")
        {
            _currentHealth -= _laserDPS;

            if (_currentHealth <= 0f) Die();
        }
    }
}
