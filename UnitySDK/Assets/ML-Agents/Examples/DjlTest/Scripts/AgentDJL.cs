using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Agent子类定义了Agent用于观察其环境，执行分配的动作以及计算用于强化训练的报酬的代码。您也可以实现可选方法，
//以在代理完成或失败任务后重置代理。
public class AgentDJL : Agent
{
    private BasicAcademy m_Academy;
    int m_Position;
    int coll;
    float ray = 0;
    Ray mRay;
    int ball_layer;
    int goal_layer;
    float dis;

    public override void InitializeAgent()
    {
        ball_layer = LayerMask.NameToLayer("ball");
        goal_layer = LayerMask.NameToLayer("goal");
        mRay = new Ray();
        m_Academy = FindObjectOfType(typeof(BasicAcademy)) as BasicAcademy;
    }

    private void FixedUpdate()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
            , transform.forward, out hitInfo, 10, ball_layer))
        {
            //if (hitInfo.transform.gameObject.layer.Equals())
            //{

            //}
            ray = 1;
            dis = Vector3.Distance(transform.position, hitInfo.transform.position);
        }
        else if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
    , transform.forward, 10, goal_layer))
        {
            if (hitInfo.transform != null)
            {
                dis = Vector3.Distance(transform.position, hitInfo.transform.position);
                ray = 2;
            }
        }
        else
        {
            dis = 0;
            ray = 0;
        }
    }

    //收集观察值
    public override void CollectObservations()
    {
        //    //视觉：距离 与夹角
        //    if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
        //, transform.forward, 5, LayerMask.NameToLayer("ball")))
        //    {
        //        ray = 1;
        //    }
        //    else if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
        //, transform.forward, 5, LayerMask.NameToLayer("goal")))
        //    {

        //        ray = 2;
        //    }
        //    else
        //        ray = 0;


        //发射射线
        AddVectorObs(ray);
        AddVectorObs(dis);
        AddVectorObs(transform.position);
        //  AddVectorObs(coll);
        //  AddVectorObs(m_Position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("wall"))
        {
            Debug.Log("触碰到障碍");
            coll = 1;
        }
        if (collision.transform.tag.Equals("goal"))
        {
            Debug.LogError("触碰到目标------");
            coll = 100;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag.Equals("wall"))
        {
          //  coll = -1;
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //我们得到几个值，用于旋转和移动
        var actionTransF = transform.forward * vectorAction[0];
        var actionTransR = Mathf.Clamp(vectorAction[1], -1f, 1f);

        transform.position += actionTransF;
        gameObject.transform.Rotate(new Vector3(0, 1, 0), actionTransR);

        //AddReward(-0.01f);

        if(transform.position.z>49 || transform.position.z < -49||
            transform.position.x > 49||transform.position.x > 49|| transform.position.y<-2)
        {
            Done();
            SetReward(-1f);
        }

        if (coll == 100)
        {
            SetReward(1f);
            Debug.LogError("获得奖励");
        }
        else if (coll == 1)
        {
            Done();
            SetReward(-1f);
            Debug.Log("碰到障碍------" + GetReward());
        }
        else
        {
            SetReward(0.1f);
        }
    }

    public override void AgentReset()
    {
        coll = 0;
        Debug.Log("重置 代理");
        gameObject.transform.localPosition = new Vector3(-7.89f, -0.85f, 8.85f);
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(-10f, 10f));
        //ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
          //  + gameObject.transform.position;
        //Reset the parameters when the Agent is reset.
       // SetResetParameters();
    }


}
