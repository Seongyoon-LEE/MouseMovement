using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] // navmeshagent ������Ʈ�� ������ �ڵ����� �߰�
[System.Serializable]// �ν����Ϳ��� ���̰� �ϱ� ����
public class PlayerAni
{
    public AnimationClip walk, idle, run, attack, skill, jump; // �ִϸ��̼� Ŭ���� Ŭ������ ���� 
}
public class Player : MonoBehaviour
{
    public PlayerAni playerAni = new PlayerAni(); // �ִϸ��̼� Ŭ���� ������ Ŭ���� ���� �Ҵ�
    [SerializeField] // �����̺� �ִϸ��̼��� �ν����Ϳ��� ���̰� �ϱ� ����
    private Animation anim;
    public NavMeshAgent agent;
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero; // Ÿ�� ��ġ �ʱ�ȭ
    Transform tr;


    void Start()
    {
     agent = GetComponent<NavMeshAgent>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }

    
    void Update()
    {
        MouseMovement();
        Attack();
        Jump();
        Run();
        Skill();
    }

    private void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.CrossFade(playerAni.skill.name, 0.2f); // ��ų �ִϸ��̼� ��ȯ
            agent.speed = 0; // �̵� ����
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            anim.CrossFade(playerAni.run.name, 0.2f); // �޸��� �ִϸ��̼� ��ȯ
            agent.speed = 7f; // �̵� �ӵ� ����
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // shift�� ���� 
        {
            anim.CrossFade(playerAni.idle.name, 0.2f);
            agent.speed = 0f;
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.CrossFade(playerAni.jump.name, 0.2f); // ���� �ִϸ��̼� ��ȯ
            agent.speed = 0; // �̵� ����
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.CrossFade(playerAni.walk.name, 0.2f); // ���̵� �ִϸ��̼� ���
        }

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.CrossFade(playerAni.attack.name, 0.2f); // ���� �ִϸ��̼� ��ȯ
            agent.speed = 0; // �̵� ����
            agent.velocity = Vector3.zero; // �ӵ� �ʱ�ȭ
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // ���콺 ��ġ�� 3D �������� � �������� ������ִ� �Լ�
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // ���̸� �ð�ȭ
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // ����ĳ��Ʈ�� ��� �浹�� ������Ʈ�� ���̾� 8(����)�� ��
            {
                target = hit.point; // Ÿ�� ��ġ�� ����ĳ��Ʈ�� �浹�� �������� ����
                agent.destination = target; // �׺�޽� ������Ʈ�� �������� Ÿ������ ����
                agent.speed = 3.5f; // �̵� �ӵ� ����
                anim.CrossFade(playerAni.walk.name, 0.2f); // �ִϸ��̼� ��ȯ
                //Debug.Log("Walk clip name: " + playerAni.walk.name);
            }
            else
            {
                if (agent.remainingDistance <= 0.5f)
                {
                    anim.CrossFade(playerAni.idle.name, 0.2f);
                }
            }
        }
    }
}
