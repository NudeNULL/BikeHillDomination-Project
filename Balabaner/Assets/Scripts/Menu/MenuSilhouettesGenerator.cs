using UnityEngine;

public class MenuSilhouettesGenerator : MonoBehaviour
{
    public int instantiateDelay;
    public int repeatRate;

    public GameObject spriteRendererContainer;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("InstantiateSilhouette", instantiateDelay, repeatRate);
    }

    void InstantiateSilhouette()
    {
        Instantiate(spriteRendererContainer);
    }
}
