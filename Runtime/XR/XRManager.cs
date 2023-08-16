using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Management;

namespace Tactile.XR
{
    /// <summary>
    /// The XRManager allows you to enable and disable XR within the scene. This is useful if you would like to manually
    /// control when to enable XR, such as for implementing a toggle between a desktop and XR mode.
    /// </summary>
    [AddComponentMenu("Tactile/XR/XR Manager")]
    public class XRManager : MonoBehaviour
    {
        [Tooltip("Whether XR is currently enabled.")] [SerializeField]
        private bool isXREnabled = true;

        public UnityEvent<bool> OnXRStatusChanged;
        private bool _wasLoaderInitializedThisSession = false;

        private void Start()
        {
#if UNITY_EDITOR

            var mode = GetXROverrideMode();
            if (mode == XROverrideMode.ForceOff)
                isXREnabled = false;

            if (mode == XROverrideMode.ForceOn)
                isXREnabled = true;

#endif

            SetXREnabled(isXREnabled);
        }

        public bool XREnabled
        {
            get => isXREnabled;
            set => SetXREnabled(value);
        }

        public void SetXREnabled(bool xrEnabled)
        {
            isXREnabled = xrEnabled;

            if (xrEnabled)
            {
                StartXR();
            }
            else
            {
                StopXR();
            }
        }

        public void ToggleXR()
        {
            SetXREnabled(!XREnabled);
        }

        private void StartXR() => StartCoroutine(StartXRCoroutine());

        private IEnumerator InitializeXR()
        {
            if (IsXRInitialized())
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();

            Debug.Log("Initializing XR...");
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
            _wasLoaderInitializedThisSession = true;
        }

        /// <summary>
        /// A coroutine to enable the XR loader. Borrowed from Unity documentation.
        /// </summary>
        /// <seealso href="https://docs.unity3d.com/Packages/com.unity.xr.management@4.2/manual/EndUser.html"/>
        private IEnumerator StartXRCoroutine()
        {
            if (!(_wasLoaderInitializedThisSession && !IsXRInitialized()))
                yield return InitializeXR();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                OnXRStatusChanged.Invoke(true);
            }
        }

        /// <summary>
        /// Stops any XR loader. Borrowed from Unity documentation.
        /// </summary>
        /// <seealso href="https://docs.unity3d.com/Packages/com.unity.xr.management@4.2/manual/EndUser.html"/>
        private void StopXR()
        {
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                Debug.Log("Stopping XR...");

                XRGeneralSettings.Instance.Manager.StopSubsystems();
                OnXRStatusChanged.Invoke(false);
            }
        }

        public static bool IsXRInitialized()
        {
            return XRGeneralSettings.Instance.Manager != null &&
                   XRGeneralSettings.Instance.Manager.isInitializationComplete;
        }

        private void OnDestroy()
        {
            if (IsXRInitialized())
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }

        #region Editor Methods

#if UNITY_EDITOR
        public const string XREnabledKey = "TACTILE_XR_ENABLED";

        public enum XROverrideMode
        {
            Default,
            ForceOff,
            ForceOn
        }

        public static XROverrideMode GetXROverrideMode()
        {
            return (XROverrideMode)UnityEditor.EditorPrefs.GetInt(XREnabledKey, (int)XROverrideMode.Default);
        }

        public static void SetXROverrideMode(XROverrideMode newMode)
        {
            UnityEditor.EditorPrefs.SetInt(XREnabledKey, (int)newMode);
        }
#endif

        #endregion
    }
}