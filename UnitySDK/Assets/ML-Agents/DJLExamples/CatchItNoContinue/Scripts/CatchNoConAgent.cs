using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchNoConAgent :  Agent
{
    private BasicAcademy m_Academy;
    int m_Position;
    Vector3 mPostion;
    float dis;
    GameObject fianl;
    Ray mRay;
    int ball_layer;
    int goal_layer;
    int coll;
    float ray = 0;
    float lastDis = 10000;

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
        // AddVectorObs(ray);
        //AddVectorObs(dis); //躲避障碍
        //  AddVectorObs(transform.eulerAngles);
        //   AddVectorObs(transform.localPosition);
        AddVectorObs(fianl.transform.localPosition - transform.localPosition); //寻找终点,距离远近
        //  AddVectorObs(coll);
        //  AddVectorObs(m_Position);
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        //我们得到几个值，用于旋转和移动
        var actionX = Mathf.Clamp(vectorAction[0], -1f, 1f);
        var actionZ = Mathf.Clamp(vectorAction[1], -1f, 1f);

        //mPostion += actionTransF;

        // gameObject.transform.Rotate(new Vector3(0, 1, 0), actionTransR);

        transform.localPosition += new Vector3(actionX, 0, actionZ);

        if (transform.localPosition.z > 18 || transform.localPosition.z < -18 ||
            transform.localPosition.x > 18 || transform.localPosition.x < -18)
        {
            SetReward(-1f);
            Done();
        }

        //AddReward(-0.01f);
        float nowDis = (transform.localPosition - fianl.transform.localPosition).magnitude;

        //   AddReward(-0.01f);
        // if (coll == 100)
        if (nowDis < 2)
        {
            Debug.LogError("完成目标");
            SetReward(1f);
            Done();
        }
        if (nowDis > (lastDis + 0.5f))
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
        //if (transform.localPosition.z>18 || transform.localPosition.z < -18||
        //    transform.localPosition.x > 18||transform.localPosition.x <-18
        //    || transform.localPosition.y<-2)
        //{
        //    Done();
        //    AddReward(-0.05f);
        //}
        //if (coll == 100)
        //{
        //    Done();
        //    SetReward(1f);
        //    Debug.LogError("获得奖励");
        //}
        //if (coll == 1)
        //{
        //    Done();
        //   // SetReward(-1f);
        //    Debug.Log("碰到障碍---获得惩罚---");
        //}
        //else
        //{
        //    SetReward(0.1f);
        //}
    }

    public override void AgentReset()
    {
        ball_layer = LayerMask.NameToLayer("ball");
        goal_layer = LayerMask.NameToLayer("goal");
        mRay = new Ray();
        fianl = gameObject.transform.parent.Find("fianl").gameObject;
        coll = 0;
        Debug.Log("重置 代理");
        gameObject.transform.localPosition = new Vector3(-7.89f, -0.85f, 8.85f);
        mPostion = gameObject.transform.localPosition;
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(0, 1, 0), Random.Range(-10f, 10f));
        fianl.transform.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), -0.7f, Random.Range(-1.5f, 1.5f));
        lastDis = (transform.localPosition - fianl.transform.localPosition).magnitude;

        //  + gameObject.transform.position;
        //Reset the parameters when the Agent is reset.
        // SetResetParameters();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("wall"))
        {
            Debug.Log("触碰到障碍");
            coll = 1;
            Done();
        }
        if (collision.transform.tag.Equals("goal"))
        {
            // float nowDis = (transform.localPosition - fianl.transform.localPosition).magnitude;
            Debug.LogError("触碰到目标------");
            coll = 100;
        }
    }

    //private void FixedUpdate()
    //{
    //    RaycastHit hitInfo;
    //    if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
    //        , transform.forward, out hitInfo, 10, ball_layer))
    //    {
    //        //if (hitInfo.transform.gameObject.layer.Equals())
    //        //{

    //        //}
    //        ray = 1;
    //        dis = Vector3.Distance(transform.position, hitInfo.transform.position);
    //    }
    //    else if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0)
    //, transform.forward, 10, goal_layer))
    //    {
    //        if (hitInfo.transform != null)
    //        {
    //            dis = Vector3.Distance(transform.position, hitInfo.transform.position);
    //            ray = 2;
    //        }
    //    }
    //    else
    //    {
    //        dis = 0;
    //        ray = 0;
    //    }

    //    //if (!m_Academy.GetIsInference())
    //    //{
    //    //    RequestDecision();
    //    //}
    //}
}
