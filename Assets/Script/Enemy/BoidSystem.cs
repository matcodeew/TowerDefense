using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoidSystem : MonoBehaviour
{
    public Transform BoidPrefab;
    public Transform Attractor;
    public int NumberOf;
    Dictionary<Vector3, List<Boids>> regions = new();
    [SerializeField] private BoidsSetting Setting;
    Boids[] boids;


    private void Start()
    {
        boids = new Boids[NumberOf];
        for (int i = 0; i < NumberOf; i++)
        {
            boids[i] = new Boids { transform = Instantiate(BoidPrefab, transform), Velocity = Random.onUnitSphere, Attractor = boids[i].Attractor};
        }
    }

    private void Update()
    {
        ApplyNExtVelocity();
        ComputeNextVelocity();
    }
    void ApplyNExtVelocity()
    {
        for (int i = 0; i < boids.Length; i++)
        {
            boids[i].ApplyNextVelocity(boids, Setting);
        }
    }
    void ComputeNextVelocity()
    {
        for (int i = 0; i < boids.Length; i++)
        {
            boids[i].ComputeNextVelocity(boids, Setting);
        }
    }
    public struct Boids
    {
        public Transform transform;
        public Transform Attractor;
        public Vector3 Velocity;
        public Vector3 NextVelocity;

        Vector3Int region;

        public void ComputeNextVelocity(Boids[] boids, BoidsSetting setting)
        {
            NextVelocity = Vector3.zero;
            Vector3 Alignement = Vector3.zero;
            Vector3 Cohesion = Vector3.zero;
            Vector3 Avoidance = Vector3.zero;
            Vector3 Attraction = Vector3.zero;
            for (int i = 0; i < boids.Length; i++)
            {
                if (boids[i].transform == transform) { continue; }

                //Alignement
                Vector3 direction = boids[i].Velocity;
                float distance = Vector3.Distance(transform.position, boids[i].transform.position);
                Alignement += Vector3.ClampMagnitude(direction / Mathf.Max(distance / 0.01f), 1);

                //Avoidance
                direction = (transform.position - boids[i].transform.position);
                distance = direction.magnitude / setting.FarThreshold;
                Avoidance += direction.normalized * (1 - distance);

                //Cohesion
                direction = -direction;
                if (distance > setting.FarThreshold)
                {
                    Cohesion += Vector3.ClampMagnitude(direction.normalized * (distance - 1), 1);
                }
            }

            Vector3 dirAttractor = Attractor.transform.position - transform.position;
            Attraction += dirAttractor.normalized;


            NextVelocity = Alignement * setting.Alignemant + Avoidance * setting.Avoidance + Cohesion * setting.Cohesion + Attraction * setting.Atraction;
            NextVelocity.Normalize();
        }
        public void ApplyNextVelocity(Boids[] boids, BoidsSetting setting)
        {
            Velocity = Vector3.Slerp(Velocity, NextVelocity, setting.TurnRate);
            transform.position += Velocity * setting.Speed * Time.deltaTime;
        }
    }

    [System.Serializable]   
    public class BoidsSetting
    {
        public float Avoidance;
        public float Cohesion;
        public float Alignemant;
        public float Atraction;
        public float FarThreshold;
        public float Speed;
        public float TurnRate;
    }
}
