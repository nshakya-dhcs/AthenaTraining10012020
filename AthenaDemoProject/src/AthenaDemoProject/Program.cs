using Amazon.CDK;

namespace AthenaDemoProject
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new AthenaDemoProjectStack(app, "AthenaDemoProjectStack");

            app.Synth();
        }
    }
}
