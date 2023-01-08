using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    public class Layers
    { 
        public static int PhysicalObstacleMask = LayerMask.NameToLayer("Default");
    }


    public static bool LineOfSight(Transform from, Transform target, float fovAngle, bool debugDraw = false)
    {
        Vector3 direction = (target.position - from.position).normalized;
        if (Vector3.Angle(from.forward, direction) < fovAngle / 2f)
        {
            if (!Physics.Linecast(from.position, target.position, Layers.PhysicalObstacleMask))
            {
                if (debugDraw)
                {
                    Debug.DrawLine(from.position, target.position, Color.green);
                }

                return true;
            }
        }

        if (debugDraw)
        {
            Debug.DrawLine(from.position, target.position, Color.red);
        }

        return false;
    }




}
