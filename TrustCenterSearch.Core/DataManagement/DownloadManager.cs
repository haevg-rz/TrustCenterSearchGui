﻿using System;
using System.IO;
using System.Net;
using System.Text;

namespace TrustCenterSearch.Core.DataManagement
{
    public class DownloadManager
    {

        private string TrustCenetrUrl { get; set; } = "https://trustcenter-data.itsg.de/";
        internal void DownloadTrustCenter(string trustCenterName, string trustCenterUrl, string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            var client = new WebClient();

            var data = client.DownloadData(trustCenterUrl);
            var str = Encoding.UTF8.GetString(data);
            File.WriteAllText(GetFilePath(trustCenterName, filePath), str);
        }

        internal string GetFilePath(string name, string filePath)
        {
            return filePath + name + @".txt";
        }

        internal bool IsUrlExisting(string url)
        {
            if (url == string.Empty)
                return false;

            if (!url.Contains(this.TrustCenetrUrl))
                return false;

            try
            {
                var urlCheck = new Uri(url);
                var request = WebRequest.Create(urlCheck);
                request.GetResponse();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}