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
        [Tooltip("Whether XR is currently enabled.")]
        [SerializeField] private bool isXREnabled = true;
        
        public UnityEvent OnXREnabled;

        public UnityEvent OnXRDisabled;

        public UnityEvent<bool> OnXRStatusChanged;

        public void SetXREnabled(bool enabled)
        {
            OnXRStatusChanged.Invoke(enabled);
        }
        
        /// <summary>
        /// A coroutine to enable the XR loader. Borrowed from Unity documentation.
        /// </summary>
        /// <seealso href="https://docs.unity3d.com/Packages/com.unity.xr.management@4.2/manual/EndUser.html"/>
        public IEnumerator StartXRCoroutine()
        {
            Debug.Log("Initializing XR...");

            // Necessary for some reason.
            if (IsXRRunning())
                StopXR();

            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                OnXREnabled.Invoke();
            }
        }

        /// <summary>
        /// Stops any XR loader. Borrowed from Unity documentation.
        /// </summary>
        /// <seealso href="https://docs.unity3d.com/Packages/com.unity.xr.management@4.2/manual/EndUser.html"/>
        public void StopXR()
        {
            if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            {
                Debug.Log("Stopping XR...");

                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
                Debug.Log("XR stopped completely.");
                OnXRDisabled.Invoke();
            }
            else
            {
                Debug.LogWarning("Tried to stop XR, but there is no active loader.");
            }
        }

        public static bool IsXRRunning()
        {
            return XRGeneralSettings.Instance.Manager != null &&
                   XRGeneralSettings.Instance.Manager.isInitializationComplete;
        }
    }
}