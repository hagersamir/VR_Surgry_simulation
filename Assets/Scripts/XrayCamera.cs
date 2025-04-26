using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class XRayEffect : MonoBehaviour
{
    public Shader xrayShader;
    [Range(0f, 1f)] public float transparency = 0.5f; // Transparency slider
    public RenderTexture FakeForOnAndOff; // Assign in Inspector
    public RenderTexture CamScreen; // Assign in Inspector

    private Material xrayMaterial;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        if (xrayShader != null)
        {
            xrayMaterial = new Material(xrayShader);
            cam.SetReplacementShader(xrayShader, "RenderType");
        }
    }

    private void Update()
    {
        if (xrayMaterial != null)
        {
            Shader.SetGlobalFloat("_Transparency", transparency);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(SwitchRenderTexture());
        }
    }

    private IEnumerator SwitchRenderTexture()
    {
        if (FakeForOnAndOff != null && CamScreen != null)
        {
            cam.targetTexture = CamScreen;
            yield return new WaitForSeconds(1f);
            cam.targetTexture = FakeForOnAndOff;
        }
    }


}
