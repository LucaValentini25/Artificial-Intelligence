using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NPC
{
    [Serializable]
    public struct NpcStats
    {
        public float maxSpeed;
        public float maxForce;
        public float hp;
        [Range(0,30)] public float viewRadius;
        [Range(0,10)] public float separationRadius;
        [Range(0,10)] public float attackRadius;
        [Range(0, 4)]
        public float weightSeparation, weightAlignment, weightCohesion;

        public float arriveDistanceThreshold;
    }
}