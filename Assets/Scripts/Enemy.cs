using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IPlayerShootReceiver
{
    // references to children components
    private EnemyAnimator _enemyAnimator;
    private EnemyAnimator360 _enemyAnimator360;

    // reference to collider
    private Collider _collider;
    
    // enemy state
    private bool _die = false;

    // nav mesh agent
    private NavMeshAgent _navMeshAgent;
    
    // reference to player
    private Player _player;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_player = GameManager.Instance.Player;
    }

    private void Start()
    {
        // get reference to the animator script
        _enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        _enemyAnimator360 = GetComponentInChildren<EnemyAnimator360>();
    }

    private void Update()
    {
        _navMeshAgent.SetDestination(GameManager.Instance.Player.transform.position);
    }

    public void OnShoot()
    {
        _die = true;
        _collider.enabled = false;
        // execute die animation via animator script
        if (_enemyAnimator != null)
            _enemyAnimator.DieAnim();
        else if (_enemyAnimator360 != null)
            _enemyAnimator360.DieAnim();
    }

    public void FinishDieAnim()
    {
        // destroy the enemy
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision col)
    {
        Hit(col.collider);
    }

    private void Hit(Collider col)
    {
        if (col.CompareTag("Player") || col.CompareTag("PlayerChildren"))
        {   
            GameManager.Instance.Player.HitByEnemy();
        }
    }
}
