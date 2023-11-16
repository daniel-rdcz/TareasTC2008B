using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    [SerializeField] Vector3 displacement;
    [SerializeField] float angle_wheel;

    [SerializeField] float direction;
    [SerializeField] AXIS rotationAxis;

    [SerializeField] private GameObject[] wheels;
    Mesh mesh;
    Mesh mesh_wheel1;
    Mesh mesh_wheel2;
    Mesh mesh_wheel3;
    Mesh mesh_wheel4;
    Vector3[] baseVertices;
    Vector3[] baseVertices_wheel1;
    Vector3[] baseVertices_wheel2;
    Vector3[] baseVertices_wheel3;
    Vector3[] baseVertices_wheel4;

    Vector3[] newVertices;
    Vector3[] newVertices_wheel1;
    Vector3[] newVertices_wheel2;
    Vector3[] newVertices_wheel3;
    Vector3[] newVertices_wheel4;

    void Start()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = mesh.vertices;

        mesh_wheel1 = wheels[0].GetComponentInChildren<MeshFilter>().mesh;
        baseVertices_wheel1 = mesh_wheel1.vertices;

        mesh_wheel2 = wheels[1].GetComponentInChildren<MeshFilter>().mesh;
        baseVertices_wheel2 = mesh_wheel2.vertices;

        mesh_wheel3 = wheels[2].GetComponentInChildren<MeshFilter>().mesh;
        baseVertices_wheel3 = mesh_wheel3.vertices;

        mesh_wheel4 = wheels[3].GetComponentInChildren<MeshFilter>().mesh;
        baseVertices_wheel4 = mesh_wheel4.vertices;

        newVertices = new Vector3[baseVertices.Length];
        for (int i=0; i < baseVertices.Length; i++)
        {
            newVertices[i] = baseVertices[i];
        }

        newVertices_wheel1 = new Vector3[baseVertices_wheel1.Length];
        for (int i=0; i < baseVertices_wheel1.Length; i++)
        {
            newVertices_wheel1[i] = baseVertices_wheel1[i];
        }

        newVertices_wheel2 = new Vector3[baseVertices_wheel2.Length];
        for (int i=0; i < baseVertices_wheel2.Length; i++)
        {
            newVertices_wheel2[i] = baseVertices_wheel2[i];
        }

        newVertices_wheel3 = new Vector3[baseVertices_wheel3.Length];
        for (int i=0; i < baseVertices_wheel3.Length; i++)
        {
            newVertices_wheel3[i] = baseVertices_wheel3[i];
        }

        newVertices_wheel4 = new Vector3[baseVertices_wheel4.Length];
        for (int i=0; i < baseVertices_wheel4.Length; i++)
        {
            newVertices_wheel4[i] = baseVertices_wheel4[i];
        }

    }

    void Update()
    {
        DoTransform();        
    }

    void DoTransform()
    {
        Vector3 OrgPosWheel1 = new Vector3(2f,0.600000024f,-2.79999995f);
        Vector3 OrgPosWheel2 = new Vector3(-2f,0.600000024f,-2.79999995f);
        Vector3 OrgPosWheel3 = new Vector3(2f,0.600000024f,2.78999996f);
        Vector3 OrgPosWheel4 = new Vector3(-2f,0.600000024f,2.78999996f);

        Matrix4x4 move = Transforms.TranslationMat(displacement.x * Time.time,
                                                      displacement.y * Time.time,
                                                      displacement.z * Time.time);

        Matrix4x4 move_wheels = Transforms.TranslationMat(-displacement.x * Time.time,
                                                      displacement.y * Time.time,
                                                      displacement.z * Time.time);

        Matrix4x4 rotate = Transforms.RotateMat(angle_wheel * Time.time, 
                                                    rotationAxis);

        Matrix4x4 rotate_car = Transforms.RotateMat(direction, AXIS.Y);

        Matrix4x4 moveWheel1 = Transforms.TranslationMat(OrgPosWheel1.x,
                                                           OrgPosWheel1.y,
                                                           OrgPosWheel1.z);

        Matrix4x4 moveWheel2 = Transforms.TranslationMat(OrgPosWheel2.x,
                                                           OrgPosWheel2.y,
                                                           OrgPosWheel2.z);

        Matrix4x4 moveWheel3 = Transforms.TranslationMat(OrgPosWheel3.x,
                                                           OrgPosWheel3.y,
                                                           OrgPosWheel3.z);

        Matrix4x4 moveWheel4 = Transforms.TranslationMat(OrgPosWheel4.x,
                                                           OrgPosWheel4.y,
                                                           OrgPosWheel4.z);                                                  
        
        Matrix4x4 composite = rotate_car * move;

        Matrix4x4 composite_wheel1 = rotate_car * moveWheel1 * move_wheels * rotate;
        Matrix4x4 composite_wheel2 = rotate_car * moveWheel2 * move_wheels * rotate;
        Matrix4x4 composite_wheel3 = rotate_car * moveWheel3 * move_wheels * rotate;
        Matrix4x4 composite_wheel4 = rotate_car * moveWheel4 * move_wheels * rotate;

        for (int i=0; i < newVertices.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices[i].x,
                                       baseVertices[i].y,
                                       baseVertices[i].z,
                                       1);
            newVertices[i] = composite * temp;
        }

        for (int i=0; i < newVertices_wheel1.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices_wheel1[i].x,
                                       baseVertices_wheel1[i].y,
                                       baseVertices_wheel1[i].z,
                                       1);
            newVertices_wheel1[i] = composite_wheel1 * temp;
        }

        for (int i=0; i < newVertices_wheel2.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices_wheel2[i].x,
                                       baseVertices_wheel2[i].y,
                                       baseVertices_wheel2[i].z,
                                       1);
            newVertices_wheel2[i] = composite_wheel2 * temp;
        }

        for (int i=0; i < newVertices_wheel3.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices_wheel3[i].x,
                                       baseVertices_wheel3[i].y,
                                       baseVertices_wheel3[i].z,
                                       1);
            newVertices_wheel3[i] = composite_wheel3 * temp;
        }

        for (int i=0; i < newVertices_wheel4.Length; i++)
        {
            Vector4 temp = new Vector4(baseVertices_wheel4[i].x,
                                       baseVertices_wheel4[i].y,
                                       baseVertices_wheel4[i].z,
                                       1);
            newVertices_wheel4[i] = composite_wheel4 * temp;
        }

        mesh.vertices = newVertices;
        mesh.RecalculateNormals();

        mesh_wheel1.vertices = newVertices_wheel1;
        mesh_wheel1.RecalculateNormals();

        mesh_wheel2.vertices = newVertices_wheel2;
        mesh_wheel2.RecalculateNormals();

        mesh_wheel3.vertices = newVertices_wheel3;
        mesh_wheel3.RecalculateNormals();

        mesh_wheel4.vertices = newVertices_wheel4;
        mesh_wheel4.RecalculateNormals();
    }
}