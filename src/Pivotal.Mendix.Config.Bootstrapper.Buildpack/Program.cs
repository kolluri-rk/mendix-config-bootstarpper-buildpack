using System;

namespace Pivotal.Mendix.Config.Bootstrapper.Buildpack
{
    public class Program
    {
        static int Main(string[] args)
        {
            return new MendixConfigBootstrapperBuildpack().Run(args);
        }
    }
}