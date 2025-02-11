using System.Collections;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    private const float MOVE_SPEED = 60f;
    private const float ROLL_SPEED = 250f;
    private const float ROLL_DECAY = 3f;
    private const float DASH_DISTANCE = 50f;
    private const float DASH_DURATION = 0.1f;
    private const float ROLL_COOLDOWN = 1f;
    private const float DASH_COOLDOWN = 1f;

    private enum State { Normal, Rolling }

    [SerializeField] private LayerMask dashLayerMask;
    [SerializeField] private GameObject characterSprite;

    private Character_Base characterBase;
    private Rigidbody2D rb;
    private Vector3 moveDir;
    private Vector3 rollDir;
    private Vector3 lastMoveDir;
    private float rollSpeed;
    private float lastRollTime;
    private float lastDashTime;
    private bool isDashing;
    private State state;

    private void Awake()
    {
        characterBase = GetComponent<Character_Base>();
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                HandleMovement();
                if (Input.GetKeyDown(KeyCode.Space)) AttemptRoll();
                if (Input.GetKeyDown(KeyCode.F)) AttemptDash();
                break;

            case State.Rolling:
                rollSpeed -= rollSpeed * ROLL_DECAY * Time.deltaTime;
                if (rollSpeed < 50f) state = State.Normal;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing) return;

        switch (state)
        {
            case State.Normal:
                rb.velocity = moveDir * MOVE_SPEED;
                break;
            case State.Rolling:
                rb.velocity = rollDir * rollSpeed;
                break;
        }
    }

    private void HandleMovement()
    {
        float moveX = 0f, moveY = 0f;

        if (Input.GetKey(KeyCode.W)) moveY = +1f;
        if (Input.GetKey(KeyCode.S)) moveY = -1f;
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
            characterSprite.GetComponent<SpriteRenderer>().flipX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveX = +1f;
            characterSprite.GetComponent<SpriteRenderer>().flipX = false;
        }

        moveDir = new Vector3(moveX, moveY).normalized;
        if (moveDir != Vector3.zero) lastMoveDir = moveDir;
        characterBase.PlayMoveAnim(moveDir);
    }

    private void AttemptRoll()
    {
        if (Time.time < lastRollTime + ROLL_COOLDOWN) return;

        lastRollTime = Time.time;
        rollDir = lastMoveDir;
        rollSpeed = ROLL_SPEED;
        state = State.Rolling;
        characterBase.PlayRollAnimation(rollDir);
    }

    private void AttemptDash()
    {
        if (Time.time < lastDashTime + DASH_COOLDOWN || isDashing) return;

        lastDashTime = Time.time;
        Vector3 dashTarget = transform.position + lastMoveDir * DASH_DISTANCE;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDir, DASH_DISTANCE, dashLayerMask);
        if (hit.collider != null) dashTarget = hit.point;

        StartCoroutine(DashCoroutine(dashTarget));
    }

    private IEnumerator DashCoroutine(Vector3 target)
    {
        isDashing = true;
        Vector3 start = transform.position;
        float elapsed = 0;

        while (elapsed < DASH_DURATION)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / DASH_DURATION);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isDashing = false;
    }
}
