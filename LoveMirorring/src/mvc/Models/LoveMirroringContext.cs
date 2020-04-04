using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mvc.Models
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
        public virtual DbSet<Musique> Musiques { get; set; }
        public virtual DbSet<NewsLetter> NewsLetters { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PicturesTag> PicturesTags { get; set; }
        public virtual DbSet<Preference> Preferences { get; set; }
        public virtual DbSet<PreferencesCorpulence> PreferencesCorpulences { get; set; }
        public virtual DbSet<PreferencesHairColor> PreferencesHairColors { get; set; }
        public virtual DbSet<PreferencesHairSize> PreferencesHairSizes { get; set; }
        public virtual DbSet<PreferencesMusique> PreferencesMusiques { get; set; }
        public virtual DbSet<PreferencesReligion> PreferencesReligions { get; set; }
        public virtual DbSet<PreferencesStyle> PreferencesStyles { get; set; }
        public virtual DbSet<Profil> Profils { get; set; }
        public virtual DbSet<ProfilsPreference> ProfilsPreferences { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<Sexuality> Sexualities { get; set; }
        public virtual DbSet<Style> Styles { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Talk> Talks { get; set; }
        public virtual DbSet<UsersExternalService> UsersExternalServices { get; set; }
        public virtual DbSet<UsersMatch> UsersMatches { get; set; }
        public virtual DbSet<UsersNewsLetter> UsersNewsLetters { get; set; }
        public virtual DbSet<UsersPreference> UsersPreferences { get; set; }
        public virtual DbSet<UsersProfil> UsersProfils { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
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
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answers_PROFIL");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Answers_QUESTION");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Birthday).HasDefaultValueSql("('0001-01-01T00:00:00.0000000')");

                entity.HasOne(d => d.Sexe)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SexeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AspNetUsers_SEXE");

                entity.HasOne(d => d.Subscription)
                    .WithMany(p => p.AspNetUsers)
                    .HasForeignKey(d => d.SubscriptionId)
                    .HasConstraintName("FK_AspNetUsers_SUBSCRIPTION");
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
            });

            modelBuilder.Entity<Corpulence>(entity =>
            {
                entity.Property(e => e.CorpulenceName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ExternalService>(entity =>
            {
                entity.Property(e => e.ExternalServiceName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<HairColor>(entity =>
            {
                entity.Property(e => e.HairColorName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<HairSize>(entity =>
            {
                entity.Property(e => e.HairSizeName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK_Message");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_ASPNETUSERS");

                entity.HasOne(d => d.Talk)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.TalkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MESSAGE_TALK");
            });

            modelBuilder.Entity<Musique>(entity =>
            {
                entity.Property(e => e.MusiqueName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<NewsLetter>(entity =>
            {
                entity.Property(e => e.NewsLetterName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Picture>(entity =>
            {
                entity.HasKey(e => e.PictureId)
                    .HasName("PK_Picture");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PICTURE_ASPNETUSERS");
            });

            modelBuilder.Entity<PicturesTag>(entity =>
            {
                entity.HasKey(e => new { e.PictureId, e.TagId });

                entity.HasOne(d => d.Picture)
                    .WithMany(p => p.PicturesTags)
                    .HasForeignKey(d => d.PictureId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PICTURETAG_PICTURE");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.PicturesTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PICTURETAG_TAG");
            });

            modelBuilder.Entity<Preference>(entity =>
            {
                entity.HasOne(d => d.Sexuality)
                    .WithMany(p => p.Preferences)
                    .HasForeignKey(d => d.SexualityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCE_SEXUALITY");
            });

            modelBuilder.Entity<PreferencesCorpulence>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.CorpulenceId })
                    .HasName("PK_PREFERENCECORPULENCE");

                entity.HasOne(d => d.Corpulence)
                    .WithMany(p => p.PreferencesCorpulences)
                    .HasForeignKey(d => d.CorpulenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCECORPULENCE_CORPULENCE");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesCorpulences)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCECORPULENCE_PREFERENCE");
            });

            modelBuilder.Entity<PreferencesHairColor>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.HairColorId })
                    .HasName("PK_PREFERENCEHAIRCOLOR");

                entity.HasOne(d => d.HairColor)
                    .WithMany(p => p.PreferencesHairColors)
                    .HasForeignKey(d => d.HairColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRCOLOR_HAIRCOLOR");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesHairColors)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRCOLOR_PREFERENCE");
            });

            modelBuilder.Entity<PreferencesHairSize>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.HairSizeId });

                entity.HasOne(d => d.HairSize)
                    .WithMany(p => p.PreferencesHairSizes)
                    .HasForeignKey(d => d.HairSizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRSIZE_HAIRSIZE");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesHairSizes)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEHAIRSIZE_PREFERENCE");
            });

            modelBuilder.Entity<PreferencesMusique>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.MusiqueId });

                entity.HasOne(d => d.Musique)
                    .WithMany(p => p.PreferencesMusiques)
                    .HasForeignKey(d => d.MusiqueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEMUSIQUE_MUSIQUE");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesMusiques)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCEMUSIQUE_PREFERENCE");
            });

            modelBuilder.Entity<PreferencesReligion>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.ReligionId })
                    .HasName("PK_PREFERENCERELIGION");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesReligions)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCERELIGION_PREFERENCE");

                entity.HasOne(d => d.Religion)
                    .WithMany(p => p.PreferencesReligions)
                    .HasForeignKey(d => d.ReligionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCERELIGION_RELIGION");
            });

            modelBuilder.Entity<PreferencesStyle>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.StyleId });

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.PreferencesStyles)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCESTYLE_PREFERENCE");

                entity.HasOne(d => d.Style)
                    .WithMany(p => p.PreferencesStyles)
                    .HasForeignKey(d => d.StyleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PREFERENCESTYLE_STYLE");
            });

            modelBuilder.Entity<Profil>(entity =>
            {
                entity.Property(e => e.ProfilDescription)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.ProfilName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ProfilsPreference>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.ProfilId })
                    .HasName("PK_PROFILPREFERENCE");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.ProfilsPreferences)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROFILPREFERENCE_PREFERENCE");

                entity.HasOne(d => d.Profil)
                    .WithMany(p => p.ProfilsPreferences)
                    .HasForeignKey(d => d.ProfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PROFILPREFERENCE_PROFIL");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionText).IsUnicode(false);
            });

            modelBuilder.Entity<Religion>(entity =>
            {
                entity.Property(e => e.ReligionName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Sex>(entity =>
            {
                entity.Property(e => e.SexeName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Sexuality>(entity =>
            {
                entity.Property(e => e.SexualityName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Style>(entity =>
            {
                entity.Property(e => e.StyleName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.Property(e => e.SubscriptionName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.TagName)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Talk>(entity =>
            {
                entity.Property(e => e.TalkName)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.TalkIdNavigations)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALK_ASPNETUSERS");

                entity.HasOne(d => d.IdUser2TalksNavigation)
                    .WithMany(p => p.TalkIdUser2TalksNavigation)
                    .HasForeignKey(d => d.IdUser2Talks)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TALK_ASPNETUSERS1");
            });

            modelBuilder.Entity<UsersExternalService>(entity =>
            {
                entity.HasKey(e => new { e.ExternalServiceId, e.Id });

                entity.HasOne(d => d.ExternalService)
                    .WithMany(p => p.UsersExternalServices)
                    .HasForeignKey(d => d.ExternalServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USEREXTERNALSERVICES_EXTERNALSERVICE");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UsersExternalServices)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USEREXTERNALSERVICES_ASPNETUSERS");
            });

            modelBuilder.Entity<UsersMatch>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Id1 });

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UsersMatchIdNavigations)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERSMATCH_ASPNETUSERS");

                entity.HasOne(d => d.Id1Navigation)
                    .WithMany(p => p.UsersMatchId1Navigation)
                    .HasForeignKey(d => d.Id1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERSMATCH_ASPNETUSERS1");
            });

            modelBuilder.Entity<UsersNewsLetter>(entity =>
            {
                entity.HasKey(e => new { e.NewsLetterId, e.Id });

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UsersNewsLetters)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERNEWSLETTER_ASPNETUSERS");

                entity.HasOne(d => d.NewsLetter)
                    .WithMany(p => p.UsersNewsLetters)
                    .HasForeignKey(d => d.NewsLetterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERNEWSLETTER_NEWSLETTER");
            });

            modelBuilder.Entity<UsersPreference>(entity =>
            {
                entity.HasKey(e => new { e.PreferenceId, e.Id });

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UsersPreferences)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPREFERENCE_ASPNETUSERS");

                entity.HasOne(d => d.Preference)
                    .WithMany(p => p.UsersPreferences)
                    .HasForeignKey(d => d.PreferenceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPREFERENCE_PREFERENCE");
            });

            modelBuilder.Entity<UsersProfil>(entity =>
            {
                entity.HasKey(e => new { e.ProfilId, e.Id });

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.UsersProfils)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPROFIL_ASPNETUSERS");

                entity.HasOne(d => d.Profil)
                    .WithMany(p => p.UsersProfils)
                    .HasForeignKey(d => d.ProfilId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USERPROFIL_PROFIL");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
