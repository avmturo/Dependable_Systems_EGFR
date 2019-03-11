using System.Collections.Generic;

namespace DepSystems.Models
{
    /// <summary>
    /// The clinician manager model contains the details of the existing clinicians within the system and
    /// contains the new clinician credential to be added to the system
    /// </summary>
    public class ClinicianManagerModel
    {
        /// <summary>
        /// A list of all of the existing clinicians in the system
        /// </summary>
        public List<Clinician> Clinicians { get; set; }

        /// <summary>
        /// The clinician credentials trying to be imported into the system
        /// </summary>
        public ImportClinicianCredentials ImportedClinicians { get; set; }

        /// <summary>
        /// Constructor that initialises the ImportedClinicians object
        /// </summary>
        public ClinicianManagerModel()
        {
            ImportedClinicians = new ImportClinicianCredentials();
        }
    }
}
