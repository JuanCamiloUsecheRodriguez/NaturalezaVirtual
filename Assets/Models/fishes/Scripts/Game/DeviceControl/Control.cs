using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour
{

    // Use this for initialization
    //private Rigidbody rb;
    public GameObject cameraObj;
    public float limX, limZ;
    public float sizeX, sizeZ;
    Vector3 acc = Vector3.zero;

    public bool move = false;
    void Start()
    {
        Input.gyro.enabled = true;
        //rb = GetComponent<Rigidbody>();
    }

    public float speed = 10.0F;
    private Vector3 velocity = Vector3.zero;
    private float current, last;
    void Update()
    {
        //velocity = Vector3.right * -2f;
        //velocity = Vector3.forward * -2f;

        /*Vector3 dir = Vector3.zero;
        dir.x = Input.acceleration.x;
        dir.z = Input.acceleration.y;
        

        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        dir *= Time.deltaTime;
        transform.Translate(dir * speed);
        Vector3 acc = Vector3.zero;
        
        if (Mathf.Abs(Input.gyro.rotationRateUnbiased.y) < .001f)
        {
            acc.x = Input.acceleration.x;
        }
        if (Mathf.Abs(Input.gyro.rotationRateUnbiased.x) < .001f)
        {
            acc.z = Input.acceleration.y;
        }
        */
        if (move)
        {
            acc.x = -Input.acceleration.x / (speed - 2 * ConfigScript.dificultad);//-Input.gyro.userAcceleration.x;

            acc.z = -Input.acceleration.y / (speed - 2 * ConfigScript.dificultad);//-Input.gyro.userAcceleration.y;
            velocity += acc;
            velocity *= .98f;
            velocity = Vector3.ClampMagnitude(velocity, .25f);// was .45

            transform.Translate(velocity);
            //cameraObj.transform.rotation = Quaternion.Euler( Vector3.right * (90 + transform.position.z / 10) + Vector3.up * (transform.position.x / 10));
            //cameraObj.transform.LookAt(Vector3.right * transform.position.x / 25 + Vector3.forward * transform.position.z / 25);
            if (Mathf.Abs(transform.position.x) + sizeX > limX)
            {
                transform.position = transform.position - Vector3.right * transform.position.x + Vector3.right * (limX - sizeX) * Mathf.Sign(transform.position.x);
            }
            if (Mathf.Abs(transform.position.z) + sizeZ > limZ)
            {
                transform.position = transform.position - Vector3.forward * transform.position.z + Vector3.forward * (limZ - sizeZ) * Mathf.Sign(transform.position.z);
            }
        }
        

    }
}