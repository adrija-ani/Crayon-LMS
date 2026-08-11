using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Crayon.Services.Repository.Implementation
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly ApplicationDbContext _context;

        public HolidayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<HolidayResponse>> AddHoliday(HolidayRequest request)
        {
            Result<HolidayResponse> result = new();
            try
            {
                bool exists = await _context.HolidaySet
                    .AnyAsync(x =>
                        x.HolidayName == request.HolidayName &&
                        x.HolidayDate.Date == request.HolidayDate.Date);
                if (exists)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Holiday Already Exists"
                    });
                    return result;
                }

                Holiday holiday = new()
                {
                    HolidayName = request.HolidayName,
                    HolidayDate = request.HolidayDate,
                    Country = request.Country
                };
                _context.HolidaySet.Add(holiday);
                await _context.SaveChangesAsync();
                result.Response = new HolidayResponse
                {
                    HolidayId = holiday.HolidayId,
                    HolidayName = holiday.HolidayName,
                    HolidayDate = holiday.HolidayDate,
                    Country = holiday.Country
                };
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<bool>> DeleteHoliday(int holidayId)
        {
            Result<bool> result = new();
            try
            {
                var holiday = await _context.HolidaySet.FirstOrDefaultAsync(x => x.HolidayId == holidayId);
                if (holiday == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Holiday Not Found"
                    });
                    return result;
                }
                _context.HolidaySet.Remove(holiday);
                await _context.SaveChangesAsync();
                result.Response = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<List<HolidayResponse>>> GetAllHolidays()
        {
            Result<List<HolidayResponse>> result = new();
            try
            {
                var holidays = await _context.HolidaySet
                    .Select(x => new HolidayResponse
                    {
                        HolidayId = x.HolidayId,
                        HolidayName = x.HolidayName,
                        HolidayDate = x.HolidayDate,
                        Country = x.Country
                    }).ToListAsync();
                result.Response = holidays;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<HolidayResponse>> GetHolidayById(int holidayId)
        {
            Result<HolidayResponse> result = new();
            try
            {
                var holiday = await _context.HolidaySet
                    .Where(x => x.HolidayId == holidayId)
                    .Select(x => new HolidayResponse
                    {
                        HolidayId = x.HolidayId,
                        HolidayName = x.HolidayName,
                        HolidayDate = x.HolidayDate,
                        Country = x.Country
                    })
                    .FirstOrDefaultAsync();
                if (holiday == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Holiday Not Found"
                    });

                    return result;
                }
                result.Response = holiday;
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
                return result;
            }
        }

        public async Task<Result<bool>> UpdateHoliday(int holidayId,HolidayRequest request)
        {
            Result<bool> result = new();

            try
            {
                var holiday = await _context.HolidaySet
                    .FirstOrDefaultAsync(x => x.HolidayId == holidayId);

                if (holiday == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Holiday Not Found"
                    });

                    return result;
                }

                holiday.HolidayName = request.HolidayName;
                holiday.HolidayDate = request.HolidayDate;
                holiday.Country = request.Country;

                await _context.SaveChangesAsync();

                result.Response = true;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }
    }
}
