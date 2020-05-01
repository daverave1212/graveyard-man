using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    [SerializeField] private SphereCollider triggerCollider;

    private Collider[] _colliders;
    private Rigidbody[] _rbs;

    public GameObject draggablePart;

    public bool knocked = false;

    private void Start()
    {
        _colliders = transform.GetChild(0).gameObject.GetComponentsInChildren<Collider>();
        _rbs = transform.GetChild(0).gameObject.GetComponentsInChildren<Rigidbody>();

        // Make trigger sphere to ignore inner colliders...
        foreach (var childCollider in _colliders)
        {
            Physics.IgnoreCollision(triggerCollider, childCollider, true);
        }

        SetRagdollState(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ProjectileShovel"))
        {
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            float initialVelocityMagnitude = otherRb.velocity.sqrMagnitude;

            otherRb.AddForce(otherRb.velocity * -1.1f, ForceMode.Impulse);

            if (Mathf.Abs(initialVelocityMagnitude) > 8.0f)
            {
                EnemyManager.Instance.DestroyEnemy(gameObject);
                TriggerDeathAnim();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && knocked)
        {
            hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            hint.SetActive(false);
        }
    }

    public void SetRagdollState(bool state)
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = state;
        }

        foreach (var rb in _rbs)
        {
            rb.isKinematic = !state;
        }
    }

    public void TriggerDeathAnim()
    {
        knocked = true;
        GetComponentInChildren<Animator>().enabled = false;
        SetRagdollState(true);
    }
}