using UnityEngine;
using System;

public class WheelDrive : MonoBehaviour
{
    [Tooltip("Maximum steering angle of the wheels")]
	public float maxSteerAngle = 30f;
	public float maxRotateAngle = 30.0f;
	public float currentTorque = 0.0f;
	public float steerAngle;
	private Quaternion rotateSteer;
	private Quaternion rotateReerSteer;
	public float angle;
	private float rotateAngle;

	private Transform[] m_Wheels;
	// Find all the WheelColliders down in the hierarchy.
	void Start() {
		var wheelTransform = transform.GetChild(0).GetChild(1);
		m_Wheels = new Transform[wheelTransform.childCount];
		for (int i = 0; i < wheelTransform.childCount; i++) {
			m_Wheels[i] = wheelTransform.GetChild(i);
		}
	}

	void Update()
	{
		angle = maxSteerAngle * steerAngle;
		var wAngle = currentTorque * maxRotateAngle;
		if (rotateAngle + wAngle >= 360.0f) {
			rotateAngle = 0.0f;
		}
		rotateAngle += wAngle;
		rotateSteer = Quaternion.Euler(rotateAngle, angle, 0.0f);
		rotateReerSteer = Quaternion.Euler(rotateAngle, 0.0f, 0.0f);
		foreach (Transform wheel in m_Wheels) {
			Transform shapeTransform = wheel.transform.GetChild(0);
			if (wheel.localPosition.z > 0) {
				RotateWheel(wheel.name, shapeTransform, rotateSteer);
			}
			else {
				RotateWheel(wheel.name, shapeTransform, rotateReerSteer);
			}
		}
	}

	void RotateWheel(String name, Transform shapeTransform, Quaternion rotateSteer) {
		if (name == "a0l" || name == "a1l") {
			shapeTransform.localRotation = rotateSteer * Quaternion.Euler(0, 180, 0);
		} else {
			shapeTransform.localRotation = rotateSteer;
		}
	}
}
