using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedBullet : Bullet {

    public AnimationCurve curve;

    public float journeyTime = 0.5f;
    private float startTime;
    private float randomValue;

    public override void Start()
    {
        base.Start();
        startTime = Time.time;
        randomValue = GetRandomNormal(0, 4.0f);
    }

    float GetRandomNormal(float mean, float standardDeviation)
    {
        float u1 = 1f - Random.value;
        float u2 = 1f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2f * Mathf.Log(u1)) * Mathf.Sin(2f * Mathf.PI * u2);
        float randNormal = mean + standardDeviation * randStdNormal;

        return randNormal;
    }

    public override void Update()
    {
        // The center of the arc
        Vector3 center = (startPosition + endPosition) * 0.5F;

        // move the center a bit downwards to make the arc vertical
        center -= new Vector3(Mathf.Abs(randomValue), randomValue, 0);

        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = startPosition - center;
        Vector3 setRelCenter = endPosition - center;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = curve.Evaluate((Time.time - startTime) / journeyTime);

        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;

        if (fracComplete >= 1.0f) Destroy(gameObject);

        base.Update();
    }
}
