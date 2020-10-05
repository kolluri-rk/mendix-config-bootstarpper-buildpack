using System;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Configuration.ConfigServer;

namespace Pivotal.Mendix.Config.Bootstrapper.Buildpack
{
    public class MendixConfigBootstrapperBuildpack : SupplyBuildpack
    {
        protected override bool Detect(string buildPath)
        {
            return false;
        }

        protected override void Apply(string buildPath, string cachePath, string depsPath, int index)
        {
            Console.WriteLine("================================================================================");
            Console.WriteLine("=============== Mendix Config Bootstrapper Buildpack execution started ================");
            Console.WriteLine("================================================================================");

            var configFileNmae = Environment.GetEnvironmentVariable("MENDIX_CONFIG_FILE") ?? "myconfig.yaml";

            var configFile = Path.Combine(buildPath, configFileNmae);

            if (!File.Exists(configFile))
            {
                Console.WriteLine($"-----> {configFileNmae} not detected, skipping further execution");
                Environment.Exit(0);
            }

            ReplaceTokens(buildPath, configFile);

            Console.WriteLine("================================================================================");
            Console.WriteLine("============== Mendix Config Bootstrapper Buildpack execution completed ===============");
            Console.WriteLine("================================================================================");
        }

        private static void ReplaceTokens(string buildPath, string configFile)
        {
            if (!File.Exists(configFile + ".orig")) // backup original {configFile} as we're gonna transform into it's place
                File.Move(configFile, configFile + ".orig");

            var config = new AppConfig();
            var appName = config.Configuration.GetValue<string>("spring:application:name");
            Console.WriteLine($"=== Writing config server values for app {appName} as config file ===");
            
            PerformTokenReplacements(configFile, config.Configuration);
        }

        private static void PerformTokenReplacements(string configFile, IConfigurationRoot config)
        {
            var configFileContent = File.ReadAllText(configFile);
            foreach (var configEntry in config.AsEnumerable())
            {
                var replaceToken = "_{" + configEntry.Key + "}_";

                if (configFileContent.Contains(replaceToken))
                {
                    Console.WriteLine($"-----> Replacing token `{replaceToken}` in {configFile}");
                    configFileContent = configFileContent.Replace(replaceToken, configEntry.Value);
                }
            }
            File.WriteAllText(configFile, configFileContent);
        }
    }
}
