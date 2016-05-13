using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Particle.Setup.Models
{
    /// <summary>
    /// Enumeration for Soft AP scan-ap sec attribute
    /// </summary>
    [Flags]
    public enum SecurityType
    {
        SecurityOpen = 0,                 // Unsecured
        SecurityWepPsk = 1,               // WEP Security with open authentication
        SecurityWepShared = 0x8001,       // WEP Security with shared authentication
        SecurityWpaTkipPsk = 0x00200002,  // WPA Security with TKIP
        SecurityWpaAesPsk = 0x00200004,   // WPA Security with AES
        SecurityWpa2AesPsk = 0x00400004,  // WPA2 Security with AES
        SecurityWpa2TkipPsk = 0x00400002, // WPA2 Security with TKIP
        SecurityWpa2MixedPsk = 0x00400006 // WPA2 Security with AES & TKIP
    }

    /// <summary>
    /// Result class from Soft AP "version" command
    /// </summary>
    public class SoftAPVersion
    {
        [JsonProperty("v")]
        public int VersionNumber { get; set; }
    }

    /// <summary>
    /// Result class from Soft AP "device-id" command
    /// </summary>
    public class SoftAPDeviceId
    {
        private string id;

        /// <remarks>
        /// Device ID's should always be converted to lowercase since the API endpoints are case sensitive
        /// </remarks>
        [JsonProperty("id")]
        public string Id
        {
            get { return id; }
            set { id = value.ToLower(); }
        }
        [JsonProperty("c")]
        public string Claimed { get; set; }
    }

    /// <summary>
    /// Result class from Soft AP "scan-ap" command
    /// </summary>
    public class SoftAPScanAP
    {
        [JsonProperty("ssid")]
        public string SSID { get; set; }
        [JsonProperty("rssi")]
        public int RSSI { get; set; }
        [JsonProperty("sec")]
        public SecurityType Security { get; set; }
        [JsonProperty("ch")]
        public int Channel { get; set; }
        [JsonProperty("mdr")]
        public int MaximumDataRate { get; set; }
    }

    /// <summary>
    /// Collection class from Soft AP "scan-ap" command
    /// </summary>
    public class SoftAPScanAPCollection : List<SoftAPScanAP>
    {
        public SoftAPScanAPCollection()
        {
        }
    }

    /// <summary>
    /// Result class from Soft AP "public-key" command
    /// </summary>
    public class SoftAPPublicKey
    {
        [JsonProperty("b")]
        public string Data { get; set; }
        [JsonProperty("r")]
        public int ResponseCode { get; set; }
    }

    /// <summary>
    /// Result class from Soft AP "set" command
    /// </summary>
    public class SoftAPKeyValue
    {
        [JsonProperty("k")]
        public string Key { get; set; }
        [JsonProperty("v")]
        public string Value { get; set; }
    }

    /// <summary>
    /// Result class from Soft AP "configure-ap" command
    /// </summary>
    public class SoftAPConfigureAP
    {
        [JsonProperty("idx")]
        public int Index { get; set; }
        [JsonProperty("ssid")]
        public string SSID { get; set; }
        [JsonProperty("pwd")]
        public string Password { get; set; }
        [JsonProperty("sec")]
        public SecurityType Security { get; set; }
        [JsonProperty("ch")]
        public int Channel { get; set; }
    }

    /// <summary>
    /// Result class from Soft AP "connect-ap" command
    /// </summary>
    public class SoftAPConnectAP
    {
        [JsonProperty("idx")]
        public int Index { get; set; }
    }
}
