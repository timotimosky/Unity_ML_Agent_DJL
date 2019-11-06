using MLAgents;
using UnityEngine;

//Agent子类定义了Agent用于观察其环境，执行分配的动作以及计算用于强化训练的报酬的代码。您也可以实现可选方法，
//以在代理完成或失败任务后重置代理。
public class AgentDJL : Agent
{
    private BasicAcademy m_Academy;
    int m_Position;
    Vector3 mPostion;
    float dis;
    GameObject fianl;

    float lastDis = 10000;

    public override void InitializeAgent()
    {
        m_Academy = FindObjectOfType(typeof(BasicAcademy)) as BasicAcademy;
    }

    //收集观察值
    public override void CollectObservations()
    {
        AddVectorObs(fianl.transform.localPosition- transform.localPosition); //寻找终点,距离远近
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("wall"))
        {
            Debug.Log("触碰到障碍");
            Done();
        }
        if (collision.transform.tag.Equals("goal"))
        {
           // float nowDis = (transform.localPosition - fianl.transform.localPosition).magnitude;
            Debug.LogError("触碰到目标------");
        }
    }

    //调用每个模拟步骤。接收大脑选择的动作
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //我们得到几个值，用于旋转和移动
        var actionX = Mathf.Clamp(vectorAction[0], -1f, 1f);
        var actionZ =  Mathf.Clamp(vectorAction[1], -1f, 1f);

        transform.localPosition += new Vector3(actionX, 0, actionZ);

        if (transform.localPosition.z > 18 || transform.localPosition.z < -18 ||
            transform.localPosition.x > 18 || transform.localPosition.x < -18)
        {
            SetReward(-1f);
            Done();
        }

        float nowDis = (transform.localPosition - fianl.transform.localPosition).magnitude;

        if (nowDis < 2)
        {
            Debug.LogError("完成目标");
            SetReward(1f);
            Done();
        }
        if (nowDis > (lastDis+0.5f))
        {
            Debug.LogError("获得远离惩罚");
            SetReward(-0.1f);
        }
        else if (nowDis < (lastDis - 0.5f))
        {
            Debug.LogError("获得接近奖励");
            SetReward(0.1f);
        }

        lastDis = nowDis;
    }

    //在代理重置（包括在会话开始时）时调用
    //该函数将重置值随机化，以便将训练推广到不依赖特定位置的情况
    public override void AgentReset()
    {
        fianl = gameObject.transform.parent.Find("fianl").gameObject;
        Debug.Log("重置 代理");
        gameObject.transform.localPosition = new Vector3(-7.89f, -0.85f, 8.85f);
        mPostion = gameObject.transform.localPosition;
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(-10f, 10f));
        fianl.transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), -0.7f, Random.Range(-1.5f, 1.5f));
        lastDis = (transform.localPosition - fianl.transform.localPosition).magnitude;
    }


}
