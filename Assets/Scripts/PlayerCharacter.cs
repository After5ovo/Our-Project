using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IPawn, IEntity
{
    Rigidbody2D rb;
    Animator anim;
    int health;
    bool isGrounded;
    bool isTowardsLeft;
    GameObject triggeringObject;
    List<GameObject> abilityInstances;
    [SerializeField] float jumpForce = 300f; // ��Ծ����
    [SerializeField] float moveSpeed = 5f; // �ƶ��ٶ�
    [SerializeField] int maxHealth = 100; // �������ֵ
    [SerializeField] GameObject[] abilities; // ���ܶ���

    // Start is called before the first frame update
    void Start()
    {

    }

    // ...

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        health = maxHealth;
        tag = "Player";
        // ʵ��������
        if (abilities != null)
        {
            abilityInstances = new List<GameObject>();
            for (int i = 0; i < abilities.Length; i++)
            {
                abilityInstances.Add(Instantiate(abilities[i], transform));
            }
        }
        isTowardsLeft = false;
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
        if (direction < 0)
        {
            isTowardsLeft = true;
        }
        else if (direction > 0)
        {
            isTowardsLeft = false;
        }
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    public void UseAbility(int abilityIndex)
    {
        if (abilityInstances != null && abilityIndex >= 0 && abilityIndex < abilityInstances.Count)
        {
            abilityInstances[abilityIndex].GetComponent<IAbility>()?.Activate(GetComponent<IEntity>());
        }
    }

    //实现接口IEntity
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
            // 处理角色死亡逻辑
            Die();
        }
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public bool IsTowardsLeft()
    {
        return isTowardsLeft;
    }
}
