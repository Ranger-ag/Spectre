using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    public Camera camera;
    public Transform ball, cameraPos;
    private Transform progress;
    public ballSettings ballSet;
    public cameraSettings cameraSet;
    public float levelSpeed = 8;
    public float sensePref = 1;
    public float cameraSense = 1;
    public bool rolling = true;
    float[] smooth = new float[] { 0, 0 };
    float cameraSpeed;
    float speed = -1;

    public Rigidbody rb;
    public float jumpHeight = 30;

    bool run = true;
    bool track = true;
    bool lose = false;
    bool win = false;

    public float startPos = 0;
    public float widthSize = Screen.width / Screen.height;
    public bool preview = false;

    Vector3[] StartPoint = new Vector3[3];
    void Start()
    {
        StartLevel();
        sensePref = (PlayerPrefs.GetFloat("Sensibility") / 2) + 0.5f;
        cameraSpeed = levelSpeed;
        StartPoint[0] = cameraPos.position;
        StartPoint[1] = ball.position;
        //StartPoint[2] = progress.position;
        StartPoint[0].z += startPos;
        StartPoint[1].z += startPos;
        StartPoint[2].z += startPos;
        track = !preview;
        cameraPos.position = StartPoint[0];
        ball.position = StartPoint[1];
        //progress.position = StartPoint[2];
    }
    // Update is called once per frame
    void Update()
    {
        if (widthSize < 1)
            cameraSet.divider = 1.5f;
        else
            cameraSet.divider = 2;
        if (run)
        {
            if (!lose && !win && track)
            {
                UpdateBall();
            }
            UpdateCamera();
            UpdateProgress();
        }
        if (lose)
        {
            cameraSpeed = Mathf.Clamp(cameraSpeed - Time.deltaTime * 5, 0, float.MaxValue);
        }
        if (win)
        {
            if (speed == -1)
                speed = cameraSpeed;
            cameraSpeed = Mathf.Clamp(cameraSpeed - Time.deltaTime * 0.75f * speed, 0, float.MaxValue);
        }
    }
    public void StartLevel()
    {
        run = true;
    }
    public void ProcessLevel(int id)
    {
        if (id == 1)
            run = true;
        else if (id == 2)
            lose = true;
        else if (id == 3)
            ResetPoses();
        else if (id == 4)
            win = true;
    }
    void ResetPoses()
    {
        cameraPos.position = StartPoint[0];
        ball.position = StartPoint[1];
        //progress.position = StartPoint[2];
        run = false;
        lose = false;
        win = false;
        cameraSpeed = levelSpeed;
    }
    void UpdateProgress()
    {
        if (!lose && rolling)
        {
            ball.position = new Vector3(ball.position.x, ball.position.y, ball.position.z + (Time.deltaTime * levelSpeed));
        }
        cameraPos.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z + (Time.deltaTime * cameraSpeed));
        //progress.position = new Vector3(0, 0, progress.position.z + Time.deltaTime * levelSpeed);
    }
    void UpdateCamera()
    {
        Vector3 newCameraPos = cameraPos.position;
        newCameraPos.x = Mathf.SmoothDamp(newCameraPos.x, ball.position.x / cameraSet.divider, ref smooth[1], cameraSet.speed);
        cameraPos.position = newCameraPos;
    }
    void UpdateBall()
    {
        Vector3 point = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        point.x -= camera.transform.position.x;
        point *= ballSet.sense * sensePref;
        Vector3 newBallPos = ball.position;
        newBallPos.x = Mathf.SmoothDamp(newBallPos.x, point.x, ref smooth[0], ballSet.speed);
        ball.position = newBallPos;
    }
    public void Jump(float jumpHeight)
    {
        rb.velocity = new Vector3(rb.velocity.x, 4, rb.velocity.z);
        rb.AddForce(0, jumpHeight, 0, ForceMode.Impulse);
    }
    [System.Serializable]
    public class ballSettings
    {
        public float sense = 15;
        public float speed = 0.05f;
    }
    [System.Serializable]
    public class cameraSettings
    {
        public float divider = 0.75f;
        public float speed = 0.05f;
    }
}