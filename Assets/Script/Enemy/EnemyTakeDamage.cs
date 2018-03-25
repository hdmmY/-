﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EnemyProperty))]
public class EnemyTakeDamage : MonoBehaviour
{
    private EnemyProperty _enemy;

    private bool _isDead;

    private int _enemyHealth;

    private void OnEnable ()
    {
        _isDead = false;

        _enemy = GetComponent<EnemyProperty> ();
        if (_enemy == null) Debug.LogError ("The Enemy Property is null!");

        _enemyHealth = _enemy.m_health;
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag (EnemyProperty.PlayerBulletTag))
        {
            var bullet = collision.transform.parent.GetComponent<JIBulletProperty> ();

            BulletPool.Instance.ReleaseGameObject (bullet.gameObject);

            var player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerProperty> ();
            player.AddNeutralization (player.m_addValue, bullet.State);

            _enemy.m_health -= bullet.m_damage;
            _enemy.CallOnDamage (_enemy);

            if (_enemy.m_health <= 0 && !_isDead)
            {
                _isDead = true;
                Destroy (_enemy.gameObject);
            }
        }
    }
}