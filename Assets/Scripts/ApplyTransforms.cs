/*
Tarea 2
José Daniel Rodríguez Cruz 	
2023-11-21
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyTransforms : MonoBehaviour
{
    public Vector3 displacement;
    [SerializeField] AXIS rotationAxis;
    [SerializeField] GameObject wheelPrefab;
    [SerializeField] int numWheels = 4;

    Mesh mesh;
    Mesh[] meshWheel;
    Vector3[] baseVertices;
    Vector3[] newVertices;
    Vector3[][] baseVerticesWheel;
    Vector3[][] newVerticesWheel;
    Vector3[] wheelCoordinates;



    // Funcion de inicio
    void Start()
    {
        // Coordenadas de las ruedas
        wheelCoordinates = new Vector3[numWheels];
        wheelCoordinates[0] = new Vector3(1.92900002f,0.672999978f,-2.79999995f);
        wheelCoordinates[1] = new Vector3(-1.92900002f,0.672999978f,-2.79999995f);
        wheelCoordinates[2] = new Vector3(-1.92900002f,0.672999978f,2.77699995f);
        wheelCoordinates[3] = new Vector3(1.92900002f,0.672999978f,2.77699995f);

        // Obtener el mesh del objeto
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        baseVertices = mesh.vertices;

        // Crear un nuevo arreglo de vertices
        newVertices = new Vector3[baseVertices.Length];
        for (int i = 0; i < baseVertices.Length; i++) {
            newVertices[i] = baseVertices[i];
        }
        baseVerticesWheel = new Vector3[numWheels][];
        newVerticesWheel = new Vector3[numWheels][];
        meshWheel = new Mesh[numWheels];

        for (int x = 0; x < numWheels; x++) {
            GameObject prefabWheel = Instantiate(wheelPrefab, transform.position, Quaternion.identity);
            meshWheel[x] = prefabWheel.GetComponentInChildren<MeshFilter>().mesh;
            baseVerticesWheel[x] = meshWheel[x].vertices;
            newVerticesWheel[x] = new Vector3[baseVerticesWheel[x].Length];
            for (int i = 0; i < baseVerticesWheel.Length; i++) {
                newVerticesWheel[x][i] = baseVerticesWheel[x][i];
            }
        } 
    }

    // Funcion de actualizacion
    void Update()
    {
        DoTransform();
    }

    // Funcion para aplicar transformaciones
    void DoTransform()
    {
        // Matriz de movimiento
        Matrix4x4 move = Transforms.TranslationMat(displacement.x * Time.time, displacement.y * Time.time, displacement.z * Time.time);

        // Angulo de rotacion
        float directionAngle = Mathf.Atan2(displacement.z, displacement.x) * Mathf.Rad2Deg - 270;
        // Matriz de rotacion del Carro y las ruedas
        Matrix4x4 rotateObject = Transforms.RotateMat(-directionAngle,  AXIS.Y);

        // Matriz de composicion para el movimiento del carro
        Matrix4x4 compositeCar = move * rotateObject;

        // Calculate rotation
        float rotWheelsAngle = -160 * displacement.magnitude;
        // Matriz de rotacion de las ruedas
        Matrix4x4 wheelsGiro = Transforms.RotateMat(rotWheelsAngle * Time.time, AXIS.X);

        // Multiplicar los vertices por la matriz de composicion
        for (int i=0; i<newVertices.Length; i++) {
            Vector4 temp = new Vector4(baseVertices[i].x,
                                       baseVertices[i].y,
                                       baseVertices[i].z,
                                       1);
            newVertices[i] = compositeCar * temp;
        }

        // Multiplicar los vertices por la matriz de composicion para las ruedas
        for (int v=0; v<numWheels; v++) {
            for (int a=0; a<newVerticesWheel[v].Length; a++) {
                Vector4 temp = new Vector4(baseVerticesWheel[v][a].x,
                                           baseVerticesWheel[v][a].y,
                                           baseVerticesWheel[v][a].z,
                                           1);

                // Matriz de composicion para el movimiento de las ruedas
                Matrix4x4 translateWheels = Transforms.TranslationMat(wheelCoordinates[v].x, wheelCoordinates[v].y, wheelCoordinates[v].z);
                Matrix4x4 compositeWheels = move * rotateObject *  translateWheels * wheelsGiro;
                newVerticesWheel[v][a] = compositeWheels * temp;
            }
            meshWheel[v].vertices = newVerticesWheel[v];
            meshWheel[v].RecalculateNormals();
            meshWheel[v].RecalculateBounds();
        }

        // Remplazar los vertices del mesh por los nuevos
        mesh.vertices = newVertices;

        // Recalcular las normales y los bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
