using UnityEngine;

namespace Ex1
{
	public interface IInfiniteScrollSetup
	{
		void OnPostSetupItems();
		void OnUpdateItem(int itemCount, GameObject obj);
	}
}