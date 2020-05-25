using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc.Models
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
            Preferences = new HashSet<Preference>();
            TalkIdNavigations = new HashSet<Talk>();
            TalkIdUser2TalkNavigation = new HashSet<Talk>();
            UserExternalServices = new HashSet<UserExternalService>();
            UserLikeId1Navigation = new HashSet<UserLike>();
            UserLikeIdNavigations = new HashSet<UserLike>();
            UserMusics = new HashSet<UserMusic>();
            UserNewsletters = new HashSet<UserNewsletter>();
            UserProfils = new HashSet<UserProfil>();
            UserStyles = new HashSet<UserStyle>();
            UserSubscriptions = new HashSet<UserSubscription>();
            UserTraces = new HashSet<UserTrace>();
        }

        [Key]
        public string Id { get; set; }
        public short? HairColorId { get; set; }
        public short? CorpulenceId { get; set; }
        public short? SexeId { get; set; }
        public short? HairSizeId { get; set; }
        public short? SubscriptionId { get; set; }
        public short? SexualityId { get; set; }
        public short? ReligionId { get; set; }
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
        public bool AccountCompleted { get; set; }

        [ForeignKey(nameof(CorpulenceId))]
        [InverseProperty("AspNetUsers")]
        public virtual Corpulence Corpulence { get; set; }
        [ForeignKey(nameof(HairColorId))]
        [InverseProperty("AspNetUsers")]
        public virtual HairColor HairColor { get; set; }
        [ForeignKey(nameof(HairSizeId))]
        [InverseProperty("AspNetUsers")]
        public virtual HairSize HairSize { get; set; }
        [ForeignKey(nameof(ReligionId))]
        [InverseProperty("AspNetUsers")]
        public virtual Religion Religion { get; set; }
        [ForeignKey(nameof(SexeId))]
        [InverseProperty(nameof(Sex.AspNetUsers))]
        public virtual Sex Sexe { get; set; }
        [ForeignKey(nameof(SexualityId))]
        [InverseProperty("AspNetUsers")]
        public virtual Sexuality Sexuality { get; set; }
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
        [InverseProperty(nameof(Preference.IdNavigation))]
        public virtual ICollection<Preference> Preferences { get; set; }
        [InverseProperty(nameof(Talk.IdNavigation))]
        public virtual ICollection<Talk> TalkIdNavigations { get; set; }
        [InverseProperty(nameof(Talk.IdUser2TalkNavigation))]
        public virtual ICollection<Talk> TalkIdUser2TalkNavigation { get; set; }
        [InverseProperty(nameof(UserExternalService.IdNavigation))]
        public virtual ICollection<UserExternalService> UserExternalServices { get; set; }
        [InverseProperty(nameof(UserLike.Id1Navigation))]
        public virtual ICollection<UserLike> UserLikeId1Navigation { get; set; }
        [InverseProperty(nameof(UserLike.IdNavigation))]
        public virtual ICollection<UserLike> UserLikeIdNavigations { get; set; }
        [InverseProperty(nameof(UserMusic.IdNavigation))]
        public virtual ICollection<UserMusic> UserMusics { get; set; }
        [InverseProperty(nameof(UserNewsletter.IdNavigation))]
        public virtual ICollection<UserNewsletter> UserNewsletters { get; set; }
        [InverseProperty(nameof(UserProfil.IdNavigation))]
        public virtual ICollection<UserProfil> UserProfils { get; set; }
        [InverseProperty(nameof(UserStyle.IdNavigation))]
        public virtual ICollection<UserStyle> UserStyles { get; set; }
        [InverseProperty(nameof(UserSubscription.User))]
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
        [InverseProperty(nameof(UserTrace.IdNavigation))]
        public virtual ICollection<UserTrace> UserTraces { get; set; }
    }
}
