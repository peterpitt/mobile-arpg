using UnityEngine;

namespace MobileArpg
{
    public sealed class BootPlaceholder : MonoBehaviour
    {
        [SerializeField] private GameConfig config;
        [SerializeField] private TextMesh versionLabel;

        private void Awake()
        {
            if (config == null || versionLabel == null)
            {
                return;
            }

            versionLabel.text = config.Version;
        }
    }
}
