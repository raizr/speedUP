namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class BoosterStop : Booster {
		public override void ApplyEffect(Car car) {
			if (car == null) {
				return;
			}
			if (!isApplied) {
				car.Stop();
				isApplied = true;
				var playerCar = (PlayerCar)car;
				if (playerCar)
				{
					playerCar.CameraShake(5f, 0.5f);
				}
			}
		}
	}
}
