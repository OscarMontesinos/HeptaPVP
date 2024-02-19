using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float spdOverTime;
    public float range;
    [HideInInspector]
    public  Rigidbody2D _rigidbody;
    [HideInInspector]
    public Vector2 startPos;
    public bool collideWalls;
    public GameObject particle;
    public bool withoutRange;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    public virtual void Update()
    {
        speed += spdOverTime * Time.deltaTime;
    }
    public virtual void FixedUpdate()
    {
        Vector2 dir = transform.up;
        _rigidbody.velocity = dir.normalized * speed;
        if (!withoutRange)
        {
            Vector2 dist = startPos - new Vector2(transform.position.x, transform.position.y);
            if (dist.magnitude > range)
            {
                Die();
            }
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && collideWalls)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(particle != null)
        {
            particle.transform.parent = null;
        }
        Destroy(gameObject);
    }
}
