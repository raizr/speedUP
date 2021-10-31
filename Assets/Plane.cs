using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SlotCar.Scripts
{
    public class Plane : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnCollisionEnter(Collision collision)
        {
            Car car = collision.collider.GetComponentInParent<Car>();
            if (car)
            {
                car.Restart();
            }
        }

    }
}
