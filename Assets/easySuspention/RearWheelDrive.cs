using UnityEngine;
using System.Collections;

public class RearWheelDrive : MonoBehaviour {

	private WheelCollider[] wheels;

	public float maxAngle = 30;
	public float maxTorque = 300;
	public GameObject wheelShape;
    float angle;
    float torque ;
    float speed=0f;
    // here we find all the WheelColliders down in the hierarchy
    public void Start()
	{
		wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < wheels.Length; ++i) 
		{
			var wheel = wheels [i];

			// create wheel shapes only when needed
			if (wheelShape != null)
			{
				var ws = GameObject.Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;

                if(wheel.transform.localPosition.x <0)
                {
                    ws.transform.localScale = new Vector3(ws.transform.localScale.x*-1f, ws.transform.localScale.y, ws.transform.localScale.z);
                }
			}
		}
	}

	// this is a really simple approach to updating wheels
	// here we simulate a rear wheel drive car and assume that the car is perfectly symmetric at local zero
	// this helps us to figure our which wheels are front ones and which are rear
	public void Update()
	{
        if (Input.GetKeyDown(KeyCode.LeftArrow) )
        { 
            angle = maxAngle * -1f;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            angle = maxAngle * 0f;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            angle = maxAngle * 1f; 
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            speed = 1f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            speed = -1f;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            speed = 0f;
        }
        torque = maxTorque * speed;
        foreach (WheelCollider wheel in wheels)
		{
			// a simple car where front wheels steer while rear ones drive
			if (wheel.transform.localPosition.z > 0)
				wheel.steerAngle = angle;

			if (wheel.transform.localPosition.z < 0)
				wheel.motorTorque = torque;

			// update visual wheels if any
			if (wheelShape) 
			{
				Quaternion q;
				Vector3 p;
				wheel.GetWorldPose (out p, out q);

				// assume that the only child of the wheelcollider is the wheel shape
				Transform shapeTransform = wheel.transform.GetChild (0);
				shapeTransform.position = p;
				shapeTransform.rotation = q;
			}

		}
	}
}
