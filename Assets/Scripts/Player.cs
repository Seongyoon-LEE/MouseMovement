using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
[System.Serializable]
public class PlayerAni
{
    public AnimationClip walk, idle, run, attack, skill, jump;
}
public class Player : MonoBehaviour
{
    public PlayerAni playerAni = new PlayerAni();
    [SerializeField]
    private Animation anim;
    public NavMeshAgent agent;
    Ray ray;
    RaycastHit hit;
    Vector3 target = Vector3.zero;
    Transform tr;


    void Start()
    {
     agent = GetComponent<NavMeshAgent>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }

    
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hit, 100, 1 << 8))
            {
                target = hit.point;
                agent.destination = target;
                agent.speed = 3.5f;
                anim.CrossFade(playerAni.walk.name, 0.2f);
                Debug.Log("Walk clip name: " + playerAni.walk.name);
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
