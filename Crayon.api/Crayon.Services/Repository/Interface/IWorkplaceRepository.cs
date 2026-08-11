using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

public interface IWorkplaceRepository
{
    Task<Result<WorkplaceResponse>> AddWorkplace(WorkplaceRequest request);

    Task<Result<List<WorkplaceResponse>>> GetAllWorkplaces();

    Task<Result<WorkplaceResponse>> GetWorkplaceById(int workplaceId);

    Task<Result<bool>> UpdateWorkplace(int workplaceId, WorkplaceRequest request);

    Task<Result<bool>> DeleteWorkplace(int workplaceId);
}