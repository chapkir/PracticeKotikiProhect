using System.Net;
using System.Text.RegularExpressions;

namespace PracticeKotikiProject;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            WriteMessage("Enter url address: ", ConsoleColor.Blue,false);
            var url = Console.ReadLine();
            bool checkUrl = IsUrlValid(url);
            if (checkUrl)
            {
                var statusCode = RequestToPage(url);
                DownloadImage(statusCode);
            }
            else
            {
                WriteMessage("The entered url does not exist.", ConsoleColor.Red, true);
            }
        }
    }

    public static int RequestToPage(string url)
    {
        int statusCode;
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowAutoRedirect = false;
            request.Method = WebRequestMethods.Http.Head;
            request.Accept = @"*/*";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            statusCode = (int)response.StatusCode;
            response.Close();
        }
        catch (WebException ex)
        {
            if (ex.Response == null)
                throw;
            statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
        }

        return statusCode;
    }

    public static void DownloadImage(int statusCode)
    {
        using (WebClient client = new WebClient())
        {
            client.DownloadFile(new Uri($"https://http.cat/{statusCode}"),
                $@"C:\Users\User\Desktop\imageStatusCat{statusCode}.jpg");
        }

        WriteMessage("The picture has been successfully uploaded to your desktop!", ConsoleColor.Green, true);
    }

    private static bool IsUrlValid(string url)
    {
        return Regex.IsMatch(url, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
    }

    public static void WriteMessage(string msg, ConsoleColor color, bool newLine)
    {
        Console.ForegroundColor = color;
        if (newLine)
        {
            Console.WriteLine(msg);
        }
        else
        {
            Console.Write(msg);
        }
    }
}