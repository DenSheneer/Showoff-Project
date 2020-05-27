using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplineMesh;

[RequireComponent(typeof(Spline))]
public class LinearMeshAlongSpline : MonoBehaviour
{
    private Spline spline;
    private MeshBender meshBender;
    private GameObject meshObjectHolder;

    [SerializeField]
    private Mesh mesh = null;
    [SerializeField]
    private Material material = null;
    [SerializeField]
    private Vector3 rotation = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 scale = new Vector3(0, 0, 0);

    void Start()
    {
        // Pre generate the mesh
        string generatedName = "LinearMesh-" + GetType().Name;
        var generatedTranform = transform.Find(generatedName);
        meshObjectHolder = generatedTranform != null ? generatedTranform.gameObject : UOUtility.Create(generatedName, gameObject,
            typeof(MeshFilter),
            typeof(MeshRenderer),
            typeof(MeshBender));

        meshObjectHolder.GetComponent<MeshRenderer>().material = material;

        meshBender = meshObjectHolder.GetComponent<MeshBender>();
        spline = GetComponent<Spline>();

        meshBender.Source = SourceMesh.Build(mesh)
            .Rotate(Quaternion.Euler(rotation))
            .Scale(scale);
        meshBender.Mode = MeshBender.FillingMode.Repeat;
        meshBender.SetInterval(spline, 0);

        if (meshObjectHolder != null)
        {
            meshBender.SetInterval(spline, spline.Length * 0.99f);
            meshBender.ComputeIfNeeded();
        }
    }
    

    public void UpdateMeshFillingMode(MeshBender.FillingMode fillingMode)
    {
        meshBender.Mode = fillingMode;
    }

    public void UpdateMeshInterval(float pInterval)
    {
        float rate = 0;
        if (pInterval < 0)
            rate = 0;
        else if (pInterval >= 1)
            rate = 0.99f;
        else
            rate = pInterval;

        if (meshObjectHolder != null)
        {
            meshBender.SetInterval(spline, spline.Length * rate);
            meshBender.ComputeIfNeeded();
        }
    }

    public Vector3 GetIntervalPosition(float pInterval)
    {
        float rate = 0;
        if (pInterval < 0)
            rate = 0;
        else if (pInterval >= 1)
            rate = 0.99f;
        else
            rate = pInterval;

        CurveSample sample = spline.GetSample(rate);

        return sample.location;
    }
}
