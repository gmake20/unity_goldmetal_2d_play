using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public int nextMove;
    public GameObject left;
    public GameObject right;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        // Think();
        Invoke("Think", 1);

    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // 
        Vector2 pos = left.transform.position;
        if (nextMove == 1) pos = right.transform.position;

        Debug.DrawRay(pos, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector3.down, 0.5f, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            Turn();
            // Debug.Log(rayHit.collider.name);
        }
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

        anim.SetInteger("WalkSpeed", nextMove);

        if (nextMove == -1) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == 1;

        CancelInvoke();
        Invoke("Think", 5);
    }
}
