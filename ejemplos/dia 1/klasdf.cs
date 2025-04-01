using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;
using ValiaLegalDAL.Models;
using ValiaLegalBLL.Classes.Helpers;
using ValiaLegalBLL.Models.Classes;
using ValiaLegalBLL.ModelManagers;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using ValiaLegalBLL.Classes.ValiaLegalAttributes;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.Linq.Expressions;
using ValiaLegalBLL.Classes;

namespace ValiaLegalBLL.Models
{
    [DisplayName("Continuity Plan")]
    public class ContinuityPlanViewModel : BaseViewModel<ContinuityPlanViewModel, ContinuityPlan>
    {
        [Required]
        [DisplayName("Related customer ID")]
        [ValiaLegalDescription("Related customer")]
        public int? IdCustomer { get; set; }

        [DisplayName("Related Customer Name")]
        [ValiaLegalDescription("Related Customer Name")]
        public string? CustomerName { get; set; }

        [Required]
        [DisplayName("Related Importance for Business")]
        [ValiaLegalDescription("Related Importance for Business")]
        public int? IdImportanceForBusiness { get; set; }

        [DisplayName("Related Importance for Business Name")]
        [ValiaLegalDescription("Related Importance for Business Name")]
        public string? ImportanceForBusinessName { get; set; }

        [Required]
        [DisplayName("Related Priority")]
        [ValiaLegalDescription("Related Priority")]
        public int? IdPriority { get; set; }

        [DisplayName("Related Priority Name")]
        [ValiaLegalDescription("Related Priority Name")]
        public string? PriorityName { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Title")]
        [ValiaLegalDescription("Title of the continuity plan")]
        public string? Title { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Scenario Description")]
        [ValiaLegalDescription("Description of the scenario")]
        public string? ScenarioDescription { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Associated Information Assets")]
        [ValiaLegalDescription("Information assets associated with the plan")]
        public string? AssociatedInformationAssets { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Scenario Responsible")]
        [ValiaLegalDescription("Person responsible for the scenario")]
        public string? ScenarioResponsible { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("TIC Provider")]
        [ValiaLegalDescription("Technology provider for the plan")]
        public string? TicProvider { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("TIC Provider SLAs")]
        [ValiaLegalDescription("Service level agreements with the TIC provider")]
        public string? TicProviderSlas { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("BIA Risk Analysis")]
        [ValiaLegalDescription("Business Impact Analysis and risk assessment")]
        public string? BiaRiskAnalysis { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("Scenario Management Responsible")]
        [ValiaLegalDescription("Person responsible for managing the scenario")]
        public string? ScenarioManagementResponsible { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Recovery Plan")]
        [ValiaLegalDescription("Detailed recovery plan")]
        public string? RecoveryPlan { get; set; }

        [Required]
        [StringLength(1000)]
        [DisplayName("Human and Technological Resources")]
        [ValiaLegalDescription("Resources required for plan execution")]
        public string? HumanAndTechnologicalResources { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("RTO (min)")]
        [ValiaLegalDescription("Recovery Time Objective in minutes")]
        public string? RtoMin { get; set; }

        [DisplayName("Created at")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime CreatedAt { get; set; }

        [DisplayName("Updated at")]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime? UpdatedAt { get; set; }

        public ContinuityPlanViewModel()
        {
            CustomerName = string.Empty;
            ImportanceForBusinessName = string.Empty;
            PriorityName = string.Empty;
        }

        /// <summary>
        /// Función para inicializar el ViewModel.
        /// </summary>
        /// <returns>Devuelve true si se inicializa correctamente o false en otro caso</returns>
        public override bool InitializeData()
        {
            Id = 0;
            IdCustomer = 0;
            IdImportanceForBusiness = 0;
            IdPriority = 0;
            Title = string.Empty;
            ScenarioDescription = string.Empty;
            AssociatedInformationAssets = string.Empty;
            ScenarioResponsible = string.Empty;
            TicProvider = string.Empty;
            TicProviderSlas = string.Empty;
            BiaRiskAnalysis = string.Empty;
            ScenarioManagementResponsible = string.Empty;
            RecoveryPlan = string.Empty;
            HumanAndTechnologicalResources = string.Empty;
            RtoMin = string.Empty;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ChgTimestamp = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// Función para validar los datos del ViewModel.
        /// </summary>
        /// <param name="localizer">Referencia al objeto IStringLocalizer con los recursos de traducción</param>
        /// <param name="errorMessages">Estructura para devolver los mensajes de error de validación</param>
        /// <returns>Devuelve true si se valida correctamente o false en otro caso</returns>
        public override bool ValidateDataLayer1And2(IStringLocalizer localizer, Dictionary<string, List<string>> errorMessages)
        {
            errorMessages ??= new Dictionary<string, List<string>>();
            bool result = false;
            
            // Validación automática del modelo
            ValidationContext validationContext = new(this);
            List<ValidationResult> validationResults = new();
            result = Validator.TryValidateObject(this, validationContext, validationResults, true);
            
            if (!result)
            {
                foreach (var validationResult in validationResults)
                {
                    string errorMessage = validationResult.ErrorMessage ?? localizer["Unknown error"];
                    foreach (string field in validationResult.MemberNames)
                    {
                        ErrorMessagesHelper.AddErrorMessage(errorMessages, field, errorMessage);
                    }
                }
            }
            
            // Validaciones adicionales
            if (result)
            {
                if (string.IsNullOrEmpty(CustomerName))
                {
                    result = false;
                    string errorMessage = localizer["{0} is mandatory", CodeHelper.GetDisplayNameFromPropertyName<ContinuityPlanViewModel>(nameof(CustomerName))];
                    ErrorMessagesHelper.AddErrorMessage(errorMessages, nameof(CustomerName), errorMessage);
                }
                
                if (IdCustomer <= 0)
                {
                    result = false;
                    string errorMessage = localizer["{0} is mandatory", CodeHelper.GetDisplayNameFromPropertyName<ContinuityPlanViewModel>(nameof(IdCustomer))];
                    ErrorMessagesHelper.AddErrorMessage(errorMessages, nameof(IdCustomer), errorMessage);
                }
                
                if (string.IsNullOrEmpty(ImportanceForBusinessName))
                {
                    result = false;
                    string errorMessage = localizer["{0} is mandatory", CodeHelper.GetDisplayNameFromPropertyName<ContinuityPlanViewModel>(nameof(ImportanceForBusinessName))];
                    ErrorMessagesHelper.AddErrorMessage(errorMessages, nameof(ImportanceForBusinessName), errorMessage);
                }
                
                if (string.IsNullOrEmpty(PriorityName))
                {
                    result = false;
                    string errorMessage = localizer["{0} is mandatory", CodeHelper.GetDisplayNameFromPropertyName<ContinuityPlanViewModel>(nameof(PriorityName))];
                    ErrorMessagesHelper.AddErrorMessage(errorMessages, nameof(PriorityName), errorMessage);
                }
            }
            
            return result;
        }

        public override int CalculateDataQuality()
        {
            return 100;
        }

        public override bool CloneData(ContinuityPlanViewModel fromData)
        {
            if (fromData != null)
            {
                Id = fromData.Id;
                IdCustomer = fromData.IdCustomer;
                IdImportanceForBusiness = fromData.IdImportanceForBusiness;
                IdPriority = fromData.IdPriority;
                Title = fromData.Title;
                ScenarioDescription = fromData.ScenarioDescription;
                AssociatedInformationAssets = fromData.AssociatedInformationAssets;
                ScenarioResponsible = fromData.ScenarioResponsible;
                TicProvider = fromData.TicProvider;
                TicProviderSlas = fromData.TicProviderSlas;
                BiaRiskAnalysis = fromData.BiaRiskAnalysis;
                ScenarioManagementResponsible = fromData.ScenarioManagementResponsible;
                RecoveryPlan = fromData.RecoveryPlan;
                HumanAndTechnologicalResources = fromData.HumanAndTechnologicalResources;
                RtoMin = fromData.RtoMin;

                CreatedAt = fromData.CreatedAt;
                UpdatedAt = fromData.UpdatedAt;
                ChgUser = fromData.ChgUser;
                ChgTimestamp = fromData.ChgTimestamp;
                ChgRowVer = fromData.ChgRowVer;
                
                CustomerName = fromData.CustomerName;
                ImportanceForBusinessName = fromData.ImportanceForBusinessName;
                PriorityName = fromData.PriorityName;
                
                return true;
            }
            return false;
        }

        public override bool CloneDataFromDAL(ContinuityPlan fromDALData)
        {
            if (fromDALData != null)
            {
                Id = fromDALData.Id;
                IdCustomer = fromDALData.IdCustomer;
                IdImportanceForBusiness = fromDALData.IdImportanceForBusines;
                IdPriority = fromData.IdPriority;
                Title = fromDALData.Title;
                ScenarioDescription = fromDALData.ScenarioDescription;
                AssociatedInformationAssets = fromDALData.AssociatedInformationAssets;
                ScenarioResponsible = fromDALData.ScenarioResponsible;
                TicProvider = fromDALData.TicProvider;
                TicProviderSlas = fromDALData.TicProviderSlas;
                BiaRiskAnalysis = fromDALData.BiaRiskAnalysis;
                ScenarioManagementResponsible = fromDALData.ScenarioManagementResponsible;
                RecoveryPlan = fromDALData.RecoveryPlan;
                HumanAndTechnologicalResources = fromDALData.HumanAndTechnologicalResources;
                RtoMin = fromDALData.RtoMin;

                CreatedAt = fromDALData.CreatedAt;
                UpdatedAt = fromDALData.UpdatedAt;
                ChgUser = fromDALData.ChgUser;
                ChgTimestamp = fromDALData.ChgTimestamp;
                ChgRowVer = fromDALData.ChgRowVer;
                
                if (fromDALData.RelatedCustomer != null)
                {
                    CustomerName = fromDALData.RelatedCustomer.Name;
                }
                
                if (fromDALData.RelatedImportanceForBusines != null)
                {
                    ImportanceForBusinessName = fromDALData.RelatedImportanceForBusines.Name;
                }
                
                if (fromDALData.RelatedPriority != null)
                {
                    PriorityName = fromDALData.RelatedPriority.Name;
                }
                
                return true;
            }
            return false;
        }

        public override bool CloneDataToDAL(ContinuityPlan toDALData)
        {
            if (toDALData != null)
            {
                toDALData.Id = Id;
                toDALData.IdCustomer = IdCustomer;
                toDALData.IdImportanceForBusines = IdImportanceForBusiness;
                toDALData.IdPriority = IdPriority;
                toDALData.Title = Title;
                toDALData.ScenarioDescription = ScenarioDescription;
                toDALData.AssociatedInformationAssets = AssociatedInformationAssets;
                toDALData.ScenarioResponsible = ScenarioResponsible;
                toDALData.TicProvider = TicProvider;
                toDALData.TicProviderSlas = TicProviderSlas;
                toDALData.BiaRiskAnalysis = BiaRiskAnalysis;
                toDALData.ScenarioManagementResponsible = ScenarioManagementResponsible;
                toDALData.RecoveryPlan = RecoveryPlan;
                toDALData.HumanAndTechnologicalResources = HumanAndTechnologicalResources;
                toDALData.RtoMin = RtoMin;

                toDALData.CreatedAt = CreatedAt;
                toDALData.UpdatedAt = UpdatedAt;
                toDALData.ChgUser = ChgUser;
                toDALData.ChgTimestamp = ChgTimestamp;
                toDALData.ChgRowVer = ChgRowVer;
                
                return true;
            }
            return false;
        }
    }
}