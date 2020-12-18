using HeadHunter.Models.Entity;
using HeadHunter.Models.UnifiedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HtmlAgilityPack;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace HeadHunter.Controllers
{
    public class EmployeeController : Controller
    {
        string githubRepositories = "";
        string githubProjects = "";
        string githubStars = "";
        string githubFollowers = "";
        string githubFollowing = "";
        string strgithubName = "";
        string github_image = "";
        const string githubUrl = "https://github.com/";
        List<string> githubLanguageList = new List<string>();
        List<string> githubLanguageListKsiz = new List<string>();
        List<string> githubLanguageListCount = new List<string>();
        List<Int32> githubLanguageListCountInt = new List<Int32>();

        ////////
        string stackName = "";
        string stackReputation = "";
        string stackAnswers = "";
        string stackQuestions = "";
        string stackReached = "";
        string stackMostTag = "";
        List<string> stackAnswersQuestionsReached = new List<string>();

        public void getInfosStack(string link_stack)
        {

            if (link_stack == null)
            {
                string mail = Session["UserMail"].ToString();
                Account user = db.Account.FirstOrDefault(m => m.AccountMail == mail);
                Profil profileStackOverFlow =  db.Profil.FirstOrDefault(m => m.AccountId == user.AccountId && m.SourceId == 2);
                link_stack = "https://stackoverflow.com/users/" + profileStackOverFlow.ProfileUsername;
            }
            else
            {
                link_stack = link_stack;
            }

            Uri url = new Uri(link_stack);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string html = client.DownloadString(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var stackNameHtml = @"//div[@class='grid--cell fw-bold']";
            var stackReputationHtml = @"//div[@class='grid--cell fs-title fc-dark']";
            var stackAnswersHtml = @"//div[@class='grid--cell fs-body3 fc-dark fw-bold']";
            var stackTagHtml = @"//a[@class='post-tag']";



            StringBuilder nameSt = new StringBuilder();
            StringBuilder reputationSt = new StringBuilder();
            StringBuilder answersSt = new StringBuilder();
            StringBuilder mostTagSt = new StringBuilder();

            var secilenstackNameHtml = document.DocumentNode.SelectNodes(stackNameHtml);
            var secilenstackReputationHtml = document.DocumentNode.SelectNodes(stackReputationHtml);
            var secilenstackAnswersHtml = document.DocumentNode.SelectNodes(stackAnswersHtml);
            var secilenstackTagHtml = document.DocumentNode.SelectNodes(stackTagHtml);



            foreach (var items in secilenstackNameHtml)
            {
                nameSt.AppendLine(items.InnerText);
                stackName = nameSt.ToString();

            }
            foreach (var items in secilenstackReputationHtml)
            {
                reputationSt.AppendLine(items.InnerText);
                stackReputation = reputationSt.ToString().Replace(",", string.Empty);

            }
            foreach (var items in secilenstackAnswersHtml)
            {

                stackAnswersQuestionsReached.Add(items.InnerText.Replace(",", string.Empty).Replace("~", string.Empty).Replace(".", string.Empty));

            }
            foreach (var items in secilenstackTagHtml)
            {

                mostTagSt.AppendLine(items.InnerText);
                stackTagHtml = mostTagSt.ToString();
                break;
            }

            stackAnswers = stackAnswersQuestionsReached[0];
            stackQuestions = stackAnswersQuestionsReached[1];

            if (stackAnswersQuestionsReached[2].Last() == 'm')
            {
                stackReached = stackAnswersQuestionsReached[2].Replace("m", "000");
            }
            else if (stackAnswersQuestionsReached[2].Last() == 'k')
            {
                stackReached = stackAnswersQuestionsReached[2].Replace("m", "000");
            }
            else
            {
                stackReached = stackAnswersQuestionsReached[2];
            }

        }



        private GoogleVisualizationDataTable ConstructDataTableStack(MarketSales[] data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            var markets = data.Select(x => x.Market).Distinct().OrderBy(x => x);

            // Get distinct years from the data
            var years = data.Select(x => x.Year).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("", "string");
            foreach (var year in years)
            {
                dataTable.AddColumn(year.ToString(), "number");
            }

            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var market in markets)
            {
                var values = new List<object>(new[] { market });
                foreach (var year in years)
                {
                    var totalSales = data
                        .Where(x => x.Market == market)
                        .Select(x => x.Count)
                        .SingleOrDefault();
                    values.Add(totalSales);
                }
                dataTable.AddRow(values);
            }


            return dataTable;
        }



        private MarketSales[] GetMarketSalesFromDatabaseStack(string Text)
        {
            getInfosStack(Text);


            // Let's pretend this came from a database, via EF, Dapper, or something like that...
            return new MarketSales[]
            {
                new MarketSales() { Market = stackMostTag.ToString(), Year = 2019, Count = Convert.ToInt32("0") },
                new MarketSales() { Market = "stackReputation", Year = 2019, Count = Convert.ToInt32(stackReputation) },
                new MarketSales() { Market = "stackAnswers", Year = 2019, Count = Convert.ToInt32(stackAnswers) },
                new MarketSales() { Market = "stackQuestions", Year = 2019, Count = Convert.ToInt32(stackQuestions) },
                new MarketSales() { Market = "stackReached", Year = 2019, Count = Convert.ToInt32(stackReached) },


            };
        }
        public void getInfos(string username)
        {
            string link_github;
            if (username == null)
            {
                string mail = Session["UserMail"].ToString();
                Account user = db.Account.FirstOrDefault(m => m.AccountMail == mail);
                Profil githubProfile = db.Profil.FirstOrDefault(m => m.AccountId == user.AccountId && m.SourceId == 1);
                link_github = githubUrl + githubProfile.ProfileUsername ;
            }
            else
            {
                link_github = githubUrl + username;
            }


            Uri url = new Uri(link_github);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string html = client.DownloadString(url);

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var githubilkbar = @"//span[@class='Counter hide-lg hide-md hide-sm']";
            var githubName = @"//span[@class='p-name vcard-fullname d-block overflow-hidden']";


            List<string> githubilkbarList = new List<string>();


            StringBuilder firstInfos = new StringBuilder();
            StringBuilder languages = new StringBuilder();


            var githubilkbarHtml = document.DocumentNode.SelectNodes(githubilkbar);
            var githubNameHtml = document.DocumentNode.SelectNodes(githubName);



            foreach (var items in githubilkbarHtml)
            {
                if (items.InnerText.Trim().Last() == 'k')
                {

                    githubilkbarList.Add(items.InnerText.Trim().Remove(items.InnerText.Trim().Length - 1).Replace(".", string.Empty) + "000");
                }
                else
                {
                    githubilkbarList.Add(items.InnerText.Trim());
                }

            }
            foreach (var items in githubNameHtml)
            {
                strgithubName = items.InnerText.Trim();
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////sd
            string mail2 = Session["UserMail"].ToString();
            Account user2 = db.Account.FirstOrDefault(m => m.AccountMail == mail2);
            Profil githubProfile2 = db.Profil.FirstOrDefault(m => m.AccountId == user2.AccountId && m.SourceId == 1);
            var scannedProfile = db.Profil.FirstOrDefault(m => m.ProfileUsername == githubProfile2.ProfileUsername);

            githubRepositories = githubilkbarList[0];//repo count
            ProfileScore repoCount = new ProfileScore();
            repoCount.ProfileId = scannedProfile.ProfileId;
            repoCount.ProfileScoreFlag = true;
            repoCount.SourceDataTypeId = 8;
            repoCount.ScoreValue = Convert.ToInt32(githubRepositories);
            db.ProfileScore.Add(repoCount);//repo count


            githubProjects = githubilkbarList[1];

            githubStars = githubilkbarList[2];//Star Count
            ProfileScore starCount = new ProfileScore();
            starCount.ProfileId = scannedProfile.ProfileId;
            starCount.ProfileScoreFlag = true;
            starCount.SourceDataTypeId = 9;
            starCount.ScoreValue = Convert.ToInt32(githubStars);
            db.ProfileScore.Add(starCount);//star count

            githubFollowers = githubilkbarList[3];
            ProfileScore FollowersCount = new ProfileScore();//Followers Count
            FollowersCount.ProfileId = scannedProfile.ProfileId;
            FollowersCount.ProfileScoreFlag = true;
            FollowersCount.SourceDataTypeId = 12;
            FollowersCount.ScoreValue = Convert.ToInt32(githubFollowers);
            db.ProfileScore.Add(FollowersCount);//Followers Count



            githubFollowing = githubilkbarList[4];
            ProfileScore FollowingCount = new ProfileScore();//Following Count
            FollowingCount.ProfileId = scannedProfile.ProfileId;
            FollowingCount.ProfileScoreFlag = true;
            FollowingCount.SourceDataTypeId = 12;
            FollowingCount.ScoreValue = Convert.ToInt32(githubFollowing);
            db.ProfileScore.Add(FollowingCount);//Following Count

            db.SaveChanges();//bu kısım sadece kayit ol kısmında calismali yoksa patlar
            //MPR:.Trim()
            string languagesLink = link_github.Trim() + "?tab=repositories";

            Uri url_repo = new Uri(languagesLink);

            WebClient client_repo = new WebClient();
            client_repo.Encoding = Encoding.UTF8;

            string html_repo = client_repo.DownloadString(url_repo);

            HtmlAgilityPack.HtmlDocument document_repo = new HtmlAgilityPack.HtmlDocument();
            document_repo.LoadHtml(html_repo);

            var githubLanguages = @"//span[@class='text-normal']";
            List<string> githubLanguageListTemp = new List<string>();

            var githubLanguageHtml = document_repo.DocumentNode.SelectNodes(githubLanguages);

            foreach (var items in githubLanguageHtml)
            {
                githubLanguageListTemp.Add(items.InnerText.Trim());
            }

            for (int i = 6; i < githubLanguageListTemp.Count; i++)
            {
                githubLanguageList.Add(githubLanguageListTemp[i]);
            }


            foreach (var i in githubLanguageList)
            {
                string dilUrl = languagesLink + "&q=&type=&language=" + i;

                Uri url_repo_count = new Uri(dilUrl);

                WebClient client_repo_count = new WebClient();
                client_repo_count.Encoding = Encoding.UTF8;

                string html_repo_count = client_repo_count.DownloadString(url_repo_count);

                HtmlAgilityPack.HtmlDocument document_repo_count = new HtmlAgilityPack.HtmlDocument();
                document_repo_count.LoadHtml(html_repo_count);


                var githubcount = @"//div[@class='user-repo-search-results-summary TableObject-item TableObject-item--primary v-align-top']//strong";

                var githubcountHtml = document_repo_count.DocumentNode.SelectNodes(githubcount);


                for (int k = 0; k < githubcountHtml.Count; k = k + 2)
                {
                    githubLanguageListCount.Add(githubcountHtml[k].InnerText.Trim());
                }

                foreach (var item in githubLanguageListCount)
                {
                    githubLanguageListCountInt.Add(Convert.ToInt32(item));
                }




            }


        }

        private GoogleVisualizationDataTable ConstructDataTable(MarketSales[] data)
        {
            var dataTable = new GoogleVisualizationDataTable();

            // Get distinct markets from the data
            var markets = data.Select(x => x.Market).Distinct().OrderBy(x => x);

            // Get distinct years from the data
            var years = data.Select(x => x.Year).Distinct().OrderBy(x => x);

            // Specify the columns for the DataTable.
            // In this example, it is Market and then a column for each year.
            dataTable.AddColumn("", "string");
            foreach (var year in years)
            {
                dataTable.AddColumn(year.ToString(), "number");
            }

            // Specify the rows for the DataTable.
            // Each Market will be its own row, containing the total sales for each year.
            foreach (var market in markets)
            {
                var values = new List<object>(new[] { market });
                foreach (var year in years)
                {
                    var totalSales = data
                        .Where(x => x.Market == market)
                        .Select(x => x.Count)
                        .SingleOrDefault();
                    values.Add(totalSales);
                }
                dataTable.AddRow(values);
            }


            return dataTable;
        }

        private MarketSales[] GetMarketSalesFromDatabase(string Text)
        {
            getInfos(Text);


            // Let's pretend this came from a database, via EF, Dapper, or something like that...
            return new MarketSales[]
            {
                new MarketSales() { Market = "Repositories", Year = 2019, Count = Convert.ToInt32(githubRepositories) },
                new MarketSales() { Market = "Projects", Year = 2019, Count = Convert.ToInt32(githubProjects) },
                new MarketSales() { Market = "Stars", Year = 2019, Count = Convert.ToInt32(githubStars) },
                new MarketSales() { Market = "Followers", Year = 2019, Count = Convert.ToInt32(githubFollowers) },
                new MarketSales() { Market = "Following", Year = 2019, Count = Convert.ToInt32(githubFollowing) },


            };
        }

        public ActionResult ProfileGraph(string Text)
        {

            getInfos(Text);


            var data = GetMarketSalesFromDatabase(Text);


            ViewBag.Diller = githubLanguageList;
            ViewBag.DilSayisi = githubLanguageListCountInt;




            return View(new SalesChartModel()
            {
                Title = "Github Profile Infos Summary",
                Subtitle = Text,
                DataTable = ConstructDataTable(data)

            });

        }


        /// 
        /// //////////////////////////////////////////////////////////////////////////////////
        /// 
        // GET: Employee
        HeadHunterEntities db = new HeadHunterEntities();

        [AuthorizeRoleAttribute(RoleNames.Employee, RoleNames.Employer)]
        public ActionResult Profile()
        {
            ProfileModel profileModel = new ProfileModel();
            string _mail = Session["UserMail"].ToString();
            Account user = db.Account.FirstOrDefault(m => m.AccountMail == _mail);
            var stackOverProfile = db.Profil.FirstOrDefault(m => m.AccountId == user.AccountId && m.SourceId == 2);
            var githubProfile = db.Profil.FirstOrDefault(m => m.AccountId == user.AccountId && m.SourceId == 1);
            

            var profileMeta = db.SelectProfileMeta(githubProfile.ProfileId).ToList();
            var profileScore = db.SelectProfileScore(githubProfile.ProfileId).ToList();
            

            profileModel.mail = user.AccountMail;
            profileModel.githubUsername = githubProfile.ProfileUsername;
            profileModel.stackOverUsername = stackOverProfile.ProfileUsername;
            profileModel.fullName = user.AccountFullName;
            

            //profileModel.githubUsername = db.Profil.FirstOrDefault(m=>m.AccountId = )

            for (int i = 0; i < profileMeta.Count(); i++)
            {
               // var isim = db.SourceDataType.FirstOrDefault(m => m.SourceDataTypeId == profileMeta[i].SourceDataTypeId).SourceDataTypeName;
                if (profileMeta[i].SourceDataTypeId == 11)//11 -> languages
                {
                   // profileModel.githubLanguages.Add(profileMeta[i].ProfileMetaValue); 
                }
             

            }
            for (int i = 0; i < profileScore.Count(); i++)
            {
                //var isim = db.SourceDataType.FirstOrDefault(m => m.SourceDataTypeId == profileScore[i].SourceDataTypeId).SourceDataTypeName;
                if (profileScore[i].SourceDataTypeId == 9)//9 -> github star count
                {
                    profileModel.githubStars = profileScore[i].ScoreValue;//burada ? olayi var 
                }
                else if(profileScore[i].SourceDataTypeId == 8)//repo count
                {
                    profileModel.githubRepos = profileScore[i].ScoreValue;
                }
                else if (profileScore[i].SourceDataTypeId == 10)//lang count
                {
                    profileModel.githubLanguageCount = profileScore[i].ScoreValue;
                }


            }
            Account model = new Account();
            string mail = Session["UserMail"].ToString();
            model = db.Account.FirstOrDefault(m => m.AccountMail == mail);
            return View(profileModel);
        }
        [AuthorizeRoleAttribute(RoleNames.Employee)]
        public ActionResult Home()
        {
            
            return View();
        }


        public ActionResult ProfileGraphStack(string Text)
        {

            getInfosStack(Text);

            var data = GetMarketSalesFromDatabaseStack(Text);


            //ViewBag.Diller = githubLanguageList;
            //ViewBag.DilSayisi = githubLanguageList.Count;


            return View(new SalesChartModel()
            {
                Title = "Stackoverflow Profile Summary",
                Subtitle = Text,
                DataTable = ConstructDataTableStack(data)


                //StackTitle = "Stackoverlow Profile Summary",
                //StackSubTitle=
            });

        }






    }
}