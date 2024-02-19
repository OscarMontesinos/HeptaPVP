using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController playerController;
    public float speed;
    public float rSpeed;
    public float zPos;
    float camPos;
    public float camDistance;
    bool moveAlone;
    public float speedZoom;
    public Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
    }

    private void Start()
    {
        zPos = transform.position.z;
        if (playerController != null)
        {
            playerController.cam = transform.GetChild(0).GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController == null )
        {
            moveAlone = true;
            cam = FindObjectOfType<Camera>();
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, playerController.transform.GetChild(0).rotation, rSpeed * Time.deltaTime);
            if (camPos < camDistance)
            {
                transform.GetChild(0).Translate(transform.up * Time.deltaTime * speed*3);
                camPos += Time.deltaTime * speed*3;
            }
        }
        else
        {
            if (camPos > 0)
            {
                transform.GetChild(0).Translate(transform.up * Time.deltaTime * -speed*3);
                camPos += Time.deltaTime * -speed*3;
            }
        }
        transform.GetChild(0).eulerAngles = new Vector3(0, 0, 0);
        if (moveAlone)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                cam.orthographicSize += speedZoom;
                if (cam.orthographicSize > 25)
                {
                    cam.orthographicSize = 25;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                cam.orthographicSize -= speedZoom;
                if (cam.orthographicSize < 1)
                {
                    cam.orthographicSize = 2;
                }

            }
            if (Input.GetKey(KeyCode.W))
            {
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y + speed * Time.deltaTime, cam.transform.position.z);
            }
            if (Input.GetKey(KeyCode.S))
            {
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - speed * Time.deltaTime, cam.transform.position.z);
            }
            if (Input.GetKey(KeyCode.D))
            {
                cam.transform.position = new Vector3(cam.transform.position.x + speed * Time.deltaTime, cam.transform.position.y, cam.transform.position.z);
            }
            if (Input.GetKey(KeyCode.A))
            {
                cam.transform.position = new Vector3(cam.transform.position.x - speed * Time.deltaTime, cam.transform.position.y, cam.transform.position.z);
            }
        }
    }
}
