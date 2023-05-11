using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.AdoRepositories
{
    public class ParkOutRepository : IRepository<ParkOut>
    {
        public IDbManager DbManager { get;}
        
        public ParkOutRepository(IDbManager dbManager)
        {
            DbManager = dbManager;
        }

        public virtual async Task AddAsync(ParkOut parkOut, CancellationToken cancellationToken)
        {
            try
            {
                SqlCommand comm = DbManager.CreateCommand();
                comm.CommandText = "INSERT INTO ParkOut(InsertTime, TagNumber, CheckIn, CheckOut, ElaspedTime, HourlyFee, Total) " +
                                   "VALUES(@InsertTime, @TagNumber, @CheckIn, @CheckOut, @ElaspedTime, @HourlyFee, @Total); SELECT SCOPE_IDENTITY()";

                comm.Parameters.Add("@InsertTime", SqlDbType.DateTime).Value = DateTime.Now;
                comm.Parameters.Add("@TagNumber", SqlDbType.VarChar).Value = parkOut.TagNumber;
                comm.Parameters.Add("@CheckIn", SqlDbType.DateTime).Value = parkOut.CheckIn;
                comm.Parameters.Add("@CheckOut", SqlDbType.DateTime).Value = parkOut.CheckOut;
                comm.Parameters.Add("@ElaspedTime", SqlDbType.Decimal).Value = parkOut.ElaspedTime;
                comm.Parameters.Add("@HourlyFee", SqlDbType.Decimal).Value = parkOut.HourlyFee;
                comm.Parameters.Add("@Total", SqlDbType.Decimal).Value = parkOut.Total;

                DbManager.OpenConnection();

                int id = await comm.ExecuteNonQueryAsync(cancellationToken);
                parkOut.Id = id;

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
        }

        public virtual async Task DeleteAsync(ParkOut parkOut, CancellationToken cancellationToken)
        {  
            try
            {
                SqlCommand comm = DbManager.CreateCommand();
                comm.CommandText = "DELETR FROM ParkOut WHERE Id = @Id";

                comm.Parameters.Add("@Id", SqlDbType.Int).Value = parkOut.Id;

                DbManager.OpenConnection();
                await comm.ExecuteNonQueryAsync(cancellationToken);
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
        }

        public Task<List<ParkOut>> GetAllAsync(Specification<ParkOut>? specification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(Specification<ParkOut>? specification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Specification<ParkOut>? specification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
