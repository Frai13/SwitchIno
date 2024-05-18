using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using EasInoAPI;
using EasInoAPI.Configuration;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using static EasInoAPI.Configuration.GenericConfiguration;

namespace SwitchIno.SwitchInoCLI
{
    public enum PortConnectMethod { FIX, AUTO }

    public class SwitchInoConfig
    {
        public PortConnectMethod Method { get; set; }
        public GenericConfiguration Configuration { get; set; }

        public SwitchInoConfig()
        {
            Method = PortConnectMethod.FIX;
            Configuration = new GenericConfiguration();
        }

        internal SwitchInoConfig(CommunicationType ComType, IEnumerable<string> args)
        {
            PortConnectMethod Method;
            if (!Enum.TryParse<PortConnectMethod>(args.ElementAt(0), out Method))
            {
                throw new ArgumentException($"Could not parse {args.ElementAt(0)}");
            }
            this.Method = Method;

            if (ComType == CommunicationType.SERIAL)
            {
                this.Configuration = new SerialComConfiguration(args.Skip(1));
            }
        }

        public SwitchInoConfig(GenericConfiguration configuration, PortConnectMethod method)
        {
            Method = method;
            Configuration = configuration;
        }


        public void Serialize()
        {
            string jsonString = JsonConvert.SerializeObject(this);

            File.WriteAllText("CommunicationConfiguration.json", jsonString);
        }

        public SwitchInoConfig Deserialize()
        {
            try
            {
                string jsonString = File.ReadAllText("CommunicationConfiguration.json");
                SwitchInoConfig conf = JsonConvert.DeserializeObject<SwitchInoConfig>(jsonString);

                if (conf == null)
                {
                    return new SwitchInoConfig();
                }

                if (conf.Configuration.ComType == GenericConfiguration.CommunicationType.SERIAL)
                {
                    conf.Configuration = JObject.Parse(jsonString)["Configuration"].ToObject<SerialComConfiguration>();
                    return conf;
                }
                else
                {
                    return conf;
                }
            }
            catch (Exception)
            {
                return new SwitchInoConfig();
            }
        }

        public override string ToString()
        {
            string outStr = "";
            outStr += $"    - Method = {Method}{Environment.NewLine}";
            outStr += $"{Configuration}";

            return outStr;
        }
    }
}
