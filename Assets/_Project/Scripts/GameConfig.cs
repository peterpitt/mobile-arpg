using UnityEngine;

namespace MobileArpg
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Mobile ARPG/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [SerializeField] private string version = "0.1.0";

        public string Version => version;
    }
}
