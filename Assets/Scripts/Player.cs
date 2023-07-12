using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour, IActionEvent, IFloorDamageReceiver
{
    // Events emitted by the player
    public event EventHandler OnActionEvent;
    
    // Velocity of the player
    [SerializeField] private float speed = 10f;
    // Mouse sensitivity
    [SerializeField] private float mouseSensitivity = 2f;
    // Animator of the player
    [SerializeField] private Animator _animatorHand;
    // Weapon of the player
    [SerializeField] private WeaponType _weapon;
    // Health of the player
    [SerializeField] private int _health = 100;
    
    // Rigidbody of the player
    private Rigidbody _rigidbody;
    
    // Camera of the player
    private Camera _camera;
    private Vector3 _cameraOriginalPosition;
    
    // Audio source of the player
    private AudioSource _audioSource;
    
    // Inputs keys and mouse
    private float _horizontal;
    private float _vertical;
    private float _mouseX;
    private float _mouseY;
    
    // States of the player
    private bool _isWalking = false;
    private bool _isShooting = false;

    
    private void Awake()
    {
        // get reference to the rigidbody, camera, audio source
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        try
        {
            // register the player in the GameManager
            GameManager.Instance.Player = this;
        }
        catch (Exception e)
        {
            Debug.Log("GameManager not found");
        }
    }
    
    // block the cursor
    private void Start()
    {
        // register the player in the GameManager
        GameManager.Instance.Player = this;
        // get the camera of the player
        _camera = GetComponentInChildren<Camera>();
        // remember the original position of the camera
        _cameraOriginalPosition = _camera.transform.localPosition;
        // lock the cursor mouse
        Cursor.lockState = CursorLockMode.Locked;
        // set health of the player
        GameManager.Instance.UpdateHealth(_health);
    }
    
    // Update is called once per frame
    void Update()
    {
        // Update Inputs
        UpdateInputs();
        
        // Update States
        UpdateStates();
        
        // Update Physics
        UpdatePhysics();
        
        // Update Animations
        UpdateAnimations();
    }

    private void UpdateInputs()
    {
        // Get the input keys from the player
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        
        // Get the input mouse from the player
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");
        
        // input mouse key for shoot the gun
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        
        // input key for action
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Action();
        }
    }

    private void UpdateStates()
    {
        // update walking state
        _isWalking = _rigidbody.velocity.magnitude > 0.1f;
    }

    private void UpdatePhysics()
    {
        // Rotate the player
        transform.Rotate(0, _mouseX * mouseSensitivity, 0);
        
        // Move the player
        _rigidbody.velocity = transform.forward * (_vertical * speed) + transform.right * (_horizontal * speed);
        
        // if player is movement, anim camera
        if (_isWalking)
        {
            // move camera up and down
            _camera.transform.localPosition = 
                Vector3.Lerp(_camera.transform.localPosition, 
                    _cameraOriginalPosition + new Vector3(0f, Mathf.Sin(Time.time * 10) * 0.05f, 0), 0.1f);
        }
        else
        {
            // fix camera to original position
            _camera.transform.localPosition =
                Vector3.Lerp(_camera.transform.localPosition,_cameraOriginalPosition, 0.1f);
        }
    }

    
    private void UpdateAnimations()
    {
        // set parameter "Weapon" in the animator
        _animatorHand.SetInteger("Weapon", (int) _weapon);
        
        // set parameter "Walking" in the animator
        _animatorHand.SetBool("Walking", _isWalking);
        
        // shoot the gun
        if (_isShooting)
        {
            _animatorHand.SetTrigger("Shoot");
            _isShooting = false;
        }
    }
    
    private void Shoot()
    {
        _isShooting = true;
        // Raycast for shoot the gun
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            // get the component PlayerShootReceiver from the object hit
            IPlayerShootReceiver playerShootReceiver = hit.collider.gameObject.GetComponent<IPlayerShootReceiver>();
            // if the object hit has the component PlayerShootReceiver, call the method OnShoot()
            if (playerShootReceiver != null)
            {
                playerShootReceiver.OnShoot();
            }
        }
    }
    
    // Action for the player
    private void Action()
    {
        // launch event for action
        OnActionEvent?.Invoke(this, EventArgs.Empty);
    }

    public void OnFloorDamage()
    {
        Debug.Log("Floor Damage");
        // emit a sound
        _audioSource.Play();
    }
    
    public void HitByEnemy()
    {
        Debug.Log("Hit by Enemy");
        _health -= 10;
        GameManager.Instance.UpdateHealth(_health);
    }
    
    // draw gizmo for debug pointer gun
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 10);
    }
}
