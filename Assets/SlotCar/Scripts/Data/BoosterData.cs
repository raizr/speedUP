namespace Assets.SlotCar.Scripts.Data {

	public enum BoosterType {
		Turbo,
		Stop,
		Money
	}

	public struct BoosterData {
		public float distanceFromStart;
		public BoosterType type;
	}
}
