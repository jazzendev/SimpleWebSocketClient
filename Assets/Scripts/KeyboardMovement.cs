using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyboardMovement : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var l = Input.GetAxis("LeftRight") * Time.deltaTime * 120.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        //var u = Input.GetAxis("UpDown") * Time.deltaTime * 8.0f;
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;

        transform.Rotate(0, l, 0);
        transform.Translate(x, 0, z);
    }
}
