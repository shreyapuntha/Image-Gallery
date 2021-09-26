﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.IO;

namespace imageGallery
{
    class DataFetcher
    {
        async Task<string> GetDatafromService(string searchstring)
        {
            string readText = null;
            try
            {
                String url = @"
https://imagefetcherapi.azurewebsites.net/api/fetch_images?query=" +
               searchstring + "&max_count=5";
                using (HttpClient c = new HttpClient())
                {
                    readText = await c.GetStringAsync(url);
                }
            }
            catch
            {
                var a = Properties.Resources.sampleData;
                string result = System.Text.Encoding.UTF8.GetString(a);
                readText = result;
            }
            return readText;
        }


        public async Task<List<ImageItem>> GetImageData(string search)
        {
            string data = await GetDatafromService(search);
            return JsonConvert.DeserializeObject<List<ImageItem>>(data);
        }

    }
}
