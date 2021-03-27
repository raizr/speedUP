using DG.Tweening;
using UnityEngine;

namespace Assets {
	public class Rotate : MonoBehaviour
	{

		// Start is called before the first frame update
		void Start()
		{
			transform.DOLocalRotate(new Vector3(0.0f, 180, 0.0f), 1.0f)
				.SetLoops(-1, LoopType.Incremental)
				.SetEase(Ease.Linear)
				.SetRelative(true);
		}

		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
