using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServerAspNetIdentity.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        /* Donnée supplémentaire pour la base de donnée */
        [DataType(DataType.Text)]
        public string Firstname { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Phone]
        [DataType(DataType.PhoneNumber)]
        public override string PhoneNumber { get; set; }

        [Column("SEXEID")]
        public short Sexeid { get; set; }

        public short? HairColorId { get; set; }
        public short? CorpulenceId { get; set; }

        public short? HairSizeId { get; set; }
        public short? SubscriptionId { get; set; }
        public short? SexualityId { get; set; }

        public bool QuizCompleted { get; set; }
        public short? ReligionId { get; set; }
        public bool AccountCompleted { get; set; }



        //public bool PictureConfirmed { get; set; }

        //public bool ActiveProfile { get; set; }
    }
}
