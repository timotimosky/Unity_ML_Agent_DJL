using UnityEngine;
using MLAgents;

public class BouncerAcademy : Academy
{
    public float gravityMultiplier = 1f;

    //��������ʱ����һ��
    public override void InitializeAcademy()
    {
        Physics.gravity = new Vector3(0, -9.8f * gravityMultiplier, 0);
    }

    //��Academy��ʼ����������ģ��ʱ��������һ�Σ������ô˷�����
    public override void AcademyReset()
    {
    }

    //��agent.AgentAction����֮ǰ���Լ�Agent�ռ���۲�ֵ֮�󣩵�ÿ��ģ�ⲽ����á�
    public override void AcademyStep()
    {
    }
}
