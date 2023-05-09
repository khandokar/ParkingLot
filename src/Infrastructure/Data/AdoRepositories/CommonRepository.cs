using ApplicationCore.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.AdoRepositories
{
    public class CommonRepository : ICommonRepository
    {
        public IDbManager DbManager { get; }

        public CommonRepository(IDbManager dbManager)
        {
            DbManager = dbManager;
        }

        public async Task<decimal> RevenueOfTheDay(DateTime dateOfRevenue, decimal hourlyFee, CancellationToken cancellationToken = default)
        {
            decimal total = 0;

            try
            {
                SqlCommand comm = DbManager.CreateCommand();

                comm.CommandText = "Select SUM(Total) AS Total from " +
                                  "(" +
                                  "Select IsNULL(Sum(CEILING([dbo].[TotalHours](CheckIn,@dateOfRevenue)) * @hourlyFee),0) AS Total " +
                                  "from ParkIn where CAST(CheckIn as date) = CAST(@dateOfRevenue as date) " +
                                  "UNION ALL " +
                                  "Select IsNULL(Sum(Total),0) AS Total from [dbo].[ParkOut] where CAST(CheckIn as date) = CAST(@dateOfRevenue as date) " +
                                  ") " +
                                  "AS T;";

                comm.Parameters.Add("@dateOfRevenue", SqlDbType.DateTime).Value = dateOfRevenue;
                comm.Parameters.Add("@hourlyFee", SqlDbType.Decimal).Value = hourlyFee;

                DbManager.OpenConnection();

                object obj = await comm.ExecuteScalarAsync(cancellationToken);

                total = Convert.ToDecimal(obj);

                DbManager.CloseConnection();
            }
            catch(SqlException)
            {
                //TODO: Log ErrorCode,SqlState,Errors,ClientConnectionId
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DbManager.CloseConnection();
            }
            return total;
        }

        public async Task<int> AverageNumberOfCar(DateTime fromDate, DateTime toDate, CancellationToken cancellationToken = default)
        {
            int count = 0;

            try
            {

                SqlCommand comm = DbManager.CreateCommand();

                comm.CommandText = "Select Round(SUM(Id)/CAST(DATEDIFF(day, @fromDate, @toDate) as float),0) as C from " +
                                  "( " +
                                  "Select Count(Id) AS Id from [ParkIn] where CAST(@fromDate as date) " +
                                  "<= CAST(CheckIn as date) and CAST(CheckIn as date) <= CAST(@toDate as date) " +
                                  "UNION ALL " +
                                  "Select Count(Id) AS Id from [ParkOut] where CAST(@fromDate as date) " +
                                  "<= CAST(CheckIn as date) and CAST(CheckIn as date) <= CAST(@toDate as date) " +
                                  " ) " +
                                  "AS T";

                comm.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                comm.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;

                DbManager.OpenConnection();

                object obj = await comm.ExecuteScalarAsync(cancellationToken);

                count = Convert.ToInt32(obj);

                DbManager.CloseConnection();
            }
            catch (SqlException)
            {
                //TODO: Log ErrorCode,SqlState,Errors,ClientConnectionId
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DbManager.CloseConnection();
            }
            return count;
        }

        public async Task<decimal> AverageRevenue(DateTime fromDate, DateTime toDate, decimal hourlyFee, CancellationToken cancellationToken = default)
        {
            int count = 0;

            try
            {
                SqlCommand comm = DbManager.CreateCommand();

                comm.CommandText = "Select Sum(Total)/DATEDIFF(day, @fromDate, @toDate) as S from " +
                                  "( " +
                                  "Select IsNULL(Sum(CEILING([dbo].[TotalHours](CheckIn,@toDate)) * @hourlyFee),0) AS Total from [dbo].[ParkIn] " +
                                  "where CAST(@fromDate as date) <= CAST(CheckIn as date) and CAST(CheckIn as date) <= CAST(@toDate as date) " +
                                  "UNION ALL " +
                                  "Select Sum(Total) AS Total from [ParkOut] where CAST(@fromDate as date) " +
                                  "<= CAST(CheckIn as date) and CAST(CheckIn as date) <= CAST(@toDate as date) " +
                                  ") " +
                                  "AS T";

                comm.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                comm.Parameters.Add("@toDate", SqlDbType.DateTime).Value = toDate;
                comm.Parameters.Add("@hourlyFee", SqlDbType.Decimal).Value = hourlyFee;

                DbManager.OpenConnection();

                object obj = await comm.ExecuteScalarAsync(cancellationToken);

                count = Convert.ToInt32(obj);

                DbManager.CloseConnection();
            }
            catch (SqlException)
            {
                //TODO: Log ErrorCode,SqlState,Errors,ClientConnectionId
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DbManager.CloseConnection();
            }
            return count;
        }
    }
}
