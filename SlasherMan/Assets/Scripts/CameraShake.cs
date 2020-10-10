using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 shakeTarget;

    public float speed = 10f;

    private float duration = 0;
    private float magnitude = 0;
    private float startShake = -1;

    public AnimationCurve magnitudeCoef;

    void Update()
    {
        if(magnitude > 0)
        {
            //adjusting time
            float t = (Time.realtimeSinceStartup - startShake) / duration;

            if(t >= 1)
            {
                magnitude = 0;
                duration = 0;
                transform.localPosition = Vector3.zero;
                return;
            }

            float actualMagnitude = magnitudeCoef.Evaluate(t) * magnitude;

            //Selecting new target when previous one reached
            if (Vector3.Distance(shakeTarget, transform.localPosition) < 0.1f)
            {
                do
                {
                    Vector2 flatTarget = Random.onUnitSphere;
                    shakeTarget = new Vector3(flatTarget.x, flatTarget.y, Random.value);
                    shakeTarget.x *= actualMagnitude;
                    shakeTarget.y *= actualMagnitude;

                } while (Vector3.Distance(shakeTarget, transform.localPosition) < actualMagnitude / 2);
            }

            //Moving camera to target
            transform.localPosition += (shakeTarget - transform.localPosition) * speed * Time.deltaTime;
        }
    }

    public void shakeCamera(float duration, float magnitude)
    {
        if(this.magnitude < magnitude)
        {
            this.duration = Mathf.Max(this.duration, duration);
            this.magnitude = Mathf.Max(this.magnitude, magnitude);
            startShake = Time.realtimeSinceStartup;
            shakeTarget = transform.localPosition;
        }
    }
}
