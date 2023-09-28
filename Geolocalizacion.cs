using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Geolocalizacion : MonoBehaviour
{
    public Text textoCoord;

    public float rangoDeteccion = 25f;

    // Coordenadas del punto 1
    public float latitude;
    public float longitude;

    // Coordenadas del punto 2
    public float lat2 = 4.663513f;
    public float lon2 = -74.058313f;

    public const float EarthRadiusMeters = 6371000.0f;

    void Start()
    {
        StartLocation();
        textoCoord.text = "status" + Input.location.status;
        Input.location.Start();

        float distance = CalculateDistance(latitude, longitude, 4.663513f, -74.058313f);


    }

    public float CalculateDistance(float latitude, float longitude, float lat2, float lon2)
    {
        // Convierte las latitudes y longitudes de grados a radianes
        latitude = Mathf.Deg2Rad * latitude;
        longitude = Mathf.Deg2Rad * longitude;
        lat2 = Mathf.Deg2Rad * lat2;
        lon2 = Mathf.Deg2Rad * lon2;

        // Diferencia en latitud y longitud
        float dLat = lat2 - latitude;
        float dLon = lon2 - longitude;

        // Fórmula de Haversine
        float a = Mathf.Pow(Mathf.Sin((dLat / 2)), 2) + Mathf.Cos(latitude) * Mathf.Cos(lat2) * Mathf.Pow(Mathf.Sin((dLon / 2)), 2);
        float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));

        // Distancia en metros
        float distance = EarthRadiusMeters * c;

        return distance;
    }

    IEnumerator StartLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
            textoCoord.text = "La geolocalización no está habilitada por el usuario.";
            yield break;
        }
        // Start location services
        Input.location.Start();

        // Wait for location services to initialize
        while (Input.location.status == LocationServiceStatus.Initializing)
            yield return new WaitForSeconds(1);

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            textoCoord.text = "Error al iniciar los servicios de ubicación.";
            yield break;
        }

        // Verifica si la ubicación está dentro del rango deseado
        if (IsLocationWithinRange())
        {
            // Habilita la detección de imagen aquí
            // Puedes agregar tu lógica para habilitar la detección de imagen en este punto
        }
        else
        {
            textoCoord.text = "Ubicación fuera del rango de detección de imagen.";
        }
    }

    private bool IsLocationWithinRange()
    {
        if (Input.location.status != LocationServiceStatus.Running)
            return false;

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;

        float distance = CalculateDistance(latitude, longitude, 4.663513f, -74.058313f);
        Debug.Log("Distancia en mts: " + distance);

        /* // Calcula la distancia entre la ubicación actual y un punto de referencia (por ejemplo, tu punto de interés)
        float distancia = CalculateDistance(latitude, longitude, 4.663513, -74.058313);*/

        return distance <= rangoDeteccion;
    }

    
    // Update is called once per frame
    void Update()

    {
        textoCoord.text = "status" + Input.location.status;
        // Get the device's location
        if (Input.location.status == LocationServiceStatus.Running)
        {
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;
            float altitude = Input.location.lastData.altitude;

            textoCoord.text = "Latitud: " + latitude + "\nLongitud: " + longitude + "\nAltitud: " + altitude;
        }
        else {
             StartLocation();
             Input.location.Start();
        }
    }


}
