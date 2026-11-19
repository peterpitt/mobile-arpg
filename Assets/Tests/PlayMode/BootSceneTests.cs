using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace MobileArpg.Tests.PlayMode
{
    public class BootSceneTests
    {
        [UnityTest]
        public IEnumerator BootScene_RunsSeveralFramesWithoutErrors()
        {
            AsyncOperation load = SceneManager.LoadSceneAsync("Boot", LoadSceneMode.Single);
            Assert.That(load, Is.Not.Null);

            while (!load.isDone)
            {
                yield return null;
            }

            for (int frame = 0; frame < 5; frame++)
            {
                yield return null;
            }

            Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo("Boot"));

            TextMesh label = Object.FindAnyObjectByType<TextMesh>();
            Assert.That(label, Is.Not.Null);
            Assert.That(label.text, Is.EqualTo("0.1.0"));

            LogAssert.NoUnexpectedReceived();
        }
    }
}
