using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    Mesh mesh;
    int team;
    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
    private void Update()
    {
        int rayCount = 180;
        float angle = 0;
        float viewDistance = Camera.main.orthographicSize * 2.1f;
        float angleIncrease = 360 / rayCount;
        Vector3 origin = transform.parent.position;

        transform.position = new Vector3(0,0 ,50);


        Vector3[] vertices = new Vector3[rayCount +3];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[(rayCount + 1) * 3];

        vertices[0] = origin ;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i <= rayCount +1; i++)
        {
            if(i+1 == rayCount + 2)
            {
                angle = 0;
            }
            Vector3 vertex;

            RaycastHit2D ray = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, GameManager.Instance.wallLayer);
            if (ray.collider == null)
            {
                ray = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, GameManager.Instance.playerWallLayer);
                if (ray && ray.rigidbody.GetComponent<Barrier>().deniesVision && ray.collider.GetComponent<Barrier>().user.team != team)
                {
                    vertex = ray.point;
                }
                else
                {
                    vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
                }
            }
            else
            {
                ray = Physics2D.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), viewDistance, GameManager.Instance.playerWallLayer + GameManager.Instance.wallLayer);
                
                vertex = ray.point;
            }


            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            } 

            vertexIndex++;

            angle -= angleIncrease;
        }


        mesh.vertices = vertices; 
        mesh.uv = uv;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
    }
}
