using Siffrum.Web.Payroll.BAL.Base;
using Siffrum.Web.Payroll.Config;
using Siffrum.Web.Payroll.DAL.Contexts;
using Siffrum.Web.Payroll.DomainModels.v1.Client;
using Siffrum.Web.Payroll.ServiceModels.Exceptions;
using Siffrum.Web.Payroll.ServiceModels.LoggedInIdentity;
using Siffrum.Web.Payroll.ServiceModels.v1.Client;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Siffrum.Web.Payroll.BAL.Client
{
    public class SqlReportProcess : SiffrumPayrollBalOdataBase<SQLReportMasterSM>
    {
        #region --Properties--

        private readonly ILoginUserDetail _loginUserDetail;
        private readonly APIConfiguration _apiConfiguration;

        #endregion --Properties--


        #region --Constructor--

        public SqlReportProcess(IMapper mapper, ILoginUserDetail loginUserDetail, ApiDbContext apiDbContext, APIConfiguration apiConfiguration)
            : base(mapper, apiDbContext)
        {
            _loginUserDetail = loginUserDetail;
            _apiConfiguration = apiConfiguration;
        }

        #endregion --Constructor--

        #region Odata
        public override async Task<IQueryable<SQLReportMasterSM>> GetServiceModelEntitiesForOdata()
        {
            var entitySet = _apiDbContext.SQLReportMasters;
            IQueryable<SQLReportMasterSM> retSM = await MapEntityAsToQuerable<SQLReportMasterDM, SQLReportMasterSM>(_mapper, entitySet);
            return retSM;
        }

        #endregion Odata

        #region --Get--

        /// <summary>
        /// Get All SQLReportMaster details in database
        /// </summary>
        /// <returns>Service Model of List of SQLReportMaster in database</returns>
        public async Task<List<SQLReportMasterSM>> GetAllSqlReport()
        {
            var dm = await _apiDbContext.SQLReportMasters.ToListAsync();
            var sm = _mapper.Map<List<SQLReportMasterSM>>(dm);
            return sm;
        }

        /// <summary>
        /// Get the SqlReports by Id.
        /// </summary>
        /// <param name="id">Primary Key of SqlReportMaster</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <returns>Service Model of SQLReportMaster in database</returns>
        public async Task<SQLReportMasterSM> GetSqlReportsById(int id)
        {
            var sqlReportsDM = await _apiDbContext.SQLReportMasters.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (sqlReportsDM != null)
            {
                return _mapper.Map<SQLReportMasterSM>(sqlReportsDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets Report of Given Query based on Company.
        /// </summary>
        /// <param name="query">String object</param>
        /// <param name="page">Integer Object</param>
        /// <param name="companyId">Primary Key of CLientCOmpanyDetailId</param>
        /// <param name="size">Integer Object that is used for retrieving number of records from a database  </param>
        /// <returns>Service Model of SQLReportMaster in database</returns>

        /*public async Task<SQLReportDataModelSM> SqlReport(string query, int? page, int? companyId, int? size = null)
        {
            size = size.HasValue ? size.Value : 10;
            page = page.HasValue ? page.Value : 1;
            string connectionString = _apiConfiguration.ApiDbConnectionString;
            if (companyId != null)
            {
                if (!query.ToLower().Contains("where"))
                {
                    query += $" where ClientCompanyDetailId = {companyId}";
                }
                else
                {
                    query += $" and ClientCompanyDetailId = {companyId}";
                }

            }
            if (!query.ToLower().Contains("order by"))
                query += " ORDER BY 1 ";
            if (page > 0)
                query += $" OFFSET {((page - 1) * size)} ROWS FETCH NEXT {size} ROWS ONLY";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable("ReportData");
            adapter.Fill(dataTable);
            SQLReportDataModelSM sQLReport = new SQLReportDataModelSM();
            if (dataTable != null)
            {
                sQLReport = new SQLReportDataModelSM();
                sQLReport.ReportDataColumns = new List<SQLReportCellSM>();
                sQLReport.ReportDataRows = new List<SQLReportRowsSM>();
                foreach (DataColumn item in dataTable.Columns)
                {
                    sQLReport.ReportDataColumns.Add(new SQLReportCellSM
                    {
                        CellColumnIndex = item.Ordinal,
                        CellColumnName = item.ColumnName,
                        CellDataType = GetCellDataType(item.DataType.Name),
                        CellValue = item.ColumnName
                    });
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var row = dataTable.Rows[i];
                    var _dataRow = new SQLReportRowsSM();
                    _dataRow.ReportDataCells = new List<SQLReportCellSM>();
                    for (int j = 0; j < row.ItemArray.Length; j++)
                    {
                        var cellType = dataTable.Columns[j].DataType.Name;
                        CellDataType cellDataType = GetCellDataType(cellType);
                        object cellVal = row[j];
                        _dataRow.ReportDataCells.Add(new SQLReportCellSM
                        {
                            CellColumnIndex = j,
                            CellColumnName = dataTable.Columns[j].ColumnName,
                            CellDataType = cellDataType,
                            CellValue = cellVal,
                        });
                    }

                    sQLReport.ReportDataRows.Add(_dataRow);
                }
            }
            sQLReport.PageSize = size.Value;
            sQLReport.PageNo = page.Value;
            sQLReport.ClientCompanyDetailId = companyId;
            //sQLReport.Query = query;
            return sQLReport;
        }*/

        public async Task<SQLReportResponseModel> SqlReportAsync(string query, int? page, int? companyId, int? size = null)
        {
            size = size.HasValue ? size.Value : 10;
            page = page.HasValue ? page.Value : 1;
            string connectionString = _apiConfiguration.ApiDbConnectionString;

            if (companyId != null)
            {
                if (!query.ToLower().Contains("where"))
                {
                    query += $" WHERE ClientCompanyDetailId = {companyId}";
                }
                else
                {
                    query += $" AND ClientCompanyDetailId = {companyId}";
                }
            }

            if (!query.ToLower().Contains("order by"))
                query += " ORDER BY 1";

            if (page > 0)
                query += $" OFFSET {((page - 1) * size)} ROWS FETCH NEXT {size} ROWS ONLY";

            SqlDataAdapter adapter = new SqlDataAdapter(query, connectionString);
            DataTable dataTable = new DataTable("ReportData");
            adapter.Fill(dataTable);

            var response = new SQLReportResponseModel
            {
                PageNo = page.Value,
                PageSize = size.Value,
                ReportData = dataTable,
                DataColumns = new List<SQLReportColumns>(),
                TotalCount = 0  // Will set below
            };

            // Add headers
            foreach (DataColumn column in dataTable.Columns)
            {
                response.DataColumns.Add(new SQLReportColumns
                {
                    ColumnIndex = column.Ordinal,
                    ColumnName = column.ColumnName
                });
            }

            response.TotalCount = dataTable.Rows.Count;

            return response;
        }


        /*private CellDataType GetCellDataType(string cellType)
        {
            var cellDataType = CellDataType.Unknown;
            switch (cellType.ToLower())
            {
                case "string":
                    cellDataType = CellDataType.String;
                    break;
                case "int32":
                    cellDataType = CellDataType.Number;
                    break;
                case "single":
                    cellDataType = CellDataType.Number;
                    break;
                case "double":
                    cellDataType = CellDataType.Number;
                    break;
                case "boolean":
                    cellDataType = CellDataType.Boolean;
                    break;
                case "int64":
                    cellDataType = CellDataType.Number;
                    break;
                case "int16":
                    cellDataType = CellDataType.Number;
                    break;
                case "datetime":
                    cellDataType = CellDataType.Date;
                    break;
                default:
                    break;

            }
            return cellDataType;
        }*/

        #endregion --Get--

        #region My EndPoints

        /// <summary>
        /// Get All SQLReportMaster details of My Company in database.
        /// </summary>
        /// <returns>Service Model of List of SQLReportMaster in database.</returns>
        public async Task<List<SQLReportMasterSM>> GetMySqlReport(int currentCompanyId)
        {
            var dm = await _apiDbContext.SQLReportMasters.Where(x => x.ClientCompanyDetailId == currentCompanyId).ToListAsync();
            var sm = _mapper.Map<List<SQLReportMasterSM>>(dm);
            return sm;
        }



        /// <summary>
        /// Get the Selected Reports by Id.
        /// </summary>
        /// <param name="id">Primary Key of SqlReportMaster</param>
        /// <param name="currentCompanyId">Primary Key of ClientCompanyDetail</param>
        /// <param name="pageNo">Integer Object</param>
        /// <returns>Service Model of SQLReportMaster in database</returns>
        /*public async Task<SQLReportDataModelSM> GetSelectSqlReportsById(int id, int currentCompanyId, int? pageNo)
        {
            var sqlReportsDM = await _apiDbContext.SQLReportMasters.Where(x => x.Id == id).Select(x => x.SQLQuery).FirstOrDefaultAsync();
            if (sqlReportsDM != null)
            {
                var selectReport = SqlReport(sqlReportsDM, pageNo, currentCompanyId);
                return selectReport.Result;
            }

            else
            {
                return null;
            }
        }*/

        public async Task<SQLReportResponseModel> GetSelectSqlReportsByIdAsync(int id, int currentCompanyId, int? pageNo)
        {
            var sqlReportsDM = await _apiDbContext.SQLReportMasters.Where(x => x.Id == id).Select(x => x.SQLQuery).FirstOrDefaultAsync();
            if (sqlReportsDM != null)
            {
                var selectReport = await SqlReportAsync(sqlReportsDM, pageNo, currentCompanyId);
                return selectReport;
            }
            else
            {
                return null;
            }
        }

        #endregion My EndPoints

        #region Add/Update Methods

        /// <summary>
        /// Add new SQLReportMaster
        /// </summary>
        /// <param name="sqlReportAPIModelSM"></param>
        /// <returns>the added record.</returns>

        public async Task<SQLReportMasterSM> AddSqlReport(SQLReportMasterSM sqlReportAPIModelSM)
        {
            var sqlReportDM = _mapper.Map<SQLReportMasterDM>(sqlReportAPIModelSM);
            sqlReportDM.CreatedBy = _loginUserDetail.LoginId;
            sqlReportDM.CreatedOnUTC = DateTime.UtcNow;

            await _apiDbContext.SQLReportMasters.AddAsync(sqlReportDM);
            if (await _apiDbContext.SaveChangesAsync() > 0)
            {
                return _mapper.Map<SQLReportMasterSM>(sqlReportDM);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Update SQLReportMaster of added record
        /// </summary>
        /// <param name="objIdToUpdate"></param>
        /// <param name="sqlReportAPIModelSM"></param>
        /// <returns>Record Updated</returns>
        /// <exception cref="SiffrumPayrollException"></exception>

        public async Task<SQLReportMasterSM> UpdateSqlReport(int objIdToUpdate, SQLReportMasterSM sqlReportAPIModelSM)
        {
            if (sqlReportAPIModelSM != null && objIdToUpdate > 0)
            {
                var isPresent = await _apiDbContext.SQLReportMasters.AnyAsync(x => x.Id == objIdToUpdate);
                if (isPresent)
                {
                    sqlReportAPIModelSM.Id = objIdToUpdate;

                    SQLReportMasterDM dbDM = await _apiDbContext.SQLReportMasters.FindAsync(objIdToUpdate);
                    _mapper.Map(sqlReportAPIModelSM, dbDM);

                    dbDM.LastModifiedBy = _loginUserDetail.LoginId;
                    dbDM.LastModifiedOnUTC = DateTime.UtcNow;

                    if (await _apiDbContext.SaveChangesAsync() > 0)
                    {
                        return _mapper.Map<SQLReportMasterSM>(dbDM);
                    }
                    return null;
                }
                else
                {
                    throw new SiffrumPayrollException(ApiErrorTypeSM.Fatal_Log, $"SqlReport Id not found: {objIdToUpdate}", "Data to update not found, add as new instead.");
                }
            }
            return null;
        }

        #endregion Add/Update Methods

        #region Delete Methods

        /// <summary>
        /// Delete SQLReportMaster by  Id
        /// </summary>
        /// <param name="id">Primary key of SqlMasterReport</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteSqlReportById(int id)
        {
            var isPresent = await _apiDbContext.SQLReportMasters.AnyAsync(x => x.Id == id);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new SQLReportMasterDM() { Id = id };
                _apiDbContext.SQLReportMasters.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Sql Report Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion Delete Methods

        #region My Delete Methods

        /// <summary>
        /// Delete SQLReportMaster by  Id
        /// </summary>
        /// <param name="id">Primary key of SqlMasterReport</param>
        /// <param name="currentCompanyId">primary key of currentCompanyId</param>
        /// <returns>boolean for success in removing record</returns>

        public async Task<DeleteResponseRoot> DeleteMySqlReportById(int id, int currentCompanyId)
        {
            var isPresent = await _apiDbContext.SQLReportMasters.AnyAsync(x => x.Id == id && x.ClientCompanyDetailId == currentCompanyId);

            //Linq to sql syntax
            //(from sub in _apiDbContext.DummySubjects  where sub.ID == id select sub).Any();

            if (isPresent)
            {
                var dmToDelete = new SQLReportMasterDM() { Id = id };
                _apiDbContext.SQLReportMasters.Remove(dmToDelete);
                if (await _apiDbContext.SaveChangesAsync() > 0)
                {
                    return new DeleteResponseRoot(true, "Sql Report Deleted Successfully");
                }
            }
            return new DeleteResponseRoot(false, "Item Not found");

        }

        #endregion My Delete Methods


    }
}
