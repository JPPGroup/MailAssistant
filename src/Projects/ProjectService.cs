using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jpp.AddIn.MailAssistant.Projects
{
    public class ProjectService
    {
        public event EventHandler ProjectListChanged;

        private readonly HttpClient client;
        private IList<Project> projects;

        public ProjectService()
        {
            this.client = this.CreateHttpClient();
            LoadProjects();
        }

        private async void LoadProjects()
        {
            this.projects = await GetProjectListAsync();
        }

        public IEnumerable<Project> GetProjects()
        {
            ReloadProjects();
            return this.projects;
        }

        private async void ReloadProjects()
        {
            var list = await GetProjectListAsync();
            if (list.Count != projects.Count)
            {
                projects = list;
                OnProjectListChanged(EventArgs.Empty);
            }
        }

        protected virtual void OnProjectListChanged(EventArgs e)
        {
            var handler = ProjectListChanged;
            handler?.Invoke(this, e);
        }

        private async Task<IList<Project>> GetProjectListAsync()
        {
            using var message = this.GetProjectRequestMessage();
            var response = await this.client.SendAsync(message).ConfigureAwait(false);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<IList<Project>>(result);
            }

            return new List<Project>();
        }

        private HttpClient CreateHttpClient()
        {
            var clientHttp = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
            clientHttp.DefaultRequestHeaders.Accept.Clear();
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return clientHttp;
        }

        private HttpRequestMessage GetProjectRequestMessage()
        {
            var builder = this.GetUriBuilder("api/projects");

            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = builder.Uri,
            };
        }

        private UriBuilder GetUriBuilder(string path)
        {
            return new UriBuilder
            {
                Scheme = "http",
                Host = "192.168.4.77",
                Port = 8080,
                Path = path,
            };
        }
    }
}
