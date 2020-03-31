using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Messages = new HashSet<Message>();
            Pictures = new HashSet<Picture>();
            TalkIdNavigations = new HashSet<Talk>();
            TalkIdUser2TalksNavigation = new HashSet<Talk>();
            UsersExternalServices = new HashSet<UsersExternalService>();
            UsersMatchId1Navigation = new HashSet<UsersMatch>();
            UsersMatchIdNavigations = new HashSet<UsersMatch>();
            UsersNewsLetters = new HashSet<UsersNewsLetter>();
            UsersPreferences = new HashSet<UsersPreference>();
            UsersProfils = new HashSet<UsersProfil>();
        }

        [Key]
        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string ConcurrencyStamp { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        [StringLength(256)]
        public string NormalizedEmail { get; set; }
        [StringLength(256)]
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        public DateTime Birthday { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public bool QuizCompleted { get; set; }
        public short SexeId { get; set; }
        public short? SubscriptionId { get; set; }

        [ForeignKey(nameof(SexeId))]
        [InverseProperty(nameof(Sex.AspNetUsers))]
        public virtual Sex Sexe { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        [InverseProperty("AspNetUsers")]
        public virtual Subscription Subscription { get; set; }
        [InverseProperty(nameof(AspNetUserClaim.User))]
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        [InverseProperty(nameof(AspNetUserLogin.User))]
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        [InverseProperty(nameof(AspNetUserRole.User))]
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        [InverseProperty(nameof(AspNetUserToken.User))]
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        [InverseProperty(nameof(Message.IdNavigation))]
        public virtual ICollection<Message> Messages { get; set; }
        [InverseProperty(nameof(Picture.IdNavigation))]
        public virtual ICollection<Picture> Pictures { get; set; }
        [InverseProperty(nameof(Talk.IdNavigation))]
        public virtual ICollection<Talk> TalkIdNavigations { get; set; }
        [InverseProperty(nameof(Talk.IdUser2TalksNavigation))]
        public virtual ICollection<Talk> TalkIdUser2TalksNavigation { get; set; }
        [InverseProperty(nameof(UsersExternalService.IdNavigation))]
        public virtual ICollection<UsersExternalService> UsersExternalServices { get; set; }
        [InverseProperty(nameof(UsersMatch.Id1Navigation))]
        public virtual ICollection<UsersMatch> UsersMatchId1Navigation { get; set; }
        [InverseProperty(nameof(UsersMatch.IdNavigation))]
        public virtual ICollection<UsersMatch> UsersMatchIdNavigations { get; set; }
        [InverseProperty(nameof(UsersNewsLetter.IdNavigation))]
        public virtual ICollection<UsersNewsLetter> UsersNewsLetters { get; set; }
        [InverseProperty(nameof(UsersPreference.IdNavigation))]
        public virtual ICollection<UsersPreference> UsersPreferences { get; set; }
        [InverseProperty(nameof(UsersProfil.IdNavigation))]
        public virtual ICollection<UsersProfil> UsersProfils { get; set; }
    }
}
