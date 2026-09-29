using System;
using System.Collections.Generic;
using UnityEngine;

namespace skillveri_Assignment
{
    public class ArtifactDetector : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private LayerMask artifactLayer;

        [Header("Settings")]
        [SerializeField] private float changeCooldown = 0.05f;
        [SerializeField] private float range = 100f;
        [SerializeField] private Camera camerafov;
        [SerializeField] private float detectionInterval = 0.1f;
        private float detectionTimer;
        public event Action<IArtifact> OnArtifactChanged;
        private IArtifact currentArtifact;
        private IArtifact pendingArtifact;
        private float pendingTimer;

        private readonly Collider[] results = new Collider[16];
        private readonly Dictionary<Collider, IArtifact> artifactCache = new Dictionary<Collider, IArtifact>();

        private void Update()
        {
            detectionTimer += Time.deltaTime;

            if (detectionTimer >= detectionInterval)
            {
                detectionTimer = 0f;
                DetectArtifacts();
            }
        }

        private void DetectArtifacts()
        {
            // CheckPlayerSeeingobject();
            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                detectionRadius,
                results,
                artifactLayer
            );

            IArtifact nearestArtifact = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider hit = results[i];
                if (hit == null) continue;

                if (!artifactCache.TryGetValue(hit, out IArtifact artifact))
                {
                    if (hit.TryGetComponent(out artifact) && artifact.Data != null)
                    {
                        artifactCache[hit] = artifact;
                    }
                    else
                    {
                        artifactCache[hit] = null;
                        continue;
                    }
                }

                if (artifact == null) continue;

                float distance = (artifact.Transform.position - transform.position).sqrMagnitude;

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestArtifact = artifact;
                }
            }

            if (nearestArtifact == currentArtifact)
            {
                pendingArtifact = null;
                pendingTimer = 0f;
                return;
            }

            if (nearestArtifact == pendingArtifact)
            {
                pendingTimer += Time.deltaTime;

                if (pendingTimer >= changeCooldown)
                {
                    currentArtifact = pendingArtifact;
                    pendingArtifact = null;
                    pendingTimer = 0f;

                    OnArtifactChanged?.Invoke(currentArtifact);
                }
            }
            else
            {
                pendingArtifact = nearestArtifact;
                pendingTimer = 0f;
            }
        }
        void CheckPlayerSeeingobject()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(camerafov.transform.position, camerafov.transform.forward, out raycastHit, range, artifactLayer))
            {
                IArtifact artifact = raycastHit.transform.GetComponentInParent<IArtifact>();

                Debug.Log("Seeing SomeObjects" + artifact.Data.name);
            }
        }
        public void ClearCache()
        {
            artifactCache.Clear();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}