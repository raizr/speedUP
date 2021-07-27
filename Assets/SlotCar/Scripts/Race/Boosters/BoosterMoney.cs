namespace Assets.SlotCar.Scripts.Race.Boosters {
	public class BoosterMoney : Booster {
		public int money = 100;
		public override void ApplyEffect(Car car) {
			RewardSystem.Instance.ApplyMoney(money);
			gameObject.SetActive(false);
		}
	}
}
