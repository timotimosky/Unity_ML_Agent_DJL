using UnityEngine;
using MLAgents;

public class Ball3DAcademy : Academy
{
    public override void AcademyReset()
    {
        float gravity = -resetParameters["gravity"];
        Debug.LogError("ÖØÖÃÖØÁ¦--" + gravity);
        Physics.gravity = new Vector3(0, gravity, 0);
    }

    public override void AcademyStep()
    {
    }
}
