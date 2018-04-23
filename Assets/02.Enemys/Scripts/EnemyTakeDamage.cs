﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (EnemyProperty))]
public class EnemyTakeDamage : MonoBehaviour
{
    private EnemyProperty _enemy;

    private bool _isDead;

    private void OnEnable ()
    {
        _isDead = false;
        _enemy = GetComponent<EnemyProperty> ();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag (EnemyProperty.PlayerBulletTag))
        {
            var bullet = collision.transform.parent.GetComponent<JIBulletProperty> ();

            EventManager.Instance.Raise (new EnemyHurtedEvent (bullet.State));

            BulletPool.Instance.ReleaseGameObject (bullet.gameObject);

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