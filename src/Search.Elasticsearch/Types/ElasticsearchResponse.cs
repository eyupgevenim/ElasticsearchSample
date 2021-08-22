namespace Search.Elastic.Types
{
    using Nest;
    using Newtonsoft.Json;
    using System;

    public class ElasticsearchResponse
    {
        private readonly IResponse _response;
        public ElasticsearchResponse(IResponse response)
        {
            _response = response;
        }

        public bool IsValid => _response.IsValid;
        public Exception Exception => _response.OriginalException;
        public string ServerError
        {
            get
            {
                if (_response.ServerError != null)
                    return JsonConvert.SerializeObject(_response.ServerError);

                return string.Empty;
            }
        }

        public void EnsureSuccess()
        {
            if (!IsValid)
            {
                throw Exception;
            }
        }
    }
}
