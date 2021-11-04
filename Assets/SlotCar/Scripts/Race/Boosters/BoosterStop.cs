namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class BoosterStop : Booster {
		public override void ApplyEffect(Car car) {
			if (car == null) {
				return;
			}
			if (!isApplied) {
				car.BoosterStop();
				//isApplied = true;
			}
		}
	}
}
