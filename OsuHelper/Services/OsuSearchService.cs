﻿// -------------------------------------------------------------------------
// Solution: OsuHelper
// Project: OsuHelper
// File: OsuSearchService.cs
// 
// Created by: Tyrrrz
// On: 28.08.2016
// -------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NegativeLayer.Extensions;
using Newtonsoft.Json;
using OsuHelper.Models.API;

namespace OsuHelper.Services
{
    public sealed class OsuSearchService : IDisposable
    {
        private const string Home = "http://osusearch.com/";

        private static string URLEncode(string arg)
        {
            return Uri.EscapeUriString(arg);
        }

        private readonly WebClient _webClient = new WebClient();

        /// <summary>
        /// Searches for beatmaps using given properties and then returns their beatmap IDs
        /// </summary>
        public async Task<IEnumerable<string>> SearchAsync(GameMode mode = GameMode.Standard, string artist = null, string title = null, string diffName = null)
        {
            string url = Home + "query";
            var args = new List<string>();

            // Hidden parameters
            args.Add("statuses=Ranked");

            // Mandatory parameters
            if (mode == GameMode.Standard)
                args.Add("modes=Standard");
            else if (mode == GameMode.CatchTheBeat)
                args.Add("modes=CtB");
            else if (mode == GameMode.Taiko)
                args.Add("modes=Taiko");
            else if (mode == GameMode.Mania)
                args.Add("modes=Mania");

            // Optional parameters
            if (artist.IsNotBlank())
                args.Add($"artist={URLEncode(artist.Trim())}");
            if (title.IsNotBlank())
                args.Add($"title={URLEncode(title.Trim())}");
            if (diffName.IsNotBlank())
                args.Add($"diff_name={URLEncode(diffName.Trim())}");
            url += "?" + args.JoinToString("&");
            string response = await _webClient.DownloadStringTaskAsync(url);

            // High-danger zone (no typechecks)
            dynamic result = JsonConvert.DeserializeObject(response);
            return ((IEnumerable<dynamic>) result.beatmaps).Select(b => b.beatmap_id.ToString()).Cast<string>();
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}