/*
 * Auteur : Sébastien Berger
 * Date : 07.05.2020
 * Détail : Contrôleur pour effectuer une recherche d'un ou plusieurs utilisateurs en fonction de ses préférences
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private LoveMirroringContext _context;
        private const double MATCHING = 0.1;
        private IConfiguration Configuration { get; set; }

        public SearchController(LoveMirroringContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // Permet de traiter la recherche pour l'utilisateur courant
        // GET : api/Search/search
        [Route("search")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> OnPostSearchAsync()
        {
            List<AspNetUser> users = GetUsers();

            AspNetUser user = null;
            string id = "";
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string checkPref = await client.GetStringAsync(Configuration["URLAPI"] + "api/Matching/checkPreferences");

            string result = JsonConvert.DeserializeObject<string>(checkPref);

            if (result.Equals("error"))
            {
                return BadRequest();
            }

            // Récurération des données et convertion des données dans le bon type
            string content = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(content);

            // Déterminer s'il y a des matchs
            List<MatchingModel> usersChoices = new List<MatchingModel>();

            // Premier tri obligatoire : 
            // Sortir de la liste les utilisateurs déjà "aimé", 
            // dont l'age ne correspond par à la préférence
            // dont le sexe ne correspond pas à la préférence
            // dont l'orientation sexuelle n'est pas la même
            List<AspNetUser> potentialUserMatchs = GetPotentialUsers(user, users);

            // Ajouter et calculer le potentiel du match : 100% = couple parfait
            foreach (AspNetUser potentialUserMatch in potentialUserMatchs)
            {
                int nbPreferenceCommun = 0;
                // Le potentiel commence à 0.2 car age et profil obligatoire (chacun vaut 0.125
                double potentielPourcentage = MATCHING * 2;

                // Vérifier si le profil correspond
                List<string> potentialUserMatchProfil = new List<string>();
                foreach (UserProfil userProfil in potentialUserMatch.UserProfils)
                {
                    potentialUserMatchProfil.Add(userProfil.Profil.ProfilName);
                }

                string profil = "";
                List<string> potentialUserProfil = new List<string>();
                foreach (UserProfil userProfil in user.UserProfils)
                {
                    potentialUserProfil.Add(userProfil.Profil.ProfilName);
                }

                foreach (string userMatchProfil in potentialUserMatchProfil)
                {
                    if (potentialUserProfil.Contains(userMatchProfil))
                    {
                        profil = userMatchProfil;
                        potentielPourcentage += MATCHING;
                        break;
                    }
                }


                // Vérifier si la corpulence correspond
                string corpulence = "";
                string corpulenceUserMatch = potentialUserMatch.Corpulence.CorpulenceName;
                foreach (Preference preferenceUser in user.Preferences)
                {
                    foreach (PreferenceCorpulence preferenceCorpulenceUser in preferenceUser.PreferenceCorpulences)
                    {
                        if (preferenceCorpulenceUser.Corpulence.CorpulenceName == corpulenceUserMatch)
                        {
                            corpulence = corpulenceUserMatch;
                            potentielPourcentage += MATCHING;
                            break;
                        }
                    }
                }

                // Vérifier si la couleur de cheveux correspond
                string hairColor = "";
                string hairColorUserMatch = potentialUserMatch.HairColor.HairColorName;
                foreach (Preference preferenceUser in user.Preferences)
                {
                    foreach (PreferenceHairColor preferenceHairColorUser in preferenceUser.PreferenceHairColors)
                    {
                        if (preferenceHairColorUser.HairColor.HairColorName == hairColorUserMatch)
                        {
                            hairColor = hairColorUserMatch;
                            potentielPourcentage += MATCHING;
                            break;
                        }
                    }
                }

                // Vérifier si la taille de cheveux correspond
                string hairSize = "";
                string hairSizeUserMatch = potentialUserMatch.HairSize.HairSizeName;
                foreach (Preference preferenceUser in user.Preferences)
                {
                    foreach (PreferenceHairSize preferenceHairSizeUser in preferenceUser.PreferenceHairSizes)
                    {
                        if (preferenceHairSizeUser.HairSize.HairSizeName == hairSizeUserMatch)
                        {
                            hairSize = hairSizeUserMatch;
                            potentielPourcentage += MATCHING;
                            break;
                        }
                    }
                }

                // Vérifier si le style correspond
                List<string> potentialUserMatchStyle = new List<string>();
                foreach (UserStyle userStyle in potentialUserMatch.UserStyles)
                {
                    potentialUserMatchStyle.Add(userStyle.Style.StyleName);
                }

                List<string> potentialUserStyle = new List<string>();
                foreach (UserStyle userStyle in user.UserStyles)
                {
                    potentialUserStyle.Add(userStyle.Style.StyleName);
                }

                string style = "";
                foreach (string userMatchStyle in potentialUserMatchStyle)
                {
                    if (potentialUserStyle.Contains(userMatchStyle))
                    {
                        style = userMatchStyle;
                        potentielPourcentage += MATCHING;
                        break;
                    }
                }

                // Vérifier si la religion correspond
                string religion = "";
                string religionUserMatch = potentialUserMatch.Religion.ReligionName;
                foreach (Preference preferenceUser in user.Preferences)
                {
                    foreach (PreferenceReligion preferenceReligionUser in preferenceUser.PreferenceReligions)
                    {
                        if (preferenceReligionUser.Religion.ReligionName == religionUserMatch)
                        {
                            religion = religionUserMatch;
                            potentielPourcentage += MATCHING;
                            break;
                        }
                    }
                }

                //Vérifier si la musique correspond
                string musicName = "";
                if (potentialUserMatch.UserMusics.Count() > 0)
                {
                    string musicMatch = potentialUserMatch.UserMusics.FirstOrDefault().Music.MusicName;
                    foreach (Preference preferenceUser in user.Preferences)
                    {
                        foreach (PreferenceMusic preferenceMusic in preferenceUser.PreferenceMusics)
                        {
                            if (preferenceMusic.Music.MusicName == musicMatch)
                            {
                                musicName = musicMatch;
                                potentielPourcentage += MATCHING;
                                break;
                            }
                        }
                    }
                }

                //Vérifier si l'artiste correspond
                string artistName = "";
                if (potentialUserMatch.UserMusics.Count() > 0)
                {
                    string artistMatch = potentialUserMatch.UserMusics.FirstOrDefault().Music.ArtistName;
                    foreach (Preference preferenceUser in user.Preferences)
                    {
                        foreach (PreferenceMusic preferenceMusic in preferenceUser.PreferenceMusics)
                        {
                            if (preferenceMusic.Music.ArtistName == artistMatch)
                            {
                                artistName = artistMatch;
                                potentielPourcentage += MATCHING;
                                break;
                            }
                        }
                    }
                }

                // Ajout du match
                usersChoices.Add(
                    new MatchingModel
                    {
                        Id = potentialUserMatch.Id,
                        UserName = potentialUserMatch.UserName,
                        Firstname = potentialUserMatch.Firstname,
                        Age = DateTime.Now.Year - potentialUserMatch.Birthday.Year,
                        Sexe = potentialUserMatch.Sexe.SexeName,
                        Profil = profil,
                        Corpulence = corpulence,
                        HairColor = hairColor,
                        HairSize = hairSize,
                        Style = style,
                        Religion = religion,
                        Sexuality = user.Sexuality.SexualityName,
                        MusicName = musicName,
                        ArtisteName = artistName,
                        PourcentageMatching = potentielPourcentage
                    }
                );

            }

            // Vérifier si le user possède au moins un abonnement
            bool hasSubscription = false;
            if (user.UserSubscriptions.Count() > 0)
            {
                DateTime lastSubscriptionDate = user.UserSubscriptions.Last().UserSubscriptionsDate;

                // Vérifier quel type d'abonnement le user a
                if (user.UserSubscriptions.Last().Subscriptions.SubscriptionName == "1 Mois")
                {
                    lastSubscriptionDate = lastSubscriptionDate.AddMonths(1);
                }
                else if (user.UserSubscriptions.Last().Subscriptions.SubscriptionName == "1 Année")
                {
                    lastSubscriptionDate = lastSubscriptionDate.AddYears(1);
                }

                // Vérifier si son abonnement est toujours valable
                if (lastSubscriptionDate < DateTime.Now)
                {
                    hasSubscription = false;
                }
                else
                {
                    hasSubscription = true;
                }
            }

            // Ne garder que les profils qui correspondent à 75% et plus
            usersChoices = usersChoices.Where(u => u.PourcentageMatching >= 0.75).ToList();
            if (usersChoices.Count() > 0)
            {
                if (!hasSubscription)
                {
                    MatchingModel one = usersChoices[0];
                    usersChoices = new List<MatchingModel>();
                    usersChoices.Add(one);
                }
            }

            return new JsonResult(usersChoices);
        }

        // Permet de liker un utilisateur
        // POST: api/Search/Like
        [Route("Like")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Like([FromBody]string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);
            UserLike ul = new UserLike();
            AspNetUser userLiked = _context.AspNetUsers.Where(d => d.UserName == username).Single();

            ul.Id = user.Id;
            ul.Id1 = userLiked.Id;
            ul.Ignored = false;

            try
            {
                //cherche si il existe une conversation entre les deux personnes
                Talk talk = _context.Talks.Where(t => t.Id == user.Id && t.IdUser2Talk == userLiked.Id).SingleOrDefault();
                if (talk == null)
                {
                    talk = _context.Talks.Where(t => t.Id == userLiked.Id && t.IdUser2Talk == user.Id).SingleOrDefault();
                }
                //crée une conversation si la conversation n'existe pas
                if (talk == null)
                {
                    Talk newtalk = new Talk { Id = user.Id, IdUser2Talk = userLiked.Id, TalkName = user.NormalizedUserName + userLiked.NormalizedUserName };
                    _context.Talks.Add(newtalk);
                    await _context.SaveChangesAsync();
                }
                _context.UserLikes.Add(ul);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // Permet de récupérer les likes d'un utilisateur courant
        // GET : api/Search/GetLike
        [Route("GetLike")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetLike()
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser user = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            user = JsonConvert.DeserializeObject<AspNetUser>(userString);

            List<UserLike> ul = _context.UserLikes.Where(d => d.Id == user.Id).ToList();
            List<AspNetUser> userList = new List<AspNetUser>();

            foreach(UserLike u in ul)
            {
                userList.Add(_context.AspNetUsers.Where(d => d.Id == u.Id1).Single());
            }

            return new JsonResult(userList);
        }

        // Permet d'enlever le like sur un utilisateur dans la liste des likes
        // DELETE : /api/Search/UnLike/user1
        [Route("UnLike/{username}")]
        [HttpDelete("{username}")]
        [Authorize]
        public async Task<IActionResult> DeleteSearch(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser currentUser = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            string us = username;
            currentUser = JsonConvert.DeserializeObject<AspNetUser>(userString);

            AspNetUser user = _context.AspNetUsers.Where(d => d.UserName == username).Single();

            if(user == null)
            {
                return BadRequest();
            }
            else
            {
                try
                {
                    UserLike userLike = _context.UserLikes.Where(d => d.Id == currentUser.Id && d.Id1 == user.Id).Single();
                    Talk talk = _context.Talks.Where(t => t.Id == currentUser.Id && t.IdUser2Talk == user.Id).SingleOrDefault();
                    List<Message> messages = null;
                    if (talk == null)
                    {
                        talk = _context.Talks.Where(t => t.Id == user.Id && t.IdUser2Talk == currentUser.Id).SingleOrDefault();
                    }                  
                    if(talk!= null)
                    {
                        messages = _context.Messages.Where(m => m.TalkId == talk.TalkId).ToList();
                    }              
                    if (messages != null) 
                    {
                        foreach (Message message in messages)
                        {
                            _context.Remove(message);
                        }
                    }
                    if(talk != null)
                    {
                        _context.Talks.Remove(talk);
                    }
                    _context.UserLikes.Remove(userLike);
                    _context.SaveChanges();
                    return Ok();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
        }

        //Permet de retourner les détails de l'utilisateur qu'on a trouvé ou liké
        //GET : api/Search/UserDetails/user1
        [Route("UserDetails/{username}")]
        [HttpGet("{username}")]
        [Authorize]
        public IActionResult GetDetails(string username)
        {
            AspNetUser userDisplay = _context.AspNetUsers
                .Include(a => a.Corpulence)
                .Include(a => a.HairColor)
                .Include(a => a.HairSize)
                .Include(a => a.Sexe)
                .Include(a => a.Sexuality)
                .Include(a => a.Subscription)
                .Include(a => a.UserStyles)
                .Include(a => a.UserSubscriptions)
                .Include(a => a.UserTraces)
                .Include(a => a.Religion)
                .Include(a => a.UserStyles)
                .Where(d => d.UserName == username)
                .Single();

            if (userDisplay == null)
            {
                return NotFound();
            }

            return new JsonResult(userDisplay);
        }

        //Permet de vérifier si les utilisateurs se sont likés mutuellement
        // GET : api/Search/CheckMatch/user1
        [Route("CheckMatch/{username}")]
        [HttpGet("{username}")]
        [Authorize]
        public async Task<IActionResult> Match(string username)
        {
            string accessToken = await HttpContext.GetTokenAsync("access_token");
            AspNetUser currentUser = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string userString = await client.GetStringAsync(Configuration["URLAPI"] + "api/Account/getUserInfo");
            currentUser = JsonConvert.DeserializeObject<AspNetUser>(userString);

            AspNetUser user = _context.AspNetUsers.Where(d => d.UserName == username).Single();

            UserLike likeCurrentUser = _context.UserLikes.Where(d => d.Id == currentUser.Id && d.Id1 == user.Id).SingleOrDefault();
            UserLike likeUser = _context.UserLikes.Where(d => d.Id == user.Id && d.Id1 == currentUser.Id).SingleOrDefault();

            if(likeCurrentUser != null && likeUser != null)
            {
                return new JsonResult("match");
            }
            else
            {
                return new JsonResult("nMatch");
            }
        }

        private List<AspNetUser> GetUsers()
        {
            return _context.AspNetUsers
                                .Include(u => u.UserSubscriptions)
                                    .ThenInclude(u => u.Subscriptions)
                                .Include(u => u.Sexe)
                                .Include(u => u.Religion)
                                .Include(u => u.HairSize)
                                .Include(u => u.HairColor)
                                .Include(u => u.Corpulence)
                                .Include(u => u.Sexuality)
                                // Ses préférences
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceCorpulences)
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceHairColors)
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceHairSizes)
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceMusics)
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceReligions)
                                .Include(u => u.Preferences)
                                    .ThenInclude(u => u.PreferenceStyles)
                                .Include(u => u.UserProfils)
                                    .ThenInclude(u => u.Profil)
                                .Include(u => u.UserStyles)
                                    .ThenInclude(u => u.Style)
                                .Include(u => u.UserMusics)
                                    .ThenInclude(u => u.Music)
                                .ToList();
        }

        // Premier tri obligatoire : 
        // Sortir de la liste les utilisateurs déjà "aimé", 
        // dont l'age ne correspond par à la préférence
        // dont le sexe ne correspond pas à la préférence
        // dont l'orientation sexuelle n'est pas la même
        private List<AspNetUser> GetPotentialUsers(AspNetUser user, List<AspNetUser> users)
        {
            // Sortir de la liste les utilisateurs déjà "aimé"
            List<string> usersAlreadyLiked = _context.UserLikes.Where(u => u.Id == user.Id).Select(u => u.Id1).ToList();
            List<AspNetUser> potentialUserMatchs = users.Where(u => !usersAlreadyLiked.Contains(u.Id)).ToList();
            // Sortir l'utilisateur courant de la liste
            potentialUserMatchs = potentialUserMatchs.Where(u => u.Id != user.Id).ToList();
            // Sortir de la liste les utilisateurs dont l'age ne correspond par à la préférence
            if (user.Preferences.Count() > 0)
            {
                potentialUserMatchs = potentialUserMatchs
                                        .Where(u => DateTime.Now.Year - u.Birthday.Year > user.Preferences.Min(p => p.AgeMin) &&
                                                    DateTime.Now.Year - u.Birthday.Year < user.Preferences.Max(p => p.AgeMax))
                                        .ToList();
            }

            // Sortir de la liste les utilisateurs dont le sexe ne correspond pas à la préférence
            if (user.Sexuality.SexualityName == "Hétérosexuel")
            {
                potentialUserMatchs = potentialUserMatchs.Where(u => u.SexeId != user.SexeId).ToList();
            }
            else if (user.Sexuality.SexualityName == "Homosexuel")
            {
                potentialUserMatchs = potentialUserMatchs.Where(u => u.SexeId == user.SexeId).ToList();
            }

            // Sortir de la liste les utilisateurs dont l'orientation sexuelle n'est pas la même
            potentialUserMatchs = potentialUserMatchs.Where(u => u.SexualityId == user.SexualityId).ToList();

            return potentialUserMatchs;
        }
    }
}