using System;
using System.Collections.Generic;
using Boxer.Data;
using Newtonsoft.Json;

namespace SpriteUtility.Data.Export
{
    [Serializable]
    public class DocumentExport
    {
        [JsonProperty("project_name")]
        public string ProjectName { get; set; }

        [JsonProperty("folders")]
        public List<FolderExport> Folders { get; set; }

        public DocumentExport(Document document)
        {
            ProjectName = document.Name;
            Folders = new List<FolderExport>();
            foreach (Folder folder in document.Children)
            {
                Folders.Add(new FolderExport(folder));
            }
        }
    }
}