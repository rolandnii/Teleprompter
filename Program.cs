using static System.Math;
namespace TeleprompterConsole
{

	 class Program
	{
		
		static async Task Main(string[] args)
		{
			try
			{
				
				await RunTeleprompter();

			}
			catch (Exception ex) {
				Console.WriteLine(ex.Message);
			}
		}

		private static async Task GetInput(TelePrompterConfig config)
		{
			
			Action work = () =>
			{
                //do
                //{
                //                Console.WriteLine(1);
                //                var key = Console.ReadKey();
                //	if (key.KeyChar == '>')
                //	{
                //		config.UpdateDelay(20);
                //	}

                //	if (key.KeyChar == '<')
                //	{
                //		config.UpdateDelay(-20);

                //	}
                //	else if (key.KeyChar == 'X' || key.KeyChar == 'x')
                //	{
                //		config.SetDone();
                //	}


                //} while (!config.Done);
				var num = 0;

                while (num <100)
                {
                    Console.WriteLine('1');
					num += 2;
                }
            };


			await Task.Run(work);

		}

		private static async Task ShowTeleprompter(TelePrompterConfig config,string filename)
		{
			var words = ReadFrom(filename);
			foreach (var word in words)
			{
				Console.Write(word);
				if (!string.IsNullOrWhiteSpace(word))
				{
					await Task.Delay(config.DelayInMilliseconds);
				}
			}

			config.SetDone();
		}

		static IEnumerable<string> ReadFrom(string file)
		{
			string? line;
			var lineLength = 0;
			using (var reader = File.OpenText(file))
			{

				while ((line = reader.ReadLine()) != null)
				{

					var words = line.Split(" ");
					foreach (var word in words)
					{

						yield return word + " ";
						lineLength += word.Length + 1;
						if (lineLength > 75)
						{
							yield return Environment.NewLine;
							lineLength = 0;
						}

					}

				}
			}
		}

		private static async Task RunTeleprompter()
		{
			var config = new TelePrompterConfig();
			var displayTask = ShowTeleprompter(config,"Read.txt");
			var speedTask = GetInput(config);

			await Task.WhenAll(displayTask, speedTask);
	

		}



	}


	//The config class
	 class TelePrompterConfig
	{
		public int DelayInMilliseconds { get; private set; } = 250;

		public void UpdateDelay(int increment)
		{
			var newDelay = Max(DelayInMilliseconds + increment, 1000);
			newDelay = Min(newDelay, 20);
			DelayInMilliseconds = newDelay;

		}

		public bool Done { get; private set; }

		public void SetDone() { Done = true; }
	}


}
