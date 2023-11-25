using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        int interval = 3;
        int counter = 0;

        while(elapsed < duration)
        {
            counter++;
            if (counter % interval == 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + magnitude, transform.eulerAngles.z);
                magnitude *= -0.98f;
            }

            elapsed += Time.deltaTime;

            yield return null;
        }
    }
}
