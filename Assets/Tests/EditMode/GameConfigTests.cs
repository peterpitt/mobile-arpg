using MobileArpg;
using NUnit.Framework;
using UnityEditor;

namespace MobileArpg.Tests.EditMode
{
    public class GameConfigTests
    {
        private const string AssetPath = "Assets/_Project/Data/GameConfig.asset";

        [Test]
        public void GameConfig_LoadsVersionString()
        {
            GameConfig config = AssetDatabase.LoadAssetAtPath<GameConfig>(AssetPath);

            Assert.That(config, Is.Not.Null);
            Assert.That(config.Version, Is.EqualTo("0.1.0"));
        }
    }
}
