using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AI_MovetoTarget
{
    // Start is called before the first frame update
    public void MovetoPlayer(Transform me,Transform target,float speed)
    {
        me.position = Vector3.MoveTowards(me.position,
        target.position, speed * Time.deltaTime);
        MathMover.Lookat(target.position, me);
    }
    

}
