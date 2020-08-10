using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathNet.Numerics.RootFinding;
using System.Diagnostics;

public class KeplersLaw : MonoBehaviour
{

    public static double gravitationalParameter = 1e-5;

    public static void SphericalToCartesian(float radius, float polar, float elevation, out Vector3 outCart)
    {
        float a = radius * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polar);
        outCart.y = radius * Mathf.Sin(elevation);
        outCart.z = a * Mathf.Sin(polar);
    }


    public static void CartesianToSpherical(Vector3 cartCoords, out float outRadius, out float outPolar, out float outElevation)
    {
        if (cartCoords.x == 0)
            cartCoords.x = Mathf.Epsilon;
        outRadius = Mathf.Sqrt((cartCoords.x * cartCoords.x)
                        + (cartCoords.y * cartCoords.y)
                        + (cartCoords.z * cartCoords.z));
        outPolar = Mathf.Atan(cartCoords.z / cartCoords.x);
        if (cartCoords.x < 0)
            outPolar += Mathf.PI;
        outElevation = Mathf.Asin(cartCoords.y / outRadius);
    }

    private static int POS_SIZE = 32;

    public double eccentricity = 0.9;
    public double semiMajorAxis = 300;

    public LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = POS_SIZE * 3;
        //lr.SetPositions(new Vector3[POS_SIZE]);
        //for (int i = 0; i < POS_SIZE; i++)
        //{
        //    lr.SetPosition(i, new Vector3());
        //}
        updateLineRenderer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called once per frame
    void OnValidate()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = POS_SIZE * 3;
        updateLineRenderer();
    }

    private void updateLineRenderer()
    {
        Vector3 p = new Vector3();
        Vector3 p2 = new Vector3();
        for (int i = 0; i < POS_SIZE; i++)
        {
            double meanAnomaly = ((float)i / (float)POS_SIZE) * 2 * Math.PI;
            double trueAnomaly = trueAnomalyFromEccentricAnomaly(eccentricity, eccentricAnomaly(meanAnomaly, eccentricity));
            double r = radius(semiMajorAxis, eccentricity, trueAnomaly);
            SphericalToCartesian((float)r, 0f, (float)trueAnomaly, out p);
            UnityEngine.Debug.Log(p);
            UnityEngine.Debug.Log(trueAnomaly);
            UnityEngine.Debug.Log(r);
            SphericalToCartesian((float) (r + semiMajorAxis * 0.05), 0f, (float)trueAnomaly, out p2);
            lr.SetPosition(i * 3, p);
            lr.SetPosition(i * 3 + 1, p2);
            lr.SetPosition(i * 3 + 2, p);
        }
    }

    public static double meanAnomalyFromEccentricAnomaly(double eccentricAnomaly, double eccentricity)
    {
        return eccentricAnomaly - eccentricity * Math.Sin(eccentricAnomaly);
    }

    public static double eccentricAnomalyFromTrueAnomaly(double trueAnomaly, double eccentricity)
    {
        return Math.Atan(Math.Sqrt(1 - Math.Pow(eccentricity, 2)) * Math.Sin(trueAnomaly) / (eccentricity + Math.Cos(trueAnomaly)));
    }

    public static double radius(double semiMajorAxis, double eccentricity, double trueAnomaly)
    {
        return semiMajorAxis * (1 - Math.Pow(eccentricity, 2)) / (1 + eccentricity * Math.Cos(trueAnomaly));
    }

    public static double orbitalPeriod(double gravitationalParameter, double semiMajorAxis)
    {
        return 2 * Math.PI * Math.Sqrt(Math.Pow(semiMajorAxis, 3) / gravitationalParameter);
    }

    public static double meanAnomaly(double period, double t)
    {
        double n = 2 * Math.PI / period;
        return n * t;
    }

    public static double eccentricAnomaly(double meanAnomaly, double eccentricity)
    {
        Func<double, double> f = (E) => E - eccentricity * Math.Sin(E) - meanAnomaly;
        Func<double, double> df = (E) => 1 - eccentricity * Math.Cos(E);
        return NewtonRaphson.FindRoot(f, df, -2*Math.PI, 2*Math.PI);
    }

    public static double trueAnomalyFromEccentricAnomaly(double eccentricity, double eccentricAnomaly)
    {
        return Math.Atan(Math.Sqrt((1 + eccentricity) * Math.Pow(Math.Tan(eccentricAnomaly / 2), 2) / (1 - eccentricity))) * 2;
    }
}
