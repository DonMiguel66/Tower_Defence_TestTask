using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [RequireComponent(typeof(LineRenderer))]
    internal class CircleBorderRenderer : MonoBehaviour
    {
        [Range(0.1f, 100f)] public float radius = 1.0f;

        [Range(3, 256)] public int numSegments = 128;

        private void Start()
        {

            //DoRenderer();
        }

        public void DoRenderer(float radius)
        {
            LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
            Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
            lineRenderer.SetColors(c1, c1);
            lineRenderer.SetWidth(0.1f, 0.1f);
            lineRenderer.SetVertexCount(numSegments + 1);
            lineRenderer.useWorldSpace = false;

            float deltaTheta = (float) (2.0 * Mathf.PI) / numSegments;
            float theta = 0f;

            for (int i = 0; i < numSegments + 1; i++)
            {
                float x = radius * Mathf.Cos(theta);
                float y = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, y, 0);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}