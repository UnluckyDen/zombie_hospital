using UnityEngine;

public class ShaderTest : MonoBehaviour
{
    public Shader shader;
    public MeshRenderer meshRenderer;
    private float _transparent;

    private void Start()
    {
        meshRenderer.material.shader = shader;
    }

    private void Update()
    {
        _transparent += Time.deltaTime;
        meshRenderer.material.SetFloat("Vector1_4DA184CF", _transparent);
    }
}
