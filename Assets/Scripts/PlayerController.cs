using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PlayerController : MonoBehaviour
{
    public static bool _activePlayer = true;

    public LayerMask _groundLayer;
    Rigidbody2D _rB;
    SpriteRenderer _playerVisual;
    [SerializeField] ParticleSystem _jumpParticles;
    InteractTrigger _trigger;
    AudioSource _audioSource;
    UIManager _uiManager;
    SFXManager _sfxManager;
    
    [SerializeField] float _speed = 5f;
    [SerializeField] float _jumpForce = 3f;
    [SerializeField] bool _isGrounded;


    private void Awake()
    {
        _rB = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _trigger = GetComponentInChildren<InteractTrigger>();
        _playerVisual = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _sfxManager = FindObjectOfType<SFXManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_activePlayer) return;

        float dir = Input.GetAxis("Horizontal") * _speed;
        Vector2 force = transform.right.normalized * dir;
        UpdateSprite(dir);

        if (dir == 0f && _isGrounded) _rB.drag += 1f;
        else _rB.drag = 0f;
        _rB.AddForce(force, ForceMode2D.Force);
    }

    private void Update()
    {
        if (!_activePlayer) return;

        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _isGrounded = false;
            Vector2 jumpVel = transform.up.normalized * _jumpForce;
            _rB.AddForce(jumpVel, ForceMode2D.Impulse);
            _jumpParticles.Play();
            _sfxManager.PlayClip(_sfxManager._jump);
        }

        if (Input.GetKeyDown(KeyCode.E) && _trigger._canInteract)
        {
            _rB.velocity = Vector2.zero; //Check
            _trigger.Interact();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _uiManager.Pause();
        }
    }

    private void UpdateSprite(float direction)
    {
        if (direction > 0f) _playerVisual.flipX = false;
        else if (direction < 0f) _playerVisual.flipX = true;
    }

    private void CheckGround()
    {
        Vector2 boxCenter = transform.position + (transform.up * -0.15f);
        _isGrounded = Physics2D.OverlapBox(boxCenter, new Vector2(0.15f, 0.15f), transform.rotation.z, _groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position + (transform.up * -0.15f), new Vector2(0.15f, 0.15f));
    }
}
