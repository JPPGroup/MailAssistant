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
            this.client = CreateHttpClient();
            this.projects = new List<Project>();
        }

        public IEnumerable<Project> GetProjects()
        {
            ReloadProjects();
            return this.projects;
        }

        private async void ReloadProjects()
        {
            var list = await GetProjectListAsync();
            if (list.Count == projects.Count) return;

            projects = list;
            OnProjectListChanged(EventArgs.Empty);
        }

        private void OnProjectListChanged(EventArgs e)
        {
            var handler = ProjectListChanged;
            handler?.Invoke(this, e);
        }

        private async Task<IList<Project>> GetProjectListAsync()
        {
            using var message = GetProjectRequestMessage();
            var response = await this.client.SendAsync(message).ConfigureAwait(false);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return response.IsSuccessStatusCode 
                ? JsonConvert.DeserializeObject<IList<Project>>(result) 
                : new List<Project>();
        }

        private static HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler {UseDefaultCredentials = true};
            var clientHttp = new HttpClient(handler) { Timeout = TimeSpan.FromMinutes(10)};
            clientHttp.DefaultRequestHeaders.Accept.Clear();
            clientHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return clientHttp;
        }

        private static HttpRequestMessage GetProjectRequestMessage()
        {
            var builder = GetUriBuilder("api/projects/cons");

            return new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = builder.Uri,
            };
        }

        private static UriBuilder GetUriBuilder(string path)
        {
            return new UriBuilder
            {
                Scheme = "http",
                Host = "jpp-web-svr",
                Port = 8080,
                Path = path,
            };
        }
    }
}
