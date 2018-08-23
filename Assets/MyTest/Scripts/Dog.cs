using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Animator DogAnimator;
    private float m_Speed = 0.1f;
    private bool m_HaveTarget = false;
    private Transform m_TargetPosition;
    // Use this for initialization
    void Awake()
    {
        GlobalEvent.ImComing += ImComing;
    }
    void OnDestroy()
    {
        GlobalEvent.ImComing -= ImComing;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_HaveTarget)
        {
            if (Vector3.Distance(m_TargetPosition.position, transform.position) < 0.1f)
            {
                DogAnimator.Play("Bark");
                m_HaveTarget = false;
            }
            else
            {
                transform.LookAt(m_TargetPosition);
                transform.position += (m_TargetPosition.position - transform.position).normalized * Time.deltaTime * m_Speed;
            }
        }
    }
    void ImComing(Transform t)
    {
        m_TargetPosition = t;
        DogAnimator.Play("Move");
        m_HaveTarget = true;
    }
}
