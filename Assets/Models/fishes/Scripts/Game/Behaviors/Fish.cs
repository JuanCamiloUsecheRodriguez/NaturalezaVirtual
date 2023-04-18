using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    // Use this for initialization
    private Vector3 velocity;
    private Vector3 desired;
    private Vector3 steering;
    private float maxSpeed;
    private float angle;
    private Vector3 targetPos;
    private Animator animator;

    public GameObject target;
    public float animatorSpeed = 1f;
    private bool seeking;
        
	void Start () {
        velocity = Vector3.zero;
        desired = Vector3.zero;
        steering = Vector3.zero;
        maxSpeed = .15f;//Random.Range(.1f, .2f);
        angle = 0;
        seeking = true;
        animator = transform.GetChild(0).GetComponent<Animator>();
        //transform.rotation = Quaternion.Euler(Vector3.up * Random.Range(0,360));
        targetPos = transform.localPosition + transform.right * Random.Range(-5f, 5f) + transform.forward * Random.Range(-5f, 5f);// + transform.up * Random.Range(9f, 10f);
        //StartCoroutine("toggleBehavior");
	}

    // Update is called once per frame
    float a = 0;
    float distance = 0;
    float slowRadius =.1f;
    void Update() {
        
        desired = targetPos - transform.position;
        distance = desired.magnitude;

        if (distance < slowRadius)
        {
            changeTarget();
        }
        desired = Vector3.Normalize(desired) * maxSpeed;
        steering = desired - velocity;
        steering = Vector3.ClampMagnitude(steering, .2f * Time.deltaTime); //.1f 
        velocity += steering;
        
        angle = (Mathf.Atan2(targetPos.x - transform.position.x, targetPos.z - transform.position.z) * Mathf.Rad2Deg - transform.localRotation.eulerAngles.y);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime*2f);
        
        transform.localPosition += velocity;

        animator.speed = 1+ animatorSpeed*velocity.magnitude / maxSpeed;

        if (Mathf.Abs(transform.position.x) < 2.5 && Mathf.Abs(transform.position.z) < 2.5)
        {
            //Handheld.Vibrate();
        }
            
	}
    void changeTarget()
    {
        targetPos = transform.localPosition + transform.right * Random.Range(-8f, 8f) + transform.forward * Random.Range(5f, 10f);
        if(Mathf.Abs(transform.localPosition.x) > 36)
            targetPos = Vector3.right * Random.Range(-24f, 24f) + Vector3.forward * Random.Range(-24f, 24f) + Vector3.up * 12f;
        if (Mathf.Abs(transform.localPosition.z) > 36)
            targetPos = Vector3.right * Random.Range(-24f, 24f) + Vector3.forward * Random.Range(-24f, 24f) + Vector3.up * 12f;
        
    }
   
}
