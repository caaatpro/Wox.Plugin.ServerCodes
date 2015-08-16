using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Wox.Plugin.ServerCodes
{

    public class Main : IPlugin
    {
        private List<string> _files = new List<string>();
		private string _icon = Directory.GetCurrentDirectory() + @"\Plugins\Wox.Plugin.ServerCodes\" + @"icon.png";

		public List<Result> Query(Query query)
        {
            var result = new List<Result>();

            if (query.ActionParameters.Count > 0)
            {
				// получаем строку запроса
                var str = query.ActionParameters[0].ToLower();

				// если строка состоит не только из чисел
				int number;
				bool r = Int32.TryParse(str, out number);

	            if (r)
	            {
		            var name = number.ToString();
		            var suportFile = Directory.GetCurrentDirectory() + @"\Plugins\Wox.Plugin.ServerCodes\Codes\index.html";

					// перебираем список файлов
					foreach (var file in _files)
					{

						// ищим файл содержащий в имени данные числа
						if (file.Contains(name))
						{
							string path = @"Plugins\Wox.Plugin.ServerCodes\Codes\" + file + @".txt";
							string text;

							// Получаем содержимое файла
							using (StreamReader sr = new StreamReader(path))
							{
								text = sr.ReadToEnd();
							}

							// выводим результат
							result.Add(new Result()
							{
								Title = file,
								SubTitle = text,
                                IcoPath = _icon,
								Action = c =>
								{
									Process.Start(suportFile);
									return true;
								}
							});
						}
					}
				}

            }

            return result;
        }

        public void Init(PluginInitContext context)
        {
            var dir = new DirectoryInfo(@"Plugins\Wox.Plugin.ServerCodes\Codes");	// папка с файлами
			
			// получаем полный путь к файлу и потом вычищаем ненужное, оставляем только имя файла. 
			_files = dir.GetFiles("*.txt").Select(file => Path.GetFileNameWithoutExtension(file.FullName)).ToList();
        }
    }
}
