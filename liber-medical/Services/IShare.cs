﻿using System;
using System.Threading.Tasks;

namespace libermedical.Services
{
    public interface IShare
    {
        /// <summary>
        /// Simply share a local file on compatible services
        /// </summary>
        /// <param name="localFilePath">path to local file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <param name="view">For iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.</param>
        /// <returns>awaitable Task</returns>
        Task ShareLocalFile(string localFilePath, string title = "", object view = null);

        /// <summary>
        /// Simply share a file from a remote resource on compatible services
        /// </summary>
        /// <param name="fileUri">uri to external file</param>
        /// <param name="fileName">name of the file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <param name="view">For iPad you must pass the view paramater. The view parameter should be the view that triggers the share action, i.e. the share button.</param>
        /// <returns>awaitable bool</returns>
        Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null);

        void ShareFileBytes(byte[] fileData, string fileName, string title = "", object view = null);

    }
}

