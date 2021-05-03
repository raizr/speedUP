using UnityEngine;

namespace Assets.SlotCar.Scripts.Utils {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;
		private static readonly object _lock = new object();

		protected virtual void Awake()
		{
			if (ShouldContinueAwake())
			{
				ContinueAwake();
			}
		}

		protected virtual void ContinueAwake()
		{

		}

		protected virtual bool ShouldContinueAwake()
		{
			if (instance != null && instance != this)
			{
				DestroyImmediate(gameObject);
				applicationIsQuitting = false;
				return false;
			}

			instance = this as T;
			return true;
		}

		public static T Instance
		{
			get
			{
				if (applicationIsQuitting && !forceSpawnAfterQuit)
				{
					return null;
				}

				lock (_lock)
				{
					if (instance == null)
					{
						T[] allInstances = FindObjectsOfType<T>();
						int instancesCount = allInstances.Length;

						if (instancesCount > 1)
						{
							Debug.LogError("[Singleton] Something went really wrong - there should never be more than 1 singleton!" + typeof(T));
						}


						instance = null;
						if (instancesCount == 1)
						{
							instance = allInstances[0];
						}
					}

					if (instance == null)
					{
						Debug.LogError("[Singleton] The singleton does not exist, add it to the GlobalScene if you need it! " + typeof(T));
					}

					return instance;
				}
			}
		}

		private static bool applicationIsQuitting = false;
		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed,
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		public virtual void OnDestroy()
		{
			if (!manualDestroy)
			{
				applicationIsQuitting = true;
			}
			manualDestroy = false;
			instance = null;
		}

		private static bool manualDestroy = false;

		protected void ManualDestroy()
		{
			manualDestroy = true;
			instance = null;
			Destroy(gameObject);
		}

		public static bool forceSpawnAfterQuit = false;

		public static bool IsSpawned
		{
			get { return instance != null; }
		}
	}
}