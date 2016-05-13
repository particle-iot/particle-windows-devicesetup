using Particle.SDK;

namespace Particle.Setup.Models
{
    /// <summary>
    /// Enumeration for Particle Device Setup completion results
    /// </summary>
    public enum SoftAPSetupResult
    {
        NotStarted,
        Started,
        Success,
        SuccessUnknown,
        SuccessDeviceOffline,
        FailureClaiming,
        FailureConfigure,
        FailureCannotDisconnectFromDevice,
        FailureLostConnectionToDevice
    };

    /// <summary>
    /// A simple class for storing information throughout the Particle Device Setup
    /// </summary>
    public class SoftAPData
    {
        public string ClaimCode { get; set; }
        public SoftAPVersion Version { get; set; }
        public SoftAPDeviceId DeviceId{ get; set; }
        public SoftAPPublicKey PublicKey { get; set; }
        public SoftAPScanAP ScanAP { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// A simple class for storing Particle Device Setup results
    /// </summary>
    public class SoftAPResult
    {
        public SoftAPSetupResult Result { get; set; }
        public ParticleDevice ParticleDevice { get; set; }
    }
}
