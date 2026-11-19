# Mobile ARPG

Initial Unity skeleton for a top-down mobile action RPG. Android and iOS player settings are in place. There is no gameplay yet: the Boot scene is a capsule on a plane, and a world-space label shows the version from a `GameConfig` asset.

## Unity version

**Unity 6.3 LTS — 6000.3.28f1** (changeset `8f082d2fe476`).

That version is recorded in `ProjectSettings/ProjectVersion.txt` and pinned in `.github/workflows/unity-ci.yml`. It is the current Unity 6.3 LTS patch (released 3 November 2026). GameCI publishes `unityci/editor` images for this editor, including the `base`, `android`, and `ios` modules the workflow uses.

Open the repository root as a Unity project. The render pipeline is the Universal Render Pipeline (package `com.unity.render-pipelines.universal` **17.3.0**, the URP version that ships with Unity 6.3). Mobile and standalone pipeline assets live under `Assets/_Project/Settings`.

## Layout

```
Assets/_Project/Scripts    runtime assembly MobileArpg.Runtime
Assets/_Project/Data       GameConfig.asset (version string)
Assets/_Project/Scenes     Boot.unity (the only scene in Build Settings)
Assets/_Project/Prefabs    empty
Assets/_Project/Art/Placeholder   primitive materials only
Assets/_Project/Settings   URP pipeline assets
Assets/Tests/EditMode      loads GameConfig and checks the version
Assets/Tests/PlayMode      loads Boot and runs five frames
```

`GameConfig` is a ScriptableObject with a `version` string (`0.1.0`). `BootPlaceholder` copies that string onto the scene label at startup.

Art in this repository is Unity primitives and two flat materials. No Asset Store packages, and no names or content from other games.

## Player settings

| Setting | Value |
| --- | --- |
| Company | `peterpitt` |
| Product | Mobile ARPG |
| Bundle version | `0.1.0` |
| Application id | `com.peterpitt.mobilearpg` (Android, iOS, and standalone) |
| Android scripting | IL2CPP |
| Android CPU | ARM64 only |
| Android minimum API | 31 (Android 12) |
| Android target API | Automatic (highest SDK installed with the editor) |
| iOS scripting | IL2CPP |
| iOS minimum | 15.0 |
| Orientation | Landscape |

Google Play, since 31 August 2026, requires new apps and updates to target Android 16 (API 36). Target SDK is left on Automatic so the build uses the platform SDK that ships inside the Unity 6000.3.28f1 Android module. Before a Play upload, confirm that module's target is API 36 (or set `AndroidTargetSdkVersion` to 36 once that platform is installed).

The CI APK is not signed with a release keystore (`androidUseCustomKeystore` is off). Unity signs it with the debug keystore from the Android SDK. No Android keystore secrets are required.

## Tests

- Edit Mode: `GameConfigTests.GameConfig_LoadsVersionString` loads `Assets/_Project/Data/GameConfig.asset` and expects version `0.1.0`.
- Play Mode: `BootSceneTests.BootScene_RunsSeveralFramesWithoutErrors` loads the Boot scene, waits five frames, and expects the version label to read `0.1.0` with no unexpected error logs.

## Git

`.gitignore` is the current [GitHub Unity gitignore](https://github.com/github/gitignore/blob/main/Unity.gitignore). `Library/`, `Temp/`, `Logs/`, `UserSettings/`, and build outputs are not committed.

`.gitattributes` tracks binary art, audio, video, models, and fonts with Git LFS, and treats Unity YAML (scenes, prefabs, assets) as text. Scene and prefab merges use the `unityyamlmerge` driver. Install UnityYAMLMerge (shipped with the editor) and point Git at it before merging those files, or Git will report that the merge driver is missing. This skeleton does not yet commit any LFS objects.

## Continuous integration

`.github/workflows/unity-ci.yml` runs on pushes to `main` and on pull requests:

1. Edit Mode tests and Play Mode tests (`game-ci/unity-test-runner@v4`), as separate jobs.
2. An Android APK (`game-ci/unity-builder@v4`, `targetPlatform: Android`, `androidExportType: androidPackage`).
3. An iOS Xcode project (`targetPlatform: iOS`) generated on the Ubuntu runner. Signing and an `.ipa` are not part of this workflow.

Test results, the APK, and the Xcode project are uploaded as Actions artifacts. The test jobs pass `githubToken` so GameCI can publish a check run. That needs the job's `checks: write` permission. If the repository's Actions setting is "Read repository contents", the check annotation is skipped and the test-result artifacts are still uploaded.

### Secrets the repository owner must add

Add these under **Settings → Secrets and variables → Actions**. The workflow only references the names below. Do not commit license files, passwords, or serials.

`unity-builder@v4` (the Android APK and iOS Xcode jobs) exits before the editor starts unless `UNITY_LICENSE` or `UNITY_SERIAL` is set. Email and password alone are not enough for those two jobs. The test-runner image accepts any one of the sets below.

Use **one** of these sets. Leave the other names unset.

**Any seat, pre-activated `.ulf` file** (works for every job in this workflow):

| Secret | Value |
| --- | --- |
| `UNITY_LICENSE` | Full text of the Unity license file (`.ulf`) |

**Unity Pro or Plus** (serial activation; also works for every job):

| Secret | Value |
| --- | --- |
| `UNITY_EMAIL` | Unity account email |
| `UNITY_PASSWORD` | Unity account password |
| `UNITY_SERIAL` | Unity serial key |

**Unity Personal, tests only:** `UNITY_EMAIL` and `UNITY_PASSWORD` are enough for the test-runner container, and they are **not** enough for the Android and iOS builds. A Personal seat that must build the APK and the Xcode project needs a `.ulf` in `UNITY_LICENSE` as well.

GameCI recommends a password made of letters and numbers only. Passwords with special characters have failed activation.

How to produce `UNITY_LICENSE`: follow [GameCI activation](https://game.ci/docs/github/activation/). Run the activation workflow once, download the license artifact, and paste the `.ulf` contents into the `UNITY_LICENSE` secret.

`GITHUB_TOKEN` is provided by Actions. It is not a secret you add.

The first Actions run on this branch ([run 44568431118](https://github.com/peterpitt/mobile-arpg/actions/runs/44568431118)) failed in all four jobs before Unity imported the project. Android and iOS stopped with `Missing Unity License File and no Serial was found.` Edit Mode and Play Mode pulled `unityci/editor:ubuntu-6000.3.28f1-linux-il2cpp-3`, then exited with `License activation strategy could not be determined.` Checkout succeeded. No project compile or test result exists yet.

## Local verification

This skeleton was assembled from Unity's URP blank template and hand-authored Boot scene, scripts, and tests. It was **not** opened in the Unity Editor, and the tests and Android build were **not** run.

What blocked a local run:

- The Unity Editor is not installed in this environment.
- No `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`, or `UNITY_SERIAL` is available.
- No Unity `.ulf` license file is on disk.

Unity exits in batch mode without a license, before it imports the project or runs tests. GitHub Actions reached the same gate: the editor image for `6000.3.28f1` downloaded, and every job stopped on the missing license. After the secrets above are added, re-run `.github/workflows/unity-ci.yml` to verify Edit Mode tests, Play Mode tests, the Android APK, and the iOS Xcode project.
