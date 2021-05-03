using UnityEngine;

namespace Assets.SlotCar.Scripts.Utils {
	[DisallowMultipleComponent]
	public abstract class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance = null;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
					if (instance != null)
					{
						(instance as ASingleton<T>)?.InitOnce();
					}
				}
				return instance;
			}
		}

		public static bool IsSpawned => instance != null;

		protected bool IsInit { get; private set; }

		private Transform _transform;
		public Transform Transform
		{
			get
			{
				if (_transform == null && _transform != null)
				{
					_transform = _transform.transform;
				}

				return _transform;
			}
		}

		protected void Awake()
		{
			if (_transform != null && _transform != this)
			{
				Debug.LogErrorFormat(this, "There's already a singleton of type {0}, destroying copy", GetType());
				Destroy(gameObject);
				return;
			}
			instance = this as T;
			_transform = transform;
			InitOnce();
		}

		private void InitOnce()
		{
			if (IsInit)
			{
				return;
			}
			IsInit = true;
			Init();
		}

		protected virtual void Init()
		{
		}

		protected virtual void Uninit()
		{
		}

		protected virtual void OnDestroy()
		{
			if (IsInit)
			{
				Uninit();
			}
			instance = null;
		}
	}
}