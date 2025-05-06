using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))] // navmeshagent 컴포넌트가 없으면 자동으로 추가
[System.Serializable]// 인스펙터에서 보이게 하기 위해
public class PlayerAni
{
    public AnimationClip walk, idle, run, attack, skill, jump; // 애니메이션 클립을 클래스로 저장 
}
public class Player : MonoBehaviour
{
    public PlayerAni playerAni = new PlayerAni(); // 애니메이션 클립을 저장할 클래스 동적 할당
    [SerializeField] // 프라이빗 애니메이션을 인스펙터에서 보이게 하기 위해
    private Animation anim;
    public NavMeshAgent agent;
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero; // 타겟 위치 초기화
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
            anim.CrossFade(playerAni.skill.name, 0.2f); // 스킬 애니메이션 전환
            agent.speed = 0; // 이동 정지
            agent.velocity = Vector3.zero; // 속도 초기화
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {

            anim.CrossFade(playerAni.run.name, 0.2f); // 달리기 애니메이션 전환
            agent.speed = 7f; // 이동 속도 증가
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) // shift를 때면 
        {
            anim.CrossFade(playerAni.idle.name, 0.2f);
            agent.speed = 0f;
            agent.velocity = Vector3.zero; // 속도 초기화
        }
    }

    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            anim.CrossFade(playerAni.jump.name, 0.2f); // 점프 애니메이션 전환
            agent.speed = 0; // 이동 정지
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            anim.CrossFade(playerAni.walk.name, 0.2f); // 아이들 애니메이션 재생
        }

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.CrossFade(playerAni.attack.name, 0.2f); // 공격 애니메이션 전환
            agent.speed = 0; // 이동 정지
            agent.velocity = Vector3.zero; // 속도 초기화
        }
    }

    private void MouseMovement()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치를 3D 공간에서 어떤 방향인지 계산해주는 함수
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow); // 레이를 시각화
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 100, 1 << 8)) // 레이캐스트를 쏘고 충돌한 오브젝트가 레이어 8(지형)일 때
            {
                target = hit.point; // 타겟 위치를 레이캐스트가 충돌한 지점으로 설정
                agent.destination = target; // 네비메시 에이전트의 목적지를 타겟으로 설정
                agent.speed = 3.5f; // 이동 속도 설정
                anim.CrossFade(playerAni.walk.name, 0.2f); // 애니메이션 전환
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
