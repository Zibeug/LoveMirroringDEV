using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Api.Models
{
    public partial class LoveMirroringContext : DbContext
    {
        public LoveMirroringContext()
        {
        }

        public LoveMirroringContext(DbContextOptions<LoveMirroringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Corpulence> Corpulences { get; set; }
        public virtual DbSet<ExternalService> ExternalServices { get; set; }
        public virtual DbSet<HairColor> HairColors { get; set; }
        public virtual DbSet<HairSize> HairSizes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Music> Musics { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PictureTag> PictureTags { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<PreferenceCorpulence> PreferenceCorpulences { get; set; }
        public virtual DbSet<PreferenceHairColor> PreferenceHairColors { get; set; }
        public virtual DbSet<PreferenceHairSize> PreferenceHairSizes { get; set; }
        public virtual DbSet<PreferenceMusic> PreferenceMusics { get; set; }
        public virtual DbSet<PreferenceReligion> PreferenceReligions { get; set; }
        public virtual DbSet<PreferenceStyle> PreferenceStyles { get; set; }
        public virtual DbSet<Profil> Profils { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<Sexuality> Sexualities { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Talk> Talks { get; set; }
        public virtual DbSet<UserExternalService> UserExternalServices { get; set; }
        public virtual DbSet<UserLike> UserLikes { get; set; }
        public virtual DbSet<UserMusic> UserMusics { get; set; }
        public virtual DbSet<UserNewsletter> UserNewsletters { get; set; }
        public virtual DbSet<UserProfil> UserProfils { get; set; }
        public virtual DbSet<UserStyle> UserStyles { get; set; }
        public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }
        public virtual DbSet<UserTrace> UserTraces { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=LoveMirroring;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerText).IsUnicode(false);

                entity.HasOne(d => d.Profil)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.ProfilId)
                    .HasConstraintName("FK_ANSWERS_PROFILS");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK_ANSWERS_QUESTIONS");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.NormalizedName).IsUnicode(false);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_ASPNETROLECLAIMS_ASPNETROLES");
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("AC_Email")
                    .IsUnique();

                entity.HasOne(d => d.Corpulence)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.CorpulenceId)
                    .HasConstraintName("FK_ASPNETUSERS_CORPULENCES");

                entity.HasOne(d => d.HairColor)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.HairColorId)
                    .HasConstraintName("FK_ASPNETUSERS_HAIRCOLORS");

                entity.HasOne(d => d.HairSize)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.HairSizeId)
                    .HasConstraintName("FK_ASPNETUSERS_HAIRSIZES");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.ReligionId)
                    .HasConstraintName("FK_ASPNETUSERS_RELIGION");

                entity.HasOne(d => d.Sexe)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SexeId)
                    .HasConstraintName("FK_ASPNETUSERS_SEXES");

                entity.HasOne(d => d.Sexuality)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SexualityId)
                    .HasConstraintName("FK_ASPNETUSERS_SEXUALITIES");

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SubscriptionId)
                    .HasConstraintName("FK_ASPNETUSERS_SUBSCRIPTIONS");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ASPNETUSERCLAIMS_ASPNETUSERS");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.Property(e => e.LoginProvider).IsUnicode(false);

                entity.Property(e => e.ProviderKey).IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ASPNETUSERLOGINS_ASPNETUSERS");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_ASPNETUSERTOKENS_ASPNETUSERS");
            });

            modelBuilder.Entity<Corpulence>(entity =>
            {
                entity.Property(e => e.CorpulenceName).IsUnicode(false);
            });

            modelBuilder.Entity<ExternalService>(entity =>
            {
                entity.Property(e => e.ExternalServiceName).IsUnicode(false);
            });

            modelBuilder.Entity<HairColor>(entity =>
            {
                entity.Property(e => e.HairColorName).IsUnicode(false);
            });

            modelBuilder.Entity<HairSize>(entity =>
            {
                entity.Property(e => e.HairSizeName).IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK_MESSAGES");

                entity.Property(e => e.MessageText).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_MESSAGES_ASPNETUSERS");

                entity.HasOne(d => d.Talk)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.TalkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGES_TALKS");
            });

            modelBuilder.Entity<Music>(entity =>
            {
                entity.Property(e => e.ArtistName).IsUnicode(false);

                entity.Property(e => e.MusicName).IsUnicode(false);
            });

            modelBuilder.Entity<Newsletter>(entity =>
            {
                entity.Property(e => e.NewsletterName).IsUnicode(false);
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.PictureId)
                    .HasName("PK_PICTURES");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_PICTURES_ASPNETUSERS");
            });

            modelBuilder.Entity<PictureTag>(entity =>
            {
                entity.HasKey(e => new { e.PictureId, e.TagId })
                    .HasName("PK_PICTURETAG");

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.PictureTags)
                    .HasForeignKey(d => d.PictureId)
                    .HasConstraintName("FK_PICTURETAG_PICTURES");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PictureTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PICTURETAG_TAGS");
            });

            modelBuilder.Entity<Preference>(entity =>
            {
                entity.HasKey(e => e.PreferenceId)
                    .HasName("PK_PREFERENCES");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Preferences)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_PREFERENCES_ASPNETUSERS");

                entity.HasOne(d => d.Sexuality)
                    .WithMany(p => p.Preferences)
                    .HasForeignKey(d => d.SexualityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCES_SEXUALITIES");
            });

            modelBuilder.Entity<PreferenceCorpulence>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.CorpulenceId })
                    .HasName("PK_PREFERENCECORPULENCES");

                entity.HasOne(d => d.Corpulence)
                    .WithMany(p => p.PreferenceCorpulences)
                    .HasForeignKey(d => d.CorpulenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCECORPULENCES_CORPULENCES");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceCorpulences)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCECORPULENCES_PREFERENCES");
            });

            modelBuilder.Entity<PreferenceHairColor>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.HairColorId })
                    .HasName("PK_PREFERENCEHAIRCOLORS");

                entity.HasOne(d => d.HairColor)
                    .WithMany(p => p.PreferenceHairColors)
                    .HasForeignKey(d => d.HairColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRCOLORS_HAIRCOLORS");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceHairColors)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCEHAIRCOLORS_PREFERENCES");
            });

            modelBuilder.Entity<PreferenceHairSize>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.HairSizeId })
                    .HasName("PK_PREFERENCEHAIRSIZES");

                entity.HasOne(d => d.HairSize)
                    .WithMany(p => p.PreferenceHairSizes)
                    .HasForeignKey(d => d.HairSizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRSIZES_HAIRSIZES");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceHairSizes)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCEHAIRSIZES_PREFERENCES");
            });

            modelBuilder.Entity<PreferenceMusic>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.MusicId })
                    .HasName("PK_PREFERENCEMUSICS");

                entity.HasOne(d => d.Music)
                    .WithMany(p => p.PreferenceMusics)
                    .HasForeignKey(d => d.MusicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEMUSICS_MUSICS");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceMusics)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCEMUSICS_PREFERENCES");
            });

            modelBuilder.Entity<PreferenceReligion>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.ReligionId })
                    .HasName("PK_PREFERENCERELIGIONS");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceReligions)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCERELIGIONS_PREFERENCES");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.PreferenceReligions)
                    .HasForeignKey(d => d.ReligionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCERELIGIONS_RELIGIONS");
            });

            modelBuilder.Entity<PreferenceStyle>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.StyleId })
                    .HasName("PK_PREFERENCESTYLES");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferenceStyles)
                    .HasForeignKey(d => d.PreferenceId)
                    .HasConstraintName("FK_PREFERENCESTYLES_PREFERENCES");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.PreferenceStyles)
                    .HasForeignKey(d => d.StyleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCESTYLES_STYLES");
            });

            modelBuilder.Entity<Profil>(entity =>
            {
                entity.Property(e => e.ProfilName).IsUnicode(false);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionText).IsUnicode(false);
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.Property(e => e.ReligionName).IsUnicode(false);
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.HasKey(e => e.SexeId)
                    .HasName("PK_SEXES");

                entity.Property(e => e.SexeName).IsUnicode(false);
            });

            modelBuilder.Entity<Sexuality>(entity =>
            {
                entity.Property(e => e.SexualityName).IsUnicode(false);
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.Property(e => e.StyleName).IsUnicode(false);
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.SubscriptionName).IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName).IsUnicode(false);
            });

            modelBuilder.Entity<Talk>(entity =>
            {
                entity.HasKey(e => e.TalkId)
                    .HasName("PK_TALKS");

                entity.Property(e => e.TalkName).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TalkIdNavigations)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_TALKS_ASPNETUSERS");

                entity.HasOne(d => d.IdUser2TalkNavigation)
                    .WithMany(p => p.TalkIdUser2TalkNavigation)
                    .HasForeignKey(d => d.IdUser2Talk)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALKS_ASPNETUSERS1");
            });

            modelBuilder.Entity<UserExternalService>(entity =>
            {
                entity.HasKey(e => new { e.ExternalServiceId, e.Id })
                    .HasName("PK_USEREXTERNALSERVICES");

                entity.HasOne(d => d.ExternalService)
                    .WithMany(p => p.UserExternalServices)
                    .HasForeignKey(d => d.ExternalServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USEREXTERNALSERVICES_EXTERNALSERVICES");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserExternalServices)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USEREXTERNALSERVICES_ASPNETUSERS");
            });

            modelBuilder.Entity<UserLike>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Id1 })
                    .HasName("PK_USERLIKES");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserLikeIdNavigations)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USERLIKES_ASPNETUSERS");

                entity.HasOne(d => d.Id1Navigation)
                    .WithMany(p => p.UserLikeId1Navigation)
                    .HasForeignKey(d => d.Id1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERLIKES_ASPNETUSERS1");
            });

            modelBuilder.Entity<UserMusic>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.MusicId })
                    .HasName("PK_USERMUSICS");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserMusics)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USERMUSICS_ASPNETUSERS");

                entity.HasOne(d => d.Music)
                    .WithMany(p => p.UserMusics)
                    .HasForeignKey(d => d.MusicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERMUSICS_MUSICS");
            });

            modelBuilder.Entity<UserNewsletter>(entity =>
            {
                entity.HasKey(e => new { e.NewsletterId, e.Id })
                    .HasName("PK_USERNEWSLETTERS");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserNewsletters)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USERNEWSLETTERS_ASPNETUSERS");

                entity.HasOne(d => d.Newsletter)
                    .WithMany(p => p.UserNewsletters)
                    .HasForeignKey(d => d.NewsletterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERNEWSLETTERS_NEWSLETTERS");
            });

            modelBuilder.Entity<UserProfil>(entity =>
            {
                entity.HasKey(e => new { e.ProfilId, e.Id })
                    .HasName("PK_USERPROFILS");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserProfils)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USERPROFILS_ASPNETUSERS");

                entity.HasOne(d => d.Profil)
                    .WithMany(p => p.UserProfils)
                    .HasForeignKey(d => d.ProfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPROFILS_PROFILS");
            });

            modelBuilder.Entity<UserStyle>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.StyleId })
                    .HasName("PK_USERSTYLES");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserStyles)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK_USERSTYLES_ASPNETUSERS");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.UserStyles)
                    .HasForeignKey(d => d.StyleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERSTYLES_STYLES");
            });

            modelBuilder.Entity<UserSubscription>(entity =>
            {
                entity.HasKey(e => e.UserSubscriptionsId)
                    .HasName("PK_UserSubscriptionsIDID");

                entity.HasOne(d => d.Subscriptions)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.SubscriptionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Subscriptions_ASPNETUSERS");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSubscriptions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_UserSubscriptions_ASPNETUSERS");
            });

            modelBuilder.Entity<UserTrace>(entity =>
            {
                entity.HasKey(e => e.Logid)
                    .HasName("PK_USERTRACES");

                entity.Property(e => e.Ipadress).IsUnicode(false);

                entity.Property(e => e.Pagevisited).IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UserTraces)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USERTRACES_ASPNETUSERS");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
