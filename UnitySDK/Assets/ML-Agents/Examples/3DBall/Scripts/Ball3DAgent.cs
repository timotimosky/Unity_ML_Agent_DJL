using UnityEngine;
using MLAgents;

public class Ball3DAgent : Agent
{
    [Header("Specific to Ball3D")]
    public GameObject ball;
    private Rigidbody m_BallRb;
    private ResetParameters m_ResetParams;

    public override void InitializeAgent()
    {
        m_BallRb = ball.GetComponent<Rigidbody>();
        var academy = FindObjectOfType<Academy>(); //管理中心
        m_ResetParams = academy.resetParameters;

        Debug.LogError("初始化 代理");
        SetResetParameters();
    }

    public override void CollectObservations()
    {
        AddVectorObs(gameObject.transform.rotation.z);//我自身偏移X
        AddVectorObs(gameObject.transform.rotation.x);//我自身偏移Z
        AddVectorObs(ball.transform.position - gameObject.transform.position);//距离远近
        AddVectorObs(m_BallRb.velocity); //观察 球的方向速度
    }

    //vectorAction有两种模式，一种是离散，一种是连续。比如在这个demo中我把act设置为离散，因为角色的行为无非是左转右转和向前，
    //那么我们把act为0时作为角色向前的信号，1和2作为角色向左和向右，用离散就合适了，
    //离散和连续在Brain中的action设置即可。
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.Continuous)
        {
            //Debug.LogError("如果是连续空间");
            var actionZ = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
            var actionX = 2f * Mathf.Clamp(vectorAction[1], -1f, 1f);

            if ((gameObject.transform.rotation.z < 0.25f && actionZ > 0f) ||
                (gameObject.transform.rotation.z > -0.25f && actionZ < 0f))
            {
                gameObject.transform.Rotate(new Vector3(0, 0, 1), actionZ);
            }

            if ((gameObject.transform.rotation.x < 0.25f && actionX > 0f) ||
                (gameObject.transform.rotation.x > -0.25f && actionX < 0f))
            {
                gameObject.transform.Rotate(new Vector3(1, 0, 0), actionX);
            }
        }
        //如果掉落
        if ((ball.transform.position.y - gameObject.transform.position.y) < -2f ||
            Mathf.Abs(ball.transform.position.x - gameObject.transform.position.x) > 3f ||
            Mathf.Abs(ball.transform.position.z - gameObject.transform.position.z) > 3f)
        {
            Done();
            SetReward(-1f);
            Debug.LogError("掉落------" + GetReward());
        }
        else //如果没掉落
        {
            SetReward(0.1f);
            //Debug.LogError("保持------" + GetReward());
        }
    }

    public override void AgentReset()
    {
        Debug.LogError("重置 代理");
        gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        gameObject.transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
        gameObject.transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));
        m_BallRb.velocity = new Vector3(0f, 0f, 0f);
        ball.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f))
            + gameObject.transform.position;
        //Reset the parameters when the Agent is reset.
        SetResetParameters();
    }

    public void SetBall()
    {
        //Set the attributes of the ball by fetching the information from the academy
        m_BallRb.mass = m_ResetParams["mass"];
        var scale = m_ResetParams["scale"];
        ball.transform.localScale = new Vector3(scale, scale, scale);
    }

    public void SetResetParameters()
    {
        SetBall();
    }
}
