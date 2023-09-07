using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacle : Obstacle
{
    public LayerMask groundLayer;
    public LayerMask playerLayer;
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
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            Debug.Log("플레이어 들어옴");
            collision.transform.parent.SetParent(transform);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            Debug.Log("플레이어 들어옴");
            collision.transform.parent.SetParent(transform);
            collision.transform.parent.position = new Vector3(collision.transform.parent.position.x, transform.position.y + transform.localScale.y * 0.39f, collision.transform.parent.position.z);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            Debug.Log("플레이어 나감");
            collision.transform.parent.SetParent(null);
        }
    }

    private void Awake()
    {
        MapManager.Instance.onPlayerDead.AddListener(ReStart);
        startPos = transform.position;
        startGravity = gravity;
        startFallSpeed = fallSpeed;
    }

    public void Update()
    {
        if (isPlayerIn&&!isGrounded) 
        {
            fallSpeed += gravity * Time.deltaTime;

            // 중력 가속도가 최대 중력 가속도를 초과하지 않도록 제한합니다.
            fallSpeed = Mathf.Min(fallSpeed, maxGravity);

            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            RaycastHit2D groundHit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y * 0.5f, transform.position.z), Vector2.down, 0.1f, groundLayer);

            // ground 레이어와 충돌했다면 멈추도록 합니다.
            if (groundHit.collider != null)
            {
                isGrounded = true;
            }
        }
    }
}
