using Flow.Client.API.Interfaces;
using Flow.Client.API.Models;
using Newtonsoft.Json;
using RestSharp;

namespace Flow.Client.API
{
    public class FlowClient : IFlowClient

    {
        public CreateGroupResponse CreateGroup(CreateGroupParams createGroupParams)
        {
            var client = new RestClient(
                "https://prod-35.westeurope.logic.azure.com:443/workflows/b95ddae3be924054819a5087cc04b7c2/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=BCaR1QWg7kfxeiJLY8FvgnbKyprfCqd7XfK1phJ3_y8");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Host", "prod-35.westeurope.logic.azure.com:443");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonConvert.SerializeObject(createGroupParams),
                ParameterType.RequestBody);
            var response = client.Execute<CreateGroupResponse>(request);
            return response.Data;
        }

        public GetUsersResponse GetUsers()
        {
            var client = new RestClient(
                "https://prod-85.westeurope.logic.azure.com:443/workflows/a9e656eaea0140209e3a13a111af6b98/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=WGe1lSoIXb2xMOTHVTJy5SsFH6Fd0A2Vckug4a6Eh1g");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "prod-35.westeurope.logic.azure.com:443");

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<GetUsersResponse>(response.Content);
        }
    }
}