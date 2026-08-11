using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IDesignationRepository
    {
        Task<Result<DesignationResponse>> AddDesignation(DesignationRequest request);
        Task<Result<List<DesignationResponse>>> GetAllDesignations();
        Task<Result<DesignationResponse>> GetDesignationById(int designationId);
        Task<Result<bool>> UpdateDesignation(int designationId, DesignationRequest request);
        Task<Result<bool>> DeleteDesignation(int designationId);
    }
}
