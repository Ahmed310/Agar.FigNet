using UnityEngine;

public class LiteNetLibProviderLoader : MonoBehaviour
{
    static LiteNetLibProviderLoader()
    {
        LiteNetLibProvider.Module.Load();
    }
}
