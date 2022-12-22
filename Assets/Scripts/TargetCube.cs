using UnityEngine;

public class TargetCube : MonoBehaviour
{
    /// <summary>
    /// The plane reference
    /// </summary>
    public FlyingAgent plane;

    /// <summary>
    /// Method that sets position of current object(target) to random point on sphere with radius 10.
    /// </summary>
    private void ResetPos()
    {
        gameObject.transform.position = plane.target.gameObject.transform.position + Random.onUnitSphere * 10;
        //gameObject.transform.position = plane.gameObject.transform.position + plane.gameObject.transform.forward * 10;
    }
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        ResetPos();
    }

    /// <summary>
    /// Event that invokes when another collider enters in trigger zone
    /// </summary>
    /// <param name="other">another collider ref</param>
    private void OnTriggerEnter(Collider other)
    {
        plane.Win();
        ResetPos();
    }
}
