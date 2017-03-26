using UnityEngine;
using System.Collections;

public class Motor : MonoBehaviour
{

    public Transform lookAt;
    public Vector3 start;
    public Vector3 end;

    public float totalTime = 2f;
    public float time = 0f;
    public float startedTime = 0f;
    public bool moving = false;


    // Update is called once per frame
    void Update()
    {

        if (moving)
        {
            transform.LookAt(lookAt);
            time = Time.time;
            Vector3 moved = Vector3.Lerp(start, end, (time - startedTime) / totalTime);
            lookAt.localPosition = moved;
        }
    }



}
