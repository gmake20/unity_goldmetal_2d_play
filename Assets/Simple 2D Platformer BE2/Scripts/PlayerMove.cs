using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed;
    public float jumpPower;
    public Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CircleCollider2D coll;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        // Debug.Log(h);
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);
        if (rigid.velocity.x > maxSpeed) rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        if (rigid.velocity.x < -maxSpeed) rigid.velocity = new Vector2(-maxSpeed, rigid.velocity.y);
    }

    private void Update()
    {
        if (rigid.velocity.y < 0)
            rigid.AddForce(Vector2.down * 5f);

        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")) 
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }

        // Stop Speed
        if(Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        }

        // 방향전환
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        /*
        if(rigid.velocity.normalized.x >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        */

        // Debug.Log(rigid.velocity.x);

        if ( Mathf.Abs(rigid.velocity.x) < 0.1)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);

        if(rigid.velocity.y < 0)
        {
            // Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.7f, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                anim.SetBool("isJumping", false);
                // Debug.Log(rayHit.collider.name);
            }

        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Enemy")
        {
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else
            {
                OnDamaged(collision.transform.position);
                //Debug.Log("Player damaged!!");
            }
        }
    }

    void OnAttack(Transform enemy)
    {
        GameManager.Instance.stagePoint += 50;

        // Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        // Enemy Die
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    } 

    void OnDamaged(Vector2 targetPos)
    {
        // Player Health Down
        GameManager.Instance.HealthDown();

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged");

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        int dirc = transform.position.x -  targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc,1) * 7 , ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");

        Invoke("OffDamaged", 3);
    }

    public void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Item")
        {
            ItemCoin coin = collision.gameObject.GetComponent<ItemCoin>();
            // Point
            GameManager.Instance.stagePoint += coin.coin;

            // Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            // Next Stage 
            GameManager.Instance.NextStage();
        }
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        coll.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        if(rigid)
            rigid.velocity = Vector2.zero;
    }
}
