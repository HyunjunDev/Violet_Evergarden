using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacle : Obstacle
{
    public LayerMask groundLayer;
    public float gravity = 2.0f; // �ʱ� �߷� ���ӵ�
    public float maxGravity = 9.8f; // �ִ� �߷� ���ӵ�
    public float fallSpeed = 0.1f; // �ʱ� �������� �ӵ�

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (1 << collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            
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

            // �߷� ���ӵ��� �ִ� �߷� ���ӵ��� �ʰ����� �ʵ��� �����մϴ�.
            fallSpeed = Mathf.Min(fallSpeed, maxGravity);

            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

            RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y * 0.5f, transform.position.z), Vector2.down, 0.1f, groundLayer);

            // ground ���̾�� �浹�ߴٸ� ���ߵ��� �մϴ�.
            if (hit.collider != null)
            {
                isGrounded = true;
            }

        }
    }

}
