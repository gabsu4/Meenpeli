using UnityEngine;
using System.Collections;

public class Enemyanimation : MonoBehaviour {
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    // Use this for initialization
    void Start () {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
       // m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
      //  if (!m_grounded && m_groundSensor.State()) {
      //      m_grounded = true;
      //      m_animator.SetBool("Grounded", m_grounded);
      //  }

        //Check if character just started falling
      //  if(m_grounded && !m_groundSensor.State()) {
      //      m_grounded = false;
      //      m_animator.SetBool("Grounded", m_grounded);
      //  }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        Vector3 currentScale = transform.localScale;

            if (inputX > 0)
                currentScale.x = -Mathf.Abs(currentScale.x); // face right
            else if (inputX < 0)
                currentScale.x = Mathf.Abs(currentScale.x); // face left

            transform.localScale = currentScale;
        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0)) {
            m_animator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }
}
