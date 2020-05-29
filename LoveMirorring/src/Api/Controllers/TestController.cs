using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Unosquare.Swan;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly LoveMirroringContext _context;
        private readonly IEmailSender _emailSender;
        private IConfiguration _configuration { get; }

        public TestController(LoveMirroringContext context, IEmailSender emailSender, IConfiguration configuration)
        {
            _context = context;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public class MyMatchingList
        {
            public List<MatchingModel> ListMatchinModel { get; set; }
            public string UserName { get; set; }
        }

        // GET: api/Test
        [HttpGet]
        public async Task<List<MyMatchingList>> GetAsync()
        {
            List<MyMatchingList> listFromUserMatchs = new List<MyMatchingList>();

            List<AspNetUser> users = GetUsers();

            foreach (AspNetUser user in users)
            {
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
                    // Le potentiel commence à 0.25 car age et profil obligatoire (chacun vaut 0.125
                    double potentielPourcentage = 0.25;

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
                            potentielPourcentage += 0.125;
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
                                potentielPourcentage += 0.125;
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
                                potentielPourcentage += 0.125;
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
                                potentielPourcentage += 0.125;
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
                            potentielPourcentage += 0.125;
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
                                potentielPourcentage += 0.125;
                                break;
                            }
                        }
                    }

                    //Vérifier si la musique correspond
                    string musicName = "";
                    string musicMatch = potentialUserMatch.UserMusics.FirstOrDefault().Music.MusicName;
                    foreach(Preference preferenceUser in user.Preferences)
                    {
                        foreach(PreferenceMusic preferenceMusic in preferenceUser.PreferenceMusics)
                        {
                            if (preferenceMusic.Music.MusicName == musicMatch)
                            {
                                musicName = musicMatch;
                                potentielPourcentage += 0.125;
                                break;
                            }
                        }
                    }

                    //Vérifier si l'artiste correspond
                    string artistName = "";
                    string artistMatch = potentialUserMatch.UserMusics.FirstOrDefault().Music.ArtistName;
                    foreach(Preference preferenceUser in user.Preferences)
                    {
                        foreach(PreferenceMusic preferenceMusic in preferenceUser.PreferenceMusics)
                        {
                            if(preferenceMusic.Music.ArtistName == artistMatch)
                            {
                                artistName = artistMatch;
                                potentielPourcentage += 0.125;
                                break;
                            }
                        }
                    }

                    // Ajout du match
                    usersChoices.Add(
                        new MatchingModel
                        {
                            Id = potentialUserMatch.Id,
                            UserName = potentialUserMatch.UserName,
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

                listFromUserMatchs.Add(new MyMatchingList
                {
                    ListMatchinModel = usersChoices,
                    UserName = user.UserName
                });

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
                    if (hasSubscription)
                    {
                        // Envoyer un seul profil (si possible)
                        string message = "<table>" +
                                             "<tr>" +
                                                 "<th>Nouveau match</th>" +
                                                 "<th></th> " +
                                             "</tr> " +
                                             "<tr>" +
                                                  "<th>" + usersChoices[0].UserName + "</th>" +
                                                  "<th><a href='" + _configuration["URLClientMVC"] + "/Search'>Découvre son profil</a></th> " +
                                             "</tr>" +
                                        "</table>";
                        // envoyer mail
                        await _emailSender.SendEmailAsync(
                                 user.Email,
                                 "Vous avez un nouveau match",
                                 message);

                    }
                    else
                    {
                        string message = "<table>" +
                                            "<tr>" +
                                                "<th>Nouveau match</th>" +
                                                "<th></th> " +
                                            "</tr> ";
                        // Envoyer 10 profils (si possible)
                        for (int i = 0; i < 10 && i < usersChoices.Count(); i++)
                        {
                            message +=      "<tr>" +
                                                "<th>" + usersChoices[i].UserName + "</th>" +
                                                "<th><a href='" + _configuration["URLClientMVC"] + "/Search'>Découvre son profil</a></th> " +
                                             "</tr>";
                        }
                        message +=        "</table>";
                        // envoyer mail
                        await _emailSender.SendEmailAsync(
                                 user.Email,
                                 "Vous avez un nouveau match",
                                 message);
                    }
                }

            }

            return listFromUserMatchs;
        }

        private List<AspNetUser> GetUsers()
        {
            return _context.AspNetUsers
                                .Include(u => u.UserSubscriptions)
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
