using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OIG_Test.Models
{
    public enum ResearchState
    {
        Concept, //0: het onderzoek is opzettelijk inactief, en kan niet worden afgenomen
        Gepland, //1: de begindatum/tijd van het onderzoek is in de toekomst. Er kunnen nog geen vragen worden beantwoord.
        Lopend,  //2: de begindatum/tijd van het onderzoek is in het verleden, de einddatum/tijd in de toekomst. Alleen op dit moment kunnen vragen worden beantwoord.
        Afgerond //3: de einddatum/tijd van het onderzoek is in het verleden. Er kunnen geen vragen meer worden beantwoord. 
    }

    public class Research
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ResearchId { get; set; }
        [Required(ErrorMessage ="Research requires Name")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime startDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime endDate { get; set; }
        [NotMapped]
        public ResearchState researchState { get; set; } 

        Research ()
        {
            researchState = ResearchState.Concept;
            DateTime currentTime = DateTime.Now;
            
            // Instantiated researches will have a name.
            if (Name != null)
            {
                // Research hasn't started yet.
                if (currentTime < startDate)
                {
                    researchState = ResearchState.Gepland;
                }
                // Research has started.
                else
                {
                    // Research has ended.
                    if (currentTime > endDate)
                    {
                        researchState = ResearchState.Afgerond;
                    }
                    // Research hasn't ended yet.
                    else
                    {
                        researchState = ResearchState.Lopend;
                    }
                }
            }
        }


    }
}
