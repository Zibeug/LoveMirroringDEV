/*
 * Auteur : Sébastien Berger 
 * Date : 10.05.2020
 * Description : Contrôleur pour effectuer des recherches à partir de la vue et liker des utilisateurs
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using mvc.Models;
using mvc.ViewModels;
using Newtonsoft.Json;
using Unosquare.Swan;

namespace mvc.Controllers
{
    public class SearchController : Controller
    {
        private IConfiguration Configuration { get; set; }

        public SearchController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IActionResult> IndexAsync()
        {
            /*
                 * Auteur : Tim Allemann 
                 * Date : 15.05.2020
                 * Description : vérifie si l'utilisateur a un abonnement ou pas
                 * 
                 */
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

            if (user == null)
            {
                return NotFound();
            }

            content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/userSubscription");
            List<UserSubscription> userSubscriptions = JsonConvert.DeserializeObject<List<UserSubscription>>(content);
            userSubscriptions = userSubscriptions.Where(u => u.UserId == user.Id).ToList();

            if (userSubscriptions.Count() == 0)
            {
                ViewData["HasSubscription"] = false;
            }
            else
            {
                ViewData["HasSubscription"] = true;
                DateTime firstSubscriptionDate = userSubscriptions.Last().UserSubscriptionsDate;

                if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddMonths(1);
                }
                else if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                {
                    firstSubscriptionDate = firstSubscriptionDate.AddYears(1);
                }

                if (firstSubscriptionDate < DateTime.Now)
                {
                    ViewData["HasSubscription"] = false;
                }
            }

            return View("Search");
        }

        // Retourne la vue avec les profils qui correspondent
        [Authorize]
        public async Task<IActionResult> Search()
        {
            try
            {
                string accessToken = await HttpContext.GetTokenAsync("access_token");

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/search");
                List<MatchingModel> searchResult = JsonConvert.DeserializeObject<List<MatchingModel>>(search);
                ViewData["Search"] = searchResult;

                /*
                 * Auteur : Tim Allemann 
                 * Date : 15.05.2020
                 * Description : vérifie si l'utilisateur a un abonnement ou pas
                 * 
                 */
                string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/account/getUserInfo");
                AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(content);

                if (user == null)
                {
                    return NotFound();
                }

                content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Data/userSubscription");
                List<UserSubscription> userSubscriptions = JsonConvert.DeserializeObject<List<UserSubscription>>(content);
                userSubscriptions = userSubscriptions.Where(u => u.UserId == user.Id).ToList();

                if (userSubscriptions.Count() == 0)
                {
                    ViewData["HasSubscription"] = false;
                }
                else
                {
                    ViewData["HasSubscription"] = true;
                    DateTime firstSubscriptionDate = userSubscriptions.Last().UserSubscriptionsDate;

                    if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                    {
                        firstSubscriptionDate = firstSubscriptionDate.AddMonths(1);
                    }
                    else if (userSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                    {
                        firstSubscriptionDate = firstSubscriptionDate.AddYears(1);
                    }

                    if (firstSubscriptionDate < DateTime.Now)
                    {
                        ViewData["HasSubscription"] = false;
                    }
                }

                content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Home/GetAds");

                List<Ad> ads = JsonConvert.DeserializeObject<List<Ad>>(content);

                if (user.SubscriptionId == null)
                {
                    ViewData["img"] = this.RandomPicture(ads);
                    ViewData["URLAPI"] = Configuration["URLAPI"];
                    ViewData["link"] = ads.Where(x => x.AdView.Equals(ViewData["img"] as string)).SingleOrDefault().Link;

                }

                return View("Search");
            }
            catch (Exception)
            {
                return View("Error");
            }
            
        }

        // Permet de liker un utilisateur
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/Like");
            string json = JsonConvert.SerializeObject(username);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(client.BaseAddress, httpContent);
            var responseString = response.Result;
            return View("Like");
        }

        // Retourne les likes de l'utilisateur
        [Authorize]
        public async Task<IActionResult> Like()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string search = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            List<AspNetUser> userList = JsonConvert.DeserializeObject<List<AspNetUser>>(search);
            ViewData["UserList"] = userList;

            return View("Like");
        }

        // Permet d'enlever son like d'un utilisateur
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnLike(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/UnLike/" + username);
            var response = client.DeleteAsync(client.BaseAddress);
            var responseString = response.Result;
            return View("Search");
        }

        // Retourne les détails de l'utilisateur choisi dans la recherche ou dans les likes
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            client.BaseAddress = new Uri(Configuration["URLAPI"] + "api/Search/UserDetails/" + username);
            string userToDisplay = await client.GetStringAsync(client.BaseAddress);
            string userList = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/GetLike");
            string userMatch = await client.GetStringAsync(Configuration["URLAPI"] + "api/Search/CheckMatch/" + username);
            
            string userMatched = JsonConvert.DeserializeObject<string>(userMatch);
            List<AspNetUser> userLike = JsonConvert.DeserializeObject<List<AspNetUser>>(userList);
            AspNetUser user = JsonConvert.DeserializeObject<AspNetUser>(userToDisplay);

            ViewData["userLike"] = "NotLike";
            foreach (AspNetUser temp in userLike)
            {
                if (temp.Id == user.Id)
                {
                    ViewData["userLike"] = "Like";
                }
            }

            ViewData["match"] = userMatched;

            /* Paul Gillet
             * 16.06.2020 
             * Permet de retourner les preferences en commun
             */
            string userInfos = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            AspNetUser currentUser = JsonConvert.DeserializeObject<AspNetUser>(userInfos);

            string selectedUserInfos = await client.GetStringAsync(Configuration["URLAPI"] + $"api/Account/GetGivenUser/{user.Id}");
            AspNetUser selectedUser = JsonConvert.DeserializeObject<AspNetUser>(selectedUserInfos);

            List<string> listCommonPreferences = new List<string>();
            listCommonPreferences = GetCommonPreferences(currentUser, selectedUser);
            

            // afficher quoi ? 
            // les preferences que lui et la personne connectée on en commun (on aime les 2 les gros même si l'un et/ou l'autre ne l'est pas) 

            ViewData["commonPreferences"] = listCommonPreferences;

            return View(user);
        }

        /* Paul Gillet
        * 16.06.2020 
        * Permet de retourner les preferences en commun
        */
        private List<string> GetCommonPreferences(AspNetUser connectedUser,AspNetUser selectedUSer)
        {
            List<string> listCommonPreferences = new List<string>();

            foreach (var preferenceSelectedUser in selectedUSer.Preferences)
            {
                foreach (var preferenceConnectedUser in connectedUser.Preferences)
                {
                    if(preferenceConnectedUser.PreferenceReligions.Count > 0 || preferenceSelectedUser.PreferenceReligions.Count > 0) 
                    {
                        var religionConnectedUser = preferenceConnectedUser.PreferenceReligions.First();
                        var religionSelectedUser = preferenceSelectedUser.PreferenceReligions.First();
                        if (religionConnectedUser.ReligionId.Equals(religionSelectedUser.ReligionId))
                        {
                            listCommonPreferences.Add(religionConnectedUser.Religion.ReligionName);
                        }
                    }
                    
                    if(preferenceConnectedUser.PreferenceCorpulences.Count > 0 || preferenceSelectedUser.PreferenceCorpulences.Count > 0)
                    {
                        var corpulenceConnectedUser = preferenceConnectedUser.PreferenceCorpulences.First();
                        var corpulenceSelectedUser = preferenceSelectedUser.PreferenceCorpulences.First();
                        if (corpulenceConnectedUser.CorpulenceId.Equals(corpulenceSelectedUser.CorpulenceId))
                        {
                            listCommonPreferences.Add(corpulenceSelectedUser.Corpulence.CorpulenceName);
                        }
                    }

                    if(preferenceConnectedUser.PreferenceHairColors.Count > 0 || preferenceSelectedUser.PreferenceHairColors.Count > 0)
                    {
                        var haircolorConnectedUser = preferenceConnectedUser.PreferenceHairColors.First();
                        var haircolorSelectedUser = preferenceSelectedUser.PreferenceHairColors.First();
                        if (haircolorConnectedUser.HairColorId.Equals(haircolorSelectedUser.HairColorId))
                        {
                            listCommonPreferences.Add(haircolorConnectedUser.HairColor.HairColorName);
                        }
                    }

                    if(preferenceConnectedUser.PreferenceHairSizes.Count > 0 || preferenceSelectedUser.PreferenceHairSizes.Count > 0)
                    {
                        var hairsizeConnectedUser = preferenceConnectedUser.PreferenceHairSizes.First();
                        var hairsizeSelectedUser = preferenceSelectedUser.PreferenceHairSizes.First();
                        if (hairsizeConnectedUser.HairSizeId.Equals(hairsizeSelectedUser.HairSizeId))
                        {
                            listCommonPreferences.Add(hairsizeConnectedUser.HairSize.HairSizeName);
                        }
                    }
                   
                    if(preferenceConnectedUser.PreferenceMusics.Count > 0  || preferenceSelectedUser.PreferenceMusics.Count > 0)
                    {
                        var musicConnectedUser = preferenceConnectedUser.PreferenceMusics.First();
                        var musicSelectedUser = preferenceSelectedUser.PreferenceMusics.First();
                        if (musicConnectedUser.MusicId.Equals(musicSelectedUser.MusicId))
                        {
                            listCommonPreferences.Add(musicConnectedUser.Music.MusicName);
                        }
                    }
                    
                    if(preferenceConnectedUser.PreferenceStyles.Count > 0 || preferenceSelectedUser.PreferenceStyles.Count > 0)
                    {
                        var styleConnectedUser = preferenceConnectedUser.PreferenceStyles.First();
                        var styleSelectedUser = preferenceSelectedUser.PreferenceStyles.First();
                        if (styleConnectedUser.StyleId.Equals(styleSelectedUser.StyleId))
                        {
                            listCommonPreferences.Add(styleConnectedUser.Style.StyleName);
                        }
                    }
                }
            }

            return listCommonPreferences;
        }

        private string RandomPicture(List<Ad> ads)
        {
            Ad[] tabAd = ads.ToArray();
            Random rand = new Random();
            int nb = rand.Next(1, ads.Count() + 1);

            return tabAd[nb - 1].AdView;
        }

        // Permet de signaler un utilisateur
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Report(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            ReportViewModel report = new ReportViewModel();
            report.Username = username;

            return View(report);
        }


       /*
       *      Auteur : Hans Morsch
       *      11.05.2020
       *      Permet d'envoyer un rapport à tous les admins
       */
       [HttpPost]
       [Authorize]
        public async Task<IActionResult> ConfirmedReport(ReportViewModel report)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (ModelState.IsValid)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                StringContent httpContent = new StringContent(report.ToJson(), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(Configuration["URLAPI"] + $"api/Account/SendReportAdmin", httpContent);
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return BadRequest();
                }
            }

            return RedirectToAction(nameof(Search));
        }
    }
}