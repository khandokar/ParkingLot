using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Infrastructure.Data.AdoRepositories
{
    public class ParkInRepository : IRepository<ParkIn>
    {
        public IDbManager DbManager { get; }

        public ParkInRepository(IDbManager dbManager)
        {
            DbManager = dbManager;
        }

        public virtual async Task AddAsync(ParkIn parkIn, CancellationToken cancellationToken = default)
        {  
            try
            {
                SqlCommand comm = DbManager.CreateCommand();
                comm.CommandText = "INSERT INTO ParkIn(TagNumber, CheckIn) VALUES(@TagNumber, @CheckIn); SELECT SCOPE_IDENTITY()";

                comm.Parameters.Add("@TagNumber", SqlDbType.VarChar).Value = parkIn.TagNumber;
                comm.Parameters.Add("@CheckIn", SqlDbType.DateTime).Value = parkIn.CheckIn;

                DbManager.OpenConnection();

                int id = await comm.ExecuteNonQueryAsync(cancellationToken);

                parkIn.Id = id;
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

        public virtual async Task DeleteAsync(ParkIn parkIn, CancellationToken cancellationToken = default)
        {
            try
            {
                SqlCommand comm = DbManager.CreateCommand();
                comm.CommandText = "DELETE FROM ParkIn where Id = @Id";

                comm.Parameters.Add("@Id", SqlDbType.Int).Value = parkIn.Id;

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

        public virtual async Task<List<ParkIn>> GetAllAsync(Specification<ParkIn>? specification, CancellationToken cancellationToken = default)
        {
            var parkIns = new List<ParkIn>();
            try
            {
                SqlCommand comm = DbManager.CreateCommand();
                comm.CommandText = "Select * FROM ParkIn ";

                if (specification != null && specification.ToParameters().Any())
                {
                    comm.CommandText += "Where ";

                    comm.CommandText += specification.Where;

                    List<SqlParameter> parameters = specification.ToParameters();

                    foreach (SqlParameter parameter in parameters)
                    {
                        comm.Parameters.Add(parameter);
                    }

                    comm.CommandText += string.IsNullOrWhiteSpace(specification.OrderBy) ? " Order By Id" : " Order By " + specification.OrderBy;
                }
                else
                {
                    comm.CommandText += " Order By Id";
                }


                DbManager.OpenConnection();

                using (SqlDataReader reader = await comm.ExecuteReaderAsync(cancellationToken))
                {
                    while (reader != null && reader.Read())
                    {
                        int id = Convert.ToInt32(reader["Id"]);
                        string tagName = Convert.ToString(reader["TagNumber"]);
                        DateTime checkIn = Convert.ToDateTime(reader["CheckIn"]);

                        ParkIn parkIn = new ParkIn(tagName, checkIn);
                        parkIn.Id = id;

                        parkIns.Add(parkIn);
                    }
                }

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
            return parkIns;
        }

        public virtual async Task<bool> AnyAsync(Specification<ParkIn>? specification, CancellationToken cancellationToken = default)
        {
            //nolock 
            bool isThereAny = false;

            try
            {
                SqlCommand comm = DbManager.CreateCommand();

                List<SqlParameter> parameters = specification.ToParameters();

                StringBuilder commandBuilder = new StringBuilder("SELECT CASE WHEN EXISTS (SELECT TOP 1 *  FROM ParkIn ");

                if (!string.IsNullOrEmpty(specification.Where))
                {
                    commandBuilder.Append("Where ");

                    commandBuilder.Append(specification.Where);

                    foreach (SqlParameter parameter in parameters)
                    {
                        comm.Parameters.Add(parameter);
                    }
                }

                commandBuilder.Append(" ) ");

                commandBuilder.Append("THEN CAST (1 AS BIT) ");
                commandBuilder.Append("ELSE CAST (0 AS BIT) END");

                comm.CommandText = commandBuilder.ToString();

                DbManager.OpenConnection();

                object obj = await comm.ExecuteScalarAsync(cancellationToken);

                isThereAny = Convert.ToBoolean(obj);

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

            return isThereAny;
        }

        public virtual async Task<int> CountAsync(Specification<ParkIn>? specification, CancellationToken cancellationToken = default)
        {
            int count = 0;

            try
            {
                SqlCommand comm = DbManager.CreateCommand();

                StringBuilder commandBuilder = new StringBuilder("Select Count(*) FROM ParkIn ");

                if (specification != null && specification.ToParameters().Any())
                {
                    List<SqlParameter> parameters = specification.ToParameters();

                    foreach (SqlParameter parameter in parameters)
                    {
                        comm.Parameters.Add(parameter);
                    }

                    commandBuilder.Append("Where ");

                    commandBuilder.Append(specification.Where);

                }

                comm.CommandText = commandBuilder.ToString();


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
