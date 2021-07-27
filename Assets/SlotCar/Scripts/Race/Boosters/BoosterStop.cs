namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class BoosterStop : Booster {
		public override void ApplyEffect(Car car) {
			if (car == null) {
				return;
			}
			car.Stop();
		}
	}
}
