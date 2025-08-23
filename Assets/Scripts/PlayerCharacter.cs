using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    int health;
    bool isGrounded;
    [SerializeField] float jumpForce = 300f; // ��Ծ����
    [SerializeField] float moveSpeed = 5f; // �ƶ��ٶ�
    [SerializeField] int maxHealth = 100; // �������ֵ

    // Start is called before the first frame update
    void Start()
    {

    }

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        health = maxHealth; // ��ʼ������ֵ
        tag = "Player"; // ���ñ�ǩΪ Player
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.01f;
    }

    //�ڲ��߼�
    private void Die()
    {
        Debug.Log($"{tag} has died.");
        rb.velocity = Vector2.zero; // ֹͣ��ɫ�ƶ�
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false; // ���ؽ�ɫ
        Destroy(gameObject);
    }


    //ʵ�ֽӿ�IPawn
    public void Jump()
    {
        //��Ծ
        if (rb != null && isGrounded) // ����Ƿ��ڵ�����
        {
            rb.AddForce(new Vector2(0, jumpForce));
        }
    }
    public void Move(float direction)
    {
        //�ƶ�
        Vector2 moveDirection = new Vector2(direction, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }

    //ʵ�ֽӿ�IEntity
    public int GetHealth()
    {
        return health;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public string GetName()
    {
        return tag;
    }
    public bool IsDead()
    {
        return health <= 0;
    }
    public void Damaged(int damage)
    {
        health -= damage;
        if (health < 0) health = 0;
        Debug.Log($"{tag} took {damage} damage, current health: {health}");
        if (health == 0)
        {
            // �����ɫ�����߼�
            Die();
        }
    }
}
