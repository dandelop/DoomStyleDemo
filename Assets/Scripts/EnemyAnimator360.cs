using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator360 : MonoBehaviour
{
    // reference to parent script
    private Enemy _enemy;
    
    // reference to "parent" rigidbody
    private Rigidbody _rigidbody;
 
    // reference to sprite renderer
    private SpriteRenderer _spriteRenderer;
    
    // reference to animator controller script
    private Animator _animator;
    private bool _diying = false;

    // player reference
    private Player _player;
    
    // constant angle diff for the animations
    private const float ANGLE = 45f;
    private const float MIDDLE_ANGLE = ANGLE / 2f;

    // Texture for skin enemy
    [SerializeField] private EnemyType _enemyType;
    [SerializeField] private Texture2D _textureAlternativeSprites;
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    
    // get references
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        // get all sprites from the texture if assigned and load it in the dictionary
        Sprite[] sprites;
        if ((_enemyType != EnemyType.Default) && (_enemyType != EnemyType.Kind))
        {
            if (_textureAlternativeSprites != null)
            {
                sprites = Resources.LoadAll<Sprite>(_textureAlternativeSprites.name);
                foreach (Sprite sprite in sprites)
                {
                    if (!_sprites.ContainsKey(sprite.name))
                    {
                        _sprites.Add(sprite.name, sprite);
                    }
                }
            }
            else
            {
                _enemyType = EnemyType.Default;
            }
        }
    }

    private void Start()
    {
        _player = GameManager.Instance.Player;
        _rigidbody = GetComponentInParent<Rigidbody>();
        _enemy = GetComponentInParent<Enemy>();
    }

    private void Update()
    {
        // if it diying don't do anything
        if (_diying)
        {
            return;
        }
        
        // change the animator parameter angle to the angle between the enemy forward and the player
        float angleSigned = Vector3.SignedAngle(_rigidbody.transform.forward, _player.transform.position - transform.position, Vector3.up);
        float angle = Vector3.Angle(_rigidbody.transform.forward, _player.transform.position - transform.position);
        //Debug.Log("Angle (signed vs unsigned): " + angleSigned +  " <--> " + angle);
        //Debug.Log("Angle 360: " + (angleSigned > 0 ? angleSigned : 360 + angleSigned));
        _animator.SetFloat("Angle", angle);
        // change animation clip based on the angle signed (0 to 180 + 0 to -180) with functions (+ , div , %)
        int index = (Mathf.RoundToInt((angleSigned > 0 ? angleSigned : 360 + angleSigned) / ANGLE)) % 8;
        String animName = "EnemyWalk_" + index * ANGLE;
        //Debug.Log("Animation name: " + animName);
        _animator.Play(animName);
    }

    
    // re-skinning the enemy
    private void LateUpdate()
    {
        // check the enemy type for re-skinning (only for different to Kind (default))
        if ((_enemyType != EnemyType.Default) && (_enemyType != EnemyType.Kind))
        {
            // set the sprite (except for the die animation that is the same)
            if (!_diying) {
                _spriteRenderer.sprite = _sprites[_spriteRenderer.sprite.name];
            }
        }
    }
    
    public void DieAnim()
    {
        _diying = true;
        // launch the die animation via trigger
        _animator.SetTrigger("Die");
    }
    
    public void FinishDieAnim()
    {
        // call to the parent to destroy the enemy
        _enemy.FinishDieAnim();
    }

    // draw gizmo to show the vectors: player to enemy, enemy forward
    private void OnDrawGizmos()
    {
        // forward from rigidbody
        if (_rigidbody != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position, _rigidbody.transform.forward);
        }

        // to player
        if (_player != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, _player.transform.position - transform.position);
        }
    }

}
