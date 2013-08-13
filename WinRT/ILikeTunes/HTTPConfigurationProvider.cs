using System.Linq;
using UpdateControls.Fields;
using UpdateControls.Correspondence.BinaryHTTPClient;

namespace ILikeTunes
{
    public class HTTPConfigurationProvider : IHTTPConfigurationProvider
    {
        private Independent<Individual> _individual = new Independent<Individual>();

        public Individual Individual
        {
            get { return _individual; }
            set { _individual.Value = value; }
        }

        public HTTPConfiguration Configuration
        {
            get
            {
                string address = "http://169.254.80.80:8080/correspondence_server_web/bin";
                string apiKey = "D1920D309A4E43EB85BD14833AFEF7E8";
				int timeoutSeconds = 30;
                return new HTTPConfiguration(address, "ILikeTunes", apiKey, timeoutSeconds);
            }
        }

        public bool IsToastEnabled
        {
            get { return false; }
        }
    }
}
