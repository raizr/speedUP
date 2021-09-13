using UnityEngine;
using Assets.SlotCar.Scripts.Race;

namespace Assets.SlotCar.Scripts.UI
{
    public class KeyboardController : MonoBehaviour
    {
        public PlayerCar car;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                car.MoveTo(MoveLine.ToLeft);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                car.MoveTo(MoveLine.ToRight);
            }
        }
    }
}


