using UnityEngine;
using System.Collections;

public class BackCam : MonoBehaviour {

    private float turnValue = 0;
    void LateUpdate()
    {
        turnValue += Time.deltaTime;
        Vector3 rotationValue = new Vector3(Camera.main.transform.rotation.eulerAngles.x, Camera.main.transform.rotation.eulerAngles.y + turnValue, Camera.main.transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(rotationValue);
    }
}
