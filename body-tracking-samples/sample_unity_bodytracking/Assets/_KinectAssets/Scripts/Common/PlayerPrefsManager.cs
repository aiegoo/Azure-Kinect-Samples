using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : Singleton<PlayerPrefsManager>
{

    [Header("License Popup")]
    public readonly string licenseTodayPrefs = "LeftLicensePopup";
    public readonly string licenseLeftDayPrefs = "LeftLicensePopupDay";
    public readonly string licenseExpiredPrefs = "LicenseExpiredPopup";
    public readonly string licensePreviousIDPrefs = "LicensePreviousID";
}
