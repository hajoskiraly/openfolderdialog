using System;

public class Class1
{
	public Class1()
	{
		static void main()
		{
            string sourceDir = @"C:\Forrasmappa";
            string targetDir = @"C:\Celmappa";

            try
            {
                if (Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string DestFile = Path.Combine(targetDir, fileName);
                    file.Copy(file, DestFile, true);
                    Console.WriteLine($"{fileName} sikeresen atmasolva")

                }
                Console.WriteLine("A fajlok sikeresen atmasolva");
            }
            catch (Exception ex)

            {
                Console.WriteLine($"Hiba tortent a fajlok atmasolasa soran: {ex.Message}");)
			}
        }
	}
}
