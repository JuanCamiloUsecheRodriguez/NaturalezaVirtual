using UnityEngine;
using System.Collections;

public class Fish2 : MonoBehaviour
{

    // Use this for initialization
    private Vector3 velocity;
    private Vector3 desired;
    private Vector3 steering;
    private float maxSpeed;
    private float angle;
    private Vector3 targetPos;
    private Animator animator;

    private float gravity = -.05f;

    public GameObject target;
    public float animatorSpeed = 6f;
    private bool seeking, catchable = true;
    public bool fast = false, inNet = false, startWander = true;

    public float CIRCLE_DISTANCE = 10f;
    public float CIRCLE_RADIUS = 5f;
    public float ANGLE_CHANGE = 30f;

    public int score;

    public int difficulty = 0; 

    private delegate void BehaviorDelegate();
    private BehaviorDelegate behaviorDelegate;

    public string normalBehavior;

    private float wanderAngle = 0;

    FishingCheck fc;
    PressureScript ps;
    void Start()
    {
        //velocity = Vector3.zero;
        
        desired = Vector3.zero;
        steering = Vector3.zero;
        if (fast)
        {
            maxSpeed = .2f;
            transform.position = transform.position - Vector3.up * (transform.position.y);
        } else
        {
            //maxSpeed = .06f + .03f * difficulty;
            //maxSpeed = .05f + .025f * ConfigScript.dificultad;
            maxSpeed = .005f;
        }
        velocity = transform.right * Random.Range(-maxSpeed, maxSpeed) + transform.forward * Random.Range(-maxSpeed, maxSpeed);
        angle = 0;
        seeking = true;
        animator = transform.GetChild(0).GetComponent<Animator>();
        if (startWander)
        {
            behaviorDelegate = wanderBehavior;
            normalBehavior = Behaviors.WANDER;
        } else
        {
            behaviorDelegate = seekBehavior;
            normalBehavior = Behaviors.SEEK;
        }
        
        //behaviorDelegate = caughtBehavior;
        //behaviorDelegate = wanderBehavior;
        //behaviorDelegate = caughtBehavior;
        if (GameObject.Find("FishingCheck") != null)
            fc = GameObject.Find("FishingCheck").GetComponent<FishingCheck>();
        if (GameObject.Find("PressureSensor") != null)
            ps = GameObject.Find("PressureSensor").GetComponent<PressureScript>();

        targetPos = transform.localPosition + transform.right * Random.Range(-5f, 5f) + transform.forward * Random.Range(-5f, 5f);
        //StartCoroutine("toggleBehavior");
    }

    // Update is called once per frame
    float a = 0;
    float distance = 0;
    float slowRadius = 1f;

    int b = 0;

    void Update()
    {
        behaviorDelegate();

        /*if (Mathf.Abs(transform.position.x) < 2.5 && Mathf.Abs(transform.position.z) < 2.5)
        {
            
            

        }*/

    }
    void OnTriggerEnter(Collider c)
    {
        if (ps != null && ps.inWater && catchable)
        {
            Handheld.Vibrate();
            fc.notify(this.gameObject);
            inNet = true;

            fc.fishes.Add(this);
        } else if (ps != null && !ps.inWater && catchable)
        {
            inNet = true;
            fc.fishes.Add(this);
            
        }
    }
    void OnTriggerExit()
    {
        inNet = false;
        //fc.fishes.Remove(this);
    }

    public void changeBehavior(string behavior)
    {
        velocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        //maxSpeed = .12f;

        switch (behavior)
        {
            case Behaviors.WANDER:
                behaviorDelegate = wanderBehavior;
                break;
            case Behaviors.SEEK:
                behaviorDelegate = seekBehavior;
                break;
            case Behaviors.JUMP:
                behaviorDelegate = caughtBehavior;
                transform.position = Vector3.zero + Vector3.up * (12);
                break;
        }
        
    }
    private void wanderBehavior()
    {
        if (b % 5 == 0)
        {
            steering = wander();
            // steering/mass            
        }
        b++;
        steering = Vector3.ClampMagnitude(steering, maxSpeed);
        steering = steering / 2f;
        velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);

        if (Mathf.Abs(transform.localPosition.x) > 50)
        {
            steering = Vector3.zero;
            velocity *= -1f;
        }
        if (Mathf.Abs(transform.localPosition.z) > 35)
        {
            steering = Vector3.zero;
            velocity *= -1f;
        }

        transform.localPosition += velocity;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.LookRotation(velocity), Time.deltaTime * 8f);

        animator.speed = 1 + animatorSpeed * velocity.magnitude / (maxSpeed*2);
    }
    public void speedUpFunction()
    {
        this.maxSpeed = .3f;
        catchable = false;
        StartCoroutine(speedUp());
    }
    public IEnumerator speedUp()
    {
        yield return new WaitForSeconds(.1f);// 2seg
        if(this.maxSpeed > .05f)
        {
            this.maxSpeed -= .0125f;
            catchable = false;
            StartCoroutine(speedUp());
        } else
        {
            this.maxSpeed = .05f;
            catchable = true;
            StopCoroutine(speedUp());
        }        
    }
    private void caughtBehavior()
    {
        
        if(score > 0)
        {
            velocity += Vector3.up * gravity;
            velocity *= .97f;
            transform.position += velocity;
            if (transform.position.y < 12)
            {
                transform.position = transform.position - Vector3.up * (transform.position.y - 12);
                StartCoroutine("jump");
            }
            if (Mathf.Sqrt(transform.position.x * transform.position.x + (transform.position.z - 2f) * (transform.position.z - 2f)) >= 2f)
            {
                velocity.x *= -.75f;
                velocity.z *= -.75f;
            }
            animator.speed = 5;
            transform.rotation = Quaternion.Euler(Vector3.forward * 90f);
        }
        else
        {
            animator.Play("Attack");
        }
               
    }
    private void seekBehavior()
    {
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

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * 2f);

        transform.localPosition += velocity;

        animator.speed = .5f + animatorSpeed * velocity.magnitude / (maxSpeed * 2);

        Debug.DrawLine(transform.position, targetPos);
    }
    void changeTarget()
    {
        targetPos = transform.localPosition + transform.right * Random.Range(-8f, 8f) + transform.forward * Random.Range(5f, 10f);
        if (Mathf.Abs(transform.localPosition.x) > 36)
            targetPos = Vector3.right * Random.Range(-24f, 24f) + Vector3.forward * Random.Range(-24f, 24f) + Vector3.up * 12f;
        if (Mathf.Abs(transform.localPosition.z) > 36)
            targetPos = Vector3.right * Random.Range(-24f, 24f) + Vector3.forward * Random.Range(-24f, 24f) + Vector3.up * 12f;

    }
    private IEnumerator jump()
    {
        velocity = Vector3.up * Random.Range(.3f, .6f) + Vector3.right * Random.Range(-.1f, .1f) + Vector3.forward * Random.Range(-.1f, .1f);
        yield return new WaitForSeconds(Random.Range(.5f,1f));
        //StartCoroutine("jump");
    }
    private Vector3 wander(){
               
        Vector3 circleCenter = velocity;
        circleCenter.Normalize();
        circleCenter.Scale(CIRCLE_DISTANCE*Vector3.one);
        
        Vector3 displacement = new Vector3(1, 0, 1);
        displacement.Scale(CIRCLE_RADIUS*Vector3.one);
        
        setAngle( ref displacement, wanderAngle);
        
        wanderAngle += Random.Range(0.0f, 1.0f) * ANGLE_CHANGE - ANGLE_CHANGE* .5f;
        
        Vector3 wanderForce = circleCenter+displacement;
        //Debug.DrawLine(Vector3.zero, displacement);
        return wanderForce;
    }
    
    public void setAngle(ref Vector3 vector, float value) {
        var len = vector.magnitude;
        vector.x = Mathf.Cos(value) * len;
        vector.z = Mathf.Sin(value) * len;
        vector.y = 0;//(Mathf.Tan(value)) * len;
    }
   
}
