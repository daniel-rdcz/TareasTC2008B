using UnityEngine;

public class SeguirCoche : MonoBehaviour
{
    public GameObject coche; // Asigna el coche desde el Inspector

    public float velocidad = 5f; // Velocidad de seguimiento

    void Update()
    {
        if (coche != null)
        {
            // Obtén la posición actual del objeto que sigue
            Vector3 posicionActual = transform.position;

            // Obtén la posición del coche
            Vector3 posicionCoche = coche.transform.position;

            // Ajusta la posición en el eje y para mantener la vista lateral
            posicionActual.y = posicionCoche.y;

            // Calcula la dirección hacia la posición del coche
            Vector3 direccion = (posicionCoche - posicionActual).normalized;

            // Calcula la nueva posición hacia la cual mover el objeto que sigue
            Vector3 nuevaPosicion = posicionActual + direccion * velocidad * Time.deltaTime;

            // Aplica la nueva posición
            transform.position = nuevaPosicion;
        }
    }
}
