﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PS.FritzBox.API
{
    public class LANEthernetInterfaceClient : FritzTR64Client
    {
        public LANEthernetInterfaceClient(string url, int timeout) : base(url, timeout)
        {

        }

        /// <summary>
        /// Gets the control url
        /// </summary>
        protected override string ControlUrl => "/tr064/upnp/control/lanethernetifcfg";

        /// <summary>
        /// Gets the request namespace
        /// </summary>
        protected override string RequestNameSpace => "urn:dslforum-org:service:LANEthernetInterfaceConfig:1";

        /// <summary>
        /// Methot to set the interface enabled
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnable(bool enable) => this.SetEnableAsync(enable);

        /// <summary>
        /// async Method to set the interface enabled
        /// </summary>
        /// <param name="enable"></param>
        public async void SetEnableAsync(bool enable)
        {
            XDocument document = await this.InvokeAsync("SetEnable", new SoapRequestParameter("NewEnable", enable ? "1" : "0"));
        }

        /// <summary>
        /// Method to get the lan ethernet interface informations
        /// </summary>
        /// <returns></returns>
        public LANEthernetInterfaceInfo GetInfo() => this.GetInfoAsync().Result;
        
        /// <summary>
        /// async Method to get the lan ethernet interface informations
        /// </summary>
        /// <returns></returns>
        public async Task<LANEthernetInterfaceInfo> GetInfoAsync()
        {
            XDocument document = await this.InvokeAsync("GetInfo", null);

            LANEthernetInterfaceInfo info = new LANEthernetInterfaceInfo();
            info.Enable = document.Descendants("NewEnable").First().Value == "1";
            info.MACAddress = document.Descendants("NewMACAddress").First().Value;
            info.MaxBitRate = UInt32.TryParse(document.Descendants("NewMaxBitRate").First().Value, out uint val) ? val : 0;
            info.Status = document.Descendants("NewStatus").First().Value;
            info.DuplexMode = document.Descendants("NewDuplexMode").First().Value;

            return info;
        }

        /// <summary>
        /// Method to get the lan interface statistics
        /// </summary>
        /// <returns>the lan interface statistics</returns>
        public LANStatistics GetStatistics() => this.GetStatisticsAsync().Result;
        
        /// <summary>
        /// async Method to get lan interface statistics
        /// </summary>
        /// <returns>the lan interface statistics</returns>
        public async Task<LANStatistics> GetStatisticsAsync()
        {
            XDocument document = await this.InvokeAsync("GetStatistics", null);
            LANStatistics statistics = new LANStatistics();
            statistics.BytesSent = Convert.ToUInt32(document.Descendants("NewBytesSent").First().Value);
            statistics.BytesReceived = Convert.ToUInt32(document.Descendants("NewBytesReceived").First().Value);
            statistics.PacketsSent = Convert.ToUInt32(document.Descendants("NewPacketsSent").First().Value);
            statistics.PacketsReceived = Convert.ToUInt32(document.Descendants("NewPacketsReceived").First().Value);

            return statistics;
        }
    }
}
