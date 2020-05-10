using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Webhook.Helpers
{
    public class WebhookLibrary
    {
        public static Configuration Configuration { get; set; }

        public static string AccountId { get; set; }

        public static void Init()
        {
            Configuration = new Configuration(new ApiClient("https://demo.docusign.net/restapi"));
            AccountId = GetAccountId();
        }

        public static string GetAccountId()
        {
            var appSettings = System.Configuration.ConfigurationManager.AppSettings;
            string username = appSettings["docusignDeveloperEmail"] ?? "mikelee005@gmail.com";
            string password = appSettings["docusignPassword"] ?? "N#ewport4331";
            string integratorKey = appSettings["docusignIntegratorKey"] ?? "4ed5be53-a1e4-49d4-80bd-563d4635eb9d";

            string authHeader = "{\"Username\":\"" + username + "\", \"Password\":\"" + password + "\", \"IntegratorKey\":\"" + integratorKey + "\"}";

            Configuration.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // the authentication api uses the apiClient (and X-DocuSign-Authentication header) that are set in Configuration object
            AuthenticationApi authApi = new AuthenticationApi(Configuration);
            LoginInformation loginInfo = authApi.Login();

            // find the default account for this user
            foreach (LoginAccount loginAccount in loginInfo.LoginAccounts)
            {
                if (loginAccount.IsDefault == "true")
                {
                    return loginAccount.AccountId;
                }
            }

            return null;
        }

        public static string GetFakeEmailAccess(string email)
        {
            // just create something unique to use with maildrop.cc
            // Read the email at http://maildrop.cc/inbox/<mailbox_name>
            string url = "https://mailinator.com/inbox2.jsp?public_to=";
            string[] parts = email.Split('@');
            if (parts[1] != "mailinator.com")
            {
                return null;
            }
            //return url + parts[0];
            return "https://www.mailinator.com/v3/index.jsp?zone=public&query=webhooktest#/#inboxpane";
        }

        public static string GetFakeEmailAccessQRCode(string address)
        {
            // $url = "http://open.visualead.com/?size=130&type=png&data=";
            string url = "https://chart.googleapis.com/chart?cht=qr&chs=150x150&chl=";
            url += Uri.EscapeDataString(address);
            int size = 150;
            string html = "<img height='" + size + "' width='" + size + "' src='" + url + "' alt='QR Code' style='margin:10px 0 10px;' />";
            return html;
        }

        public static string GetFakeName()
        {
            string[] first_names = new string[] {"Verna", "Walter", "Blanche", "Gilbert", "Cody", "Kathy",
		    "Judith", "Victoria", "Jason", "Meghan", "Flora", "Joseph", "Rafael",
		    "Tamara", "Eddie", "Logan", "Otto", "Jamie", "Mark", "Brian", "Dolores",
		    "Fred", "Oscar", "Jeremy", "Margart", "Jennie", "Raymond", "Pamela",
		    "David", "Colleen", "Marjorie", "Darlene", "Ronald", "Glenda", "Morris",
		    "Myrtis", "Amanda", "Gregory", "Ariana", "Lucinda", "Stella", "James",
		    "Nathaniel", "Maria", "Cynthia", "Amy", "Sylvia", "Dorothy", "Kenneth",
		    "Jackie"};

            string[] last_names = new string[] {"Francisco", "Deal", "Hyde", "Benson", "Williamson", 
		    "Bingham", "Alderman", "Wyman", "McElroy", "Vanmeter", "Wright", "Whitaker", 
		    "Kerr", "Shaver", "Carmona", "Gremillion", "O'Neill", "Markert", "Bell", 
		    "King", "Cooper", "Allard", "Vigil", "Thomas", "Luna", "Williams", 
		    "Fleming", "Byrd", "Chaisson", "McLeod", "Singleton", "Alexander", 
		    "Harrington", "McClain", "Keels", "Jackson", "Milne", "Diaz", "Mayfield", 
		    "Burnham", "Gardner", "Crawford", "Delgado", "Pape", "Bunyard", "Swain", 
		    "Conaway", "Hetrick", "Lynn", "Petersen"};

            Random random = new Random();
            string first = first_names[random.Next(0, first_names.Length - 1)];
            string last = last_names[random.Next(0, first_names.Length - 1)];
            return first + " " + last;
        }

        public static string GetFakeEmail(string name)
        {
            // just create something unique to use with maildrop.cc
            // Read the email at http://maildrop.cc/inbox/<mailbox_name>	
            Random random = new Random();
            string email = random.Next(0, 100) + DateTime.Now.ToString() + name;
            email = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(email));

            Regex rgx = new Regex("[^a-zA-Z0-9]");
            email = rgx.Replace(email, "");

            //return email.Substring(email.Length - 25, 25) + "@mailinator.com";
            return "webhooktest@mailinator.com";
        }
    }
}