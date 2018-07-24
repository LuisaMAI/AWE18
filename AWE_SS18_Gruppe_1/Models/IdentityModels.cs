using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace AWE_SS18_Gruppe_1.Models
{
    // Sie können Profildaten für den Benutzer durch Hinzufügen weiterer Eigenschaften zur ApplicationUser-Klasse hinzufügen. Weitere Informationen finden Sie unter "http://go.microsoft.com/fwlink/?LinkID=317594".
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
        public bool? Active { get; set; } = true;
        // Kann mehrere Thesen betreuen
        public virtual ICollection<Thesis> Thesen { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public virtual DbSet<Thesis> ThesisDb { get; set; }
        public virtual DbSet<Programme> ProgrammeDB { get; set; }



    }

    /*
    * Frei = Das Thesisthema ist bislang von niemandem belegt worden.
    * Reserviert = Für die Thesis wurde ein/e Student/in vorgemerkt, die Arbeit ist aber noch nicht angemeldet.
    * Angemeldet = Die Thesis wurde offiziell beim Prüfungsamt angemeldet und befindet sich in Bearbeitung.
    * Abgegeben = Die Thesis wurde abgegeben und muss begutachtet werden.
    * Bewertet = Das Gutachten für die Thesis wurde erstellt.
    * 
    * */

    public enum Status
    {
        frei, reserviert, angemeldet, abgegeben, bewertet

    }

    public enum Typ
    {
        Bachelor, Master
    }

    public class Thesis : IValidatableObject
    {

        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Titel")]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Beschreibung")]
        public string Description { get; set; }
       
        public virtual ApplicationUser User { get; set; }
        [Required]
        public bool Bachelor { get; set; }
        [Required]
        public bool Master { get; set; }
        [Required]
        public Status Status { get; set; } = default(Status);
        [Display(Name = "Name Student")]
        public string StudentName { get; set; }
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Display(Name = "Email Student")]
        public string StudentEmail { get; set; }
        [Display(Name = "Matrikelnummer")]
        public string StudentID { get; set; }
        [Display(Name = "Anmeldedatum")]
        public DateTime? Registration { get; set; }
        [Display(Name = "Abgabedatum")]
        public DateTime? Filing { get; set; }

        public Typ Typ { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Zusammenfassung")]
        public string Summary { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Stärken")]
        public string Strenghts { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Schwächen")]
        public string Weaknesses { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Auswertung")]
        public string Evaluation { get; set; }
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Bewertung Inhalt")]
        public int? ContentVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Bewertung Layout")]
        public int? LayoutVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Bewertung Struktur")]
        public int? StructureVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Bewertung Format")]
        public int? StyleVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Bewertung Literatur")]
        public int? LiteraturVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Schwierigkeitsgrad")]
        public int? DifficultyVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Neuigkeitsgrad")]
        public int? NoveltyVal { get; set; } = 1;
        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Erfülltheitsgrad")]
        public int? RichnessVal { get; set; } = 1;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Inhalt")]
        public int? ContentWt { get; set; } = 30;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Layout")]
        public int? LayoutWt { get; set; } = 15;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Struktur")]
        public int? StructureWt { get; set; } = 10;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Style")]
        public int? StyleWt { get; set; } = 10;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Literatur")]
        public int? LiteratureWt { get; set; } = 10;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Schwierigkeitsgrade")]
        public int? DifficultyWt { get; set; } = 5;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Neuigkeitsgrad")]
        public int? NoveltyWt { get; set; } = 10;
        [Range(0, 100, ErrorMessage = "Fehler! Es sind nur Werte zwischen 0 und 100 erlaubt.")]
        [Display(Name = "Gewichtung Erfülltheitsgrad")]
        public int? RichnessWt { get; set; } = 10;

        [Range(1, 5, ErrorMessage = "Fehler! Es sind nur Zahlen zwischen 1 und 5 erlaubt.")]
        [Display(Name = "Note")]

        public double? Grade { get; set; }

        [Required]
        [Display(Name = "Letzte Änderung")]
        public DateTime LastModified { get; set; } = DateTime.Now;
        [Display(Name = "Programm")]
        public int ProgrammeID { get; set; }
        public virtual Programme Programme { get; set; }

       

        public IEnumerable<ValidationResult> Validate(ValidationContext ValContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();

            if (Status.Equals(Status.frei))
            {
                //auf Bool Status zugreifen

                if (Title == null) res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
                if (Description == null) res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
               
            }
            else if (Status.Equals(Status.reserviert))
            {


                if (Title == null) res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
                if (Description == null) res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
                if (StudentName == null) res.Add(new ValidationResult("Der Name muss bei einer Reservierung angegeben werden.", new[] { "StudentName" }));
                if (StudentEmail == null) res.Add(new ValidationResult("Die E-Mail muss bei der Reservierung angegeben werden.", new[] { "StudentEmail" }));
            }
            else if (Status.Equals(Status.angemeldet))
            {

                if (Title == null) res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
                if (Description == null) res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
                if (StudentName == null) res.Add(new ValidationResult("Der Name muss bei einer Anmeldung angegeben werden.", new[] { "StudentName" }));
                if (StudentEmail == null) res.Add(new ValidationResult("Die E-Mail muss bei einer Anmeldung angegeben werden.", new[] { "StudentEmail" }));
                if (StudentID == null) res.Add(new ValidationResult("Die Matrikelnummer muss bei der Anmeldung angegeben werden.", new[] { "StudentID" }));
                if (Registration == null) res.Add(new ValidationResult("Die E-Mail muss bei einer Anmeldung angegeben werden.", new[] { "Registration" }));
            }
            else if (Status.Equals(Status.abgegeben))
            {


                if (Title == null) res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
                if (Description == null) res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
                if (StudentName == null) res.Add(new ValidationResult("Der Name muss bei der Abgabe angegeben werden.", new[] { "StudentName" }));
                if (StudentEmail == null) res.Add(new ValidationResult("Die E-Mail muss bei der Abgabe angegeben werden.", new[] { "StudentEmail" }));
                if (StudentID == null) res.Add(new ValidationResult("Die Matrikelnummer muss bei der Abgabe angegeben werden.", new[] { "StudentID" }));
                if (Registration == null) res.Add(new ValidationResult("Es muss das Datum der Thesis-Anmeldung abgegeben werden.", new[] { "Registration" }));
                if (Filing == null) res.Add(new ValidationResult("Das Abgabedatum der Thesis muss angegeben werden.", new[] { "Filing" }));
            }
            else
            {

                if (Title == null) res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
                if (Description == null) res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
                if (StudentName == null) res.Add(new ValidationResult("Der Name muss bei der Abgabe angegeben werden.", new[] { "StudentName" }));
                if (StudentEmail == null) res.Add(new ValidationResult("Die E-Mail muss bei der Abgabe angegeben werden.", new[] { "StudentEmail" }));
                if (StudentID == null) res.Add(new ValidationResult("Die Matrikelnummer muss bei der Abgabe angegeben werden.", new[] { "StudentID" }));
                if (Registration == null) res.Add(new ValidationResult("Es muss das Datum der Thesis-Anmeldung abgegeben werden.", new[] { "Registration" }));
                if (Filing == null) res.Add(new ValidationResult("Das Abgabedatum der Thesis muss angegeben werden.", new[] { "Filing" }));
                if (Summary == null) res.Add(new ValidationResult("Es muss zur Bewertung eine Zusammenfassung abgegeben werden. ", new[] { "Summary" }));
                if (Strenghts == null) res.Add(new ValidationResult("Zur Bewertung müssen die Stärken der Arbeit angegeben werden.", new[] { "Strenghts" }));
                if (Weaknesses == null) res.Add(new ValidationResult("Zur Bewertung müssen die Schwächen der Arbeit angegeben werden.", new[] { "Weaknesses" }));
                if (Evaluation == null) res.Add(new ValidationResult("Zur bewertung muss eine Evalutation angegeben werden.", new[] { "Evaluation" }));

                //Hier wird Berechnet, ob die Gewichtungen in Summe 100 ergeben
            }
            int? summeBewertung = ContentWt + LayoutWt + LiteratureWt + StructureWt + StyleWt +  DifficultyWt + NoveltyWt + RichnessWt;
            if (summeBewertung != 100)
            {
                res.Add(new ValidationResult("Es gab einen Fehler bei der Gewichtung der Bewertungseinheiten.", new[] { "ContentWt", "LayoutWt", "LiteratureWt", "StructureWt", "StyleWt", "LiteratureWt", "DifficultyWt", "NoveltyWt", "RichnessWt" }));
            }



            if (Title == null)
            {
                res.Add(new ValidationResult("Es muss ein Titel für die Arbeit angegeben werden.", new[] { "Title" }));
            }
            if (Description == null)
            {
                res.Add(new ValidationResult("Es muss eine Beschreibung angegeben werden.", new[] { "Description" }));
            }

            // Mindestens eins von beiden muss true sein
            if (Bachelor == false && Master == false)
            {
                res.Add(new ValidationResult("Bitte wähle eine Qualifikationsstufe für die Thesis aus.", new[] { "Bachelor", "Master" }));
            }

            return res;
        }

    }

    /*
    * z.B. Wirtschaftsinformatik B.Sc.
    */
    public class Programme
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Programm")]
        public string Name { get; set; }
        public virtual ICollection<Thesis> Thesis { get; set; }
    }

    }
