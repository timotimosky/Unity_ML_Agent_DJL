using UnityEngine;
using MLAgents;

public class BouncerAcademy : Academy
{
    public float gravityMultiplier = 1f;

    //启动环境时调用一次
    public override void InitializeAcademy()
    {
        Physics.gravity = new Vector3(0, -9.8f * gravityMultiplier, 0);
    }

    //当Academy开始或重新启动模拟时（包括第一次），调用此方法。
    public override void AcademyReset()
    {
    }

    //在agent.AgentAction（）之前（以及Agent收集其观察值之后）的每个模拟步骤调用。
    public override void AcademyStep()
    {
    }
}
