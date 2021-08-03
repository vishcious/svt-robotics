using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace svtrobotics {
    public class RobotStatusService {

        public HttpClient HttpClient;
        public RobotStatusService(HttpClient httpClient){
            this.HttpClient = httpClient;
            this.HttpClient.BaseAddress = new Uri("https://svtrobotics.free.beeceptor.com");
            this.HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<RobotStatusResponse[]> GetRobotStatuses() {
            return await this.HttpClient.GetFromJsonAsync<RobotStatusResponse[]>("robots");
        }

    }
}