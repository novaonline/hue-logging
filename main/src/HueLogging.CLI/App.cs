using HueLogging.CLI.Models;
using HueLogging.Standard.Models.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HueLogging.CLI
{
	public class App
	{
		private readonly IHueAccess hueAccess;

		public App(IHueAccess hueAccess)
		{
			this.hueAccess = hueAccess;
		}

		public async Task Setup()
		{
			Console.Write("This setup requires an API key. Let's get access to the hue bridge button. Are you ready to setup? (y/n): ");
			if (Console.ReadLine().Equals("y"))
			{
				var setup = await hueAccess.Setup();
				using (StreamReader r = new StreamReader("file.json"))
				{
					string json = r.ReadToEnd();
					DynamicConfiguration items = JsonConvert.DeserializeObject<DynamicConfiguration>(json);

					var txt = JsonConvert.SerializeObject(items);
					File.WriteAllText("file.json",txt);
				}
				//JObject
			}
			else
			{
				Console.WriteLine("Try command again when you are ready. Bye.");
			}
		}
	}
}
