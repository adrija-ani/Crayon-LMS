using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using System;
using System.Collections.Generic;
using System.Text;

public interface IHolidayRepository
{
    Task<Result<HolidayResponse>> AddHoliday(HolidayRequest request);
    Task<Result<List<HolidayResponse>>> GetAllHolidays();
    Task<Result<HolidayResponse>> GetHolidayById(int holidayId);
    Task<Result<bool>> UpdateHoliday(int holidayId,HolidayRequest request);
    Task<Result<bool>> DeleteHoliday(int holidayId);
}
