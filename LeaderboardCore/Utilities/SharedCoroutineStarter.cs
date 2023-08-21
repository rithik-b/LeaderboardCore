using UnityEngine;

namespace LeaderboardCore.Utilities
{
	public class SharedCoroutineStarter : MonoBehaviour {
		static MonoBehaviour _instance = null;
		public static MonoBehaviour Instance => _instance ??= new GameObject().AddComponent<SharedCoroutineStarter>();

		void Awake() {
			DontDestroyOnLoad(gameObject);
		}
	}
}
