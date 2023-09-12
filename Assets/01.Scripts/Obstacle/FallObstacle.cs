using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacle : Obstacle
{
    public LayerMask groundLayer;
    public float gravity = 2.0f; // 초기 중력 가속도
    public float maxGravity = 9.8f; // 최대 중력 가속도
    public float fallSpeed = 0.1f; // 초기 떨어지는 속도

    private Vector3 startPos;
    private float startGravity;
    private float startFallSpeed;

    [SerializeField]
    private bool isGrounded = false;

    private bool isPlayerIn;
    public bool IsPlayerIn
    {
        get { return isPlayerIn; }
        set { isPlayerIn = value; }
    }

    public override void ReStart()
    {
        transform.position = startPos;
        isGrounded = false;
        isPlayerIn = false;
        gravity = startGravity;
        fallSpeed = startFallSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(transform);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(transform);
            collision.transform.parent.position = new Vector3(collision.transform.parent.position.x, transform.position.y + transform.GetComponentInChildren<BoxCollider2D>().size.y * 0.35f, collision.transform.parent.position.z);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent.SetParent(null);
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RaycastHit2D groundHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y * 0.5f, transform.position.z), Vector2.down, collision.transform.GetComponentInChildren<BoxCollider2D>().size.y * collision.transform.localScale.y, groundLayer);
            if (groundHit.collider != null)
            {
                if (collision.transform.position.x >= transform.position.x - transform.localScale.x * 0.5f - collision.transform.GetComponentInChildren<BoxCollider2D>().size.x * collision.transform.localScale.x  && collision.transform.position.x <= transform.position.x + transform.localScale.x * 0.5f + collision.transform.GetComponentInChildren<BoxCollider2D>().size.x * collision.transform.localScale.x )
                {
                    Debug.Log("충돌했어요구르트아줌마");
                    EventManager.Instance.onPlayerDead.Invoke();
                }
            }
        }
    }

    public void Gravity()
    {
        if (isPlayerIn && !isGrounded)
        {
            fallSpeed += gravity * Time.deltaTime;

            fallSpeed = Mathf.Min(fallSpeed, maxGravity);

            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            RaycastHit2D groundHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y * 0.5f, transform.position.z), Vector2.down, 0.1f, groundLayer);

            if (groundHit.collider != null)
            {
                isGrounded = true;
            }
        }
    }


    private void Awake()
    {
        EventManager.Instance.onFadeIn.AddListener(ReStart);
        startPos = transform.position;
        startGravity = gravity;
        startFallSpeed = fallSpeed;
    }

    public void Update()
    {
        Gravity();
    }
}
