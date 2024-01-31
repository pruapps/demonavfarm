using FarmIT_Api.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace FarmIT_Api.database_accesslayer
{
    public interface IMasterDB
    {
        DataTable Get_Item_Master(int Company_Id,string Nature_Id,int batch_id);
        DataSet Get_Item_Master_Serialno(int Item_id,string inventory_type);
        DataTable Get_OverHeadCost_Master(int Company_Id, string Nature_Id);
        DataTable Get_Location_Master(int Company_Id, string Nature_Id);
        DataTable Get_Breed_Master(int Company_Id, string Nature_Id);
        DataTable Get_Parameter_Master(int Company_Id, string Nature_Id);
        DataTable Get_Parameter_Master_kpi(int Company_Id, string Nature_Id);
        DataTable Get_DataEntryType_Master(int Company_Id, string Nature_Id);
        DataTable Get_LineOfBusiness();
        DataTable Get_FarmType();
        DataSet Get_Livestock_Type(int company_id);
        DataTable Get_ItemCategory(int Nature_id);
        DataTable Get_Uom();
        DataTable Get_Uom(string type);
        DataTable Get_Company();
        DataTable Get_UOM_Conversion_Master(int Company_Id);
        DataTable Get_UOM_Conversion_Details(int uom_id);
        DataTable Get_Animal_Master(int Company_Id);
        DataTable Get_Animal_Detail(int animal_id);
        DataTable Get_Location_List(int Company_id,int Nob_id);
        DataTable Get_Sub_Location_List(int locaation_id,int company_id);
        DataTable Get_SerialNo(int company_id,int location_id, string serial_no);
        DataTable Get_ParameterInput_Detail(int Parameter_id);
        DataTable Get_Animal_Livestock(int Company_Id,int item_id,int batch_id);
        DataTable Get_Animal_Livestock_For_Inventory(int Company_Id, int item_id, int batch_id, int inventory_id);  //created by hk 18-jan-2022
        DataTable Get_GL_Template_Summary(int company_id);
        DataSet Get_GL_CODE(int company_id);
        DataSet Get_dataentry_type_detail_byname(int company_id, int lob_id, string item_category_name);
        DataSet Get_Breed_attribute_summary(int company_id, int breed_id);
        DataSet Get_Item_attribute_summary(int company_id, int item_id);

        DataSet Get_Resource_Card_summary(int company_id);
        DataSet Get_Resource_Card_Load(int resource_card ,string view_by);

        DataTable Get_All_NOB_Location_List(int Nature_id,int company_id);
        DataTable Get_All_Batch_ByLocation_List(int Nature_id, int company_id, string location);
        DataSet Get_All_Item_ByLocation_And_Batch_List(int Nature_id, int company_id, string location, string batch, string category, string transfer_type);

        string Insert_Item_Master(ItemHeader im,DataTable il);
        string Insert_Breed_Master(Breed_Master bm);
        string Insert_Location_Master(Location_Master lm);
        string Insert_Parameter_Master(Parameter_Master pm);
        string Insert_Category_Master(DataEntryType_Master dm);
        string Insert_UOM_Master(int uom_id,string uom_type,string uom,int use_base_uom,string base_uom,decimal conversion,int company_id,int created_by);
        string Insert_ANIMAL_REGISTER_Master(Animal_Master im);
        string Insert_ANIMAL_REGISTER_Master_bulk(DataTable im, int company_id, int created_by);
        string Insert_Parameter_Master_KPI(Parameter_Master_Kpi pm);
        string Insert_GL_Template_Master(GL_Template gl);
        string Delete_location_master(int company_id,int location_id);

        string Insert_breed_Attribute(Breed_Attribute ba);
        string Insert_item_Attribute(item_Attribute ba);
        string Insert_resourcecard(resource_cardModel rs);
    }
    public class MasterDB: IMasterDB
    {
        public readonly string constr;

        private SqlConnection con;

        public MasterDB(string connectionString)
        {
            constr = AesOperation.DecryptString(connectionString);
        }
        private void connection()
        {
            con = new SqlConnection(constr);
        }
        public DataTable Get_Item_Master(int Company_Id,string Nature_Id,int batch_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ITEM_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet Get_Item_Master_Serialno(int item_id,string inventory_type)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ITEM_MASTER_SERIAL", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    cmd.Parameters.AddWithValue("@INVENTORY_TYPE", inventory_type);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_OverHeadCost_Master(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_OVER_HEAD_COST_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Location_Master(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LOCATION_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Breed_Master(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_BREED_MASTER", con))
                {                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Parameter_Master(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PARAMETER_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Parameter_Master_kpi(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_PARAMETER_KPI_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_DataEntryType_Master(int Company_Id, string Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_DATAENTRY_TYPE_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_LineOfBusiness()
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LOB_MASTER", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_FarmType()
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_FARM_TYPE", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet Get_Livestock_Type(int company_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_LIVESTOCK_TYPE", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_ItemCategory(int Nature_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ITEM_CATEGORY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.Parameters.AddWithValue("@NATURE_ID", Nature_Id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Uom()
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UOM", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Uom(string type)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UOM_TypeWise", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@type", type);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Company()
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_COMPANY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_UOM_Conversion_Master(int Company_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UOM_CONVERSION_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_UOM_Conversion_Details(int uom_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_UOM_CONVERSION_Details", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UOM_ID", uom_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public string Insert_Item_Master(ItemHeader im,DataTable il)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_ITEM", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ITEM_ID", im.item_id);
                    cmd.Parameters.AddWithValue("@ITEM_NAME", im.item_name);
                    cmd.Parameters.AddWithValue("@ITEM_CATEGORY", im.item_category);
                    cmd.Parameters.AddWithValue("@UOM", im.uom);
                    cmd.Parameters.AddWithValue("@UNIT_COST", im.unit_cost);
                    cmd.Parameters.AddWithValue("@NATURE_ID", im.industry_id);
                    cmd.Parameters.AddWithValue("@STATUS", im.status);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", im.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", im.created_by);
                    cmd.Parameters.AddWithValue("@Quantity", im.quantity);
                    cmd.Parameters.AddWithValue("@isBatchApp", im.isbatchapp);
                    cmd.Parameters.AddWithValue("@ITEM_TYPE", im.item_type);
                    cmd.Parameters.AddWithValue("@isNegativeAllow", im.isNegativeallow);
                    cmd.Parameters.AddWithValue("@isLOB_ITEM", im.isLobItem);
                    cmd.Parameters.AddWithValue("@Inventory_type", im.Inventory_type);
                    cmd.Parameters.AddWithValue("@tbl_item_sl", il);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_Breed_Master(Breed_Master bm)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_BREED", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BREED_ID", bm.breed_id);
                    cmd.Parameters.AddWithValue("@BREED_NAME", bm.breed_name);
                    cmd.Parameters.AddWithValue("@NATURE_ID", bm.industry_id);
                    cmd.Parameters.AddWithValue("@STATUS", bm.status);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", bm.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", bm.created_by);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_Location_Master(Location_Master lm)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_LOCATION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LOCATION_ID", lm.location_id);
                    cmd.Parameters.AddWithValue("@Primary_location_id", lm.Primary_location_id);
                    cmd.Parameters.AddWithValue("@isLocDefault", lm.islocdefault);
                    cmd.Parameters.AddWithValue("@LOCATION_NAME", lm.location_name);
                    cmd.Parameters.AddWithValue("@LocationType", lm.locationtype);
                    cmd.Parameters.AddWithValue("@FARM_TYPE_ID", lm.farm_type_id);
                    cmd.Parameters.AddWithValue("@ADDRESS", lm.address);
                    cmd.Parameters.AddWithValue("@NATURE_ID", lm.industry_id);
                    cmd.Parameters.AddWithValue("@STATUS", lm.status);
                    cmd.Parameters.AddWithValue("@AREA", lm.area);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", lm.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", lm.created_by);
                    cmd.Parameters.AddWithValue("@Farmer_Name", lm.farmer_name);
                    cmd.Parameters.AddWithValue("@NATURE_ID_LIST", lm.industry_id_list);
                    cmd.Parameters.AddWithValue("@Description", lm.description);
                    cmd.Parameters.AddWithValue("@Location_Entity", lm.location_entry);
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_Parameter_Master(Parameter_Master pm)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_PARAMETER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PARAMETER_ID", pm.parameter_id);
                    cmd.Parameters.AddWithValue("@PARAMETER_NAME", pm.parameter_name);
                    cmd.Parameters.AddWithValue("@PARAMETER_TYPE_ID", pm.parameter_type_id);
                    cmd.Parameters.AddWithValue("@FORMULA_FLAG", pm.formula_flag);
                    cmd.Parameters.AddWithValue("@LOB_ID", pm.lob_id);
                    cmd.Parameters.AddWithValue("@NATURE_ID", pm.industry_id);
                    cmd.Parameters.AddWithValue("@STATUS", pm.status);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", pm.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", pm.created_by);
                    cmd.Parameters.AddWithValue("@Livestock_flag", pm.livestock_flag);

                    cmd.Parameters.AddWithValue("@parameter_input_type", pm.parameter_input_type);
                    cmd.Parameters.AddWithValue("@parameter_input_format", pm.parameter_input_format);
                    cmd.Parameters.AddWithValue("@livestock_type", pm.livestock_type);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_Category_Master(DataEntryType_Master dm)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_DATAENTRY_TYPE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DATAENTRY_TYPE_ID", dm.dataentry_type_id);
                    cmd.Parameters.AddWithValue("@DATAENTRY_TYPE", dm.dataentry_type);
                    cmd.Parameters.AddWithValue("@UOM", dm.uom);
                    cmd.Parameters.AddWithValue("@ALTERNATE_UOM", dm.alternate_uom);
                    cmd.Parameters.AddWithValue("@NOB_ID", dm.nature_id);
                    cmd.Parameters.AddWithValue("@LOB_ID", dm.lob_id);
                    cmd.Parameters.AddWithValue("@STATUS", dm.status);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", dm.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", dm.created_by);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_UOM_Master(int uom_id, string uom_type, string uom, int use_base_uom, string base_uom, decimal conversion, int company_id, int created_by)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_UOM_CONVERSION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CUOM_ID",uom_id);
                    cmd.Parameters.AddWithValue("@UOM_TYPE", uom_type);
                    cmd.Parameters.AddWithValue("@UOM", uom);
                    cmd.Parameters.AddWithValue("@USE_BASE_UOM",use_base_uom);
                    cmd.Parameters.AddWithValue("@BASE_UOM", base_uom);
                    cmd.Parameters.AddWithValue("@UOM_CONVERSION", conversion);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", created_by);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_ANIMAL_REGISTER_Master(Animal_Master im)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_ANIMAL_REGISTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ANIMAL_REG_ID", im.ANIMAL_REG_ID);
                    cmd.Parameters.AddWithValue("@Birth_Date", im.Birth_Date);
                    cmd.Parameters.AddWithValue("@Dad_Number", im.Dad_Number);
                    cmd.Parameters.AddWithValue("@Mom_Number", im.Mom_Number);
                    cmd.Parameters.AddWithValue("@Mom_Breed", im.Mom_Breed);
                    cmd.Parameters.AddWithValue("@Dad_Breed", im.Dad_Breed);
                    cmd.Parameters.AddWithValue("@Livestock_Number", im.Livestock_Number);
                    cmd.Parameters.AddWithValue("@Gender", im.Gender);
                    cmd.Parameters.AddWithValue("@Breed", im.Breed);
                    cmd.Parameters.AddWithValue("@CREATED_BY", im.CREATED_BY);
                    cmd.Parameters.AddWithValue("@Sub_Breed", im.Sub_Breed);
                    cmd.Parameters.AddWithValue("@Shed_Number", im.Shed_Number);
                    cmd.Parameters.AddWithValue("@Remarks", im.Remarks);
                    cmd.Parameters.AddWithValue("@Age_Day", im.Age_Day);
                    cmd.Parameters.AddWithValue("@Age_Month", im.Age_Month);
                    cmd.Parameters.AddWithValue("@Age_Year", im.Age_Year);
                    cmd.Parameters.AddWithValue("@Death_Date", im.Death_Date);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", im.COMPANY_ID);
                    cmd.Parameters.AddWithValue("@LOCATION_ID", im.LOCATION_ID);
                    cmd.Parameters.AddWithValue("@SUB_LOCATION_ID", im.SUB_LOCATION_ID);
                    cmd.Parameters.AddWithValue("@ITEM_ID", im.Item_id);
                    cmd.Parameters.AddWithValue("@Serial_no", im.Serial_No  );
                    cmd.Parameters.AddWithValue("@isPregnent", im.Pregnancy);
                    cmd.Parameters.AddWithValue("@Pregnent_Date", im.Pregnancy_date);
                    cmd.Parameters.AddWithValue("@livestock_type", im.livestock_type);
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public string Insert_ANIMAL_REGISTER_Master_bulk(DataTable im, int company_id, int created_by)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_BULK_INSERT_ANIMAL_REGISTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@tbl_animal", im);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@created_by", created_by);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@message";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 500;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable Get_Animal_Master(int Company_Id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ANIMAL_REGISTER_SUMMARY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Animal_Detail(int animal_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ANIMAL_REGISTER_DETAILS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ANIMAL_REG_ID", animal_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Location_List(int company_id,int nob_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_LOCATION_LIST", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_id", company_id);
                    cmd.Parameters.AddWithValue("@nob_id", nob_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Sub_Location_List(int location_id,int company_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_SUBLOCATION", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_Animal_Livestock(int Company_Id,int item_id,int batch_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ANIMAL_REGISTER_LIVESTOCK", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public DataTable Get_Animal_Livestock_For_Inventory(int Company_Id, int item_id, int batch_id, int inventory_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_ANIMAL_REGISTER_LIVESTOCK_FOR_INVENTORY", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", Company_Id);
                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch_id);
                    cmd.Parameters.AddWithValue("@INVENTORY_ID", inventory_id);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_SerialNo(int company_id,int location_id ,string serial_no)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_SERIALNO", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@serial_no", serial_no);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public string Insert_Parameter_Master_KPI(Parameter_Master_Kpi pm)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_INSERT_PARAMETER_KPI", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PARAMETER_KPI_ID", pm.parameter_kpi_id);
                    cmd.Parameters.AddWithValue("@PARAMETER_ID", pm.parameter_id);
                    cmd.Parameters.AddWithValue("@DATAENTRY_TYPE_ID", pm.dataentry_type_id);
                    cmd.Parameters.AddWithValue("@KPI_TYPE", pm.kpi_type);
                    cmd.Parameters.AddWithValue("@KPI_Value", pm.kpi_value);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", pm.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", pm.created_by);
 
                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataTable Get_ParameterInput_Detail(int parameter_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_PARAMETER_INPUT_DETAIL", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PARAMETER_ID", parameter_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public string Insert_GL_Template_Master(GL_Template gl)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("INSERT_GL_TEMPLATE", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GL_ID", gl.GL_ID);
                    cmd.Parameters.AddWithValue("@GL_CODE", gl.GL_CODE);
                    cmd.Parameters.AddWithValue("@ITEM_CATEGORY_ID", gl.ITEM_CATEGORY_ID);
                    cmd.Parameters.AddWithValue("@PARAMETER_TYPE_ID", gl.PARAMETER_TYPE_ID);
                    cmd.Parameters.AddWithValue("@NOB_ID", gl.NOB_ID);
                    cmd.Parameters.AddWithValue("@LOB_ID", gl.LOB_ID);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", gl.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", gl.created_by);

                    cmd.Parameters.AddWithValue("@WIP_CODE", gl.WIP_CODE);
                    cmd.Parameters.AddWithValue("@AC_TYPE", gl.AC_TYPE);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataTable Get_GL_Template_Summary(int company_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_GL_TEMPLATE_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet Get_GL_CODE(int company_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_GL_CODE", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet Get_dataentry_type_detail_byname(int company_id, int lob_id, string item_category_name)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("Get_dataentry_type_detail_byname", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@lob_id", lob_id);
                    cmd.Parameters.AddWithValue("@item_category_name", item_category_name);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        
       public string Delete_location_master(int company_id,int location_id)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_DELETE_LOCATION_MASTER", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@location_id", location_id);
                    cmd.Parameters.AddWithValue("@company_id", location_id);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public string Insert_breed_Attribute(Breed_Attribute ba)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("INSERT_BREED_ATTRIBUTES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@attribute_form", ba.attribute_form);
                    cmd.Parameters.AddWithValue("@breed_id", ba.breed_id);
                    cmd.Parameters.AddWithValue("@attribute_id", ba.attribute_id);
                    cmd.Parameters.AddWithValue("@attribute_item_id", ba.attribute_item_id);
                    cmd.Parameters.AddWithValue("@value", ba.value);
                    cmd.Parameters.AddWithValue("@name", ba.name);
                    cmd.Parameters.AddWithValue("@text", ba.text);
                    cmd.Parameters.AddWithValue("@uom", ba.uom);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", ba.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", ba.created_by);
                    cmd.Parameters.AddWithValue("@STATUS", ba.status);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataSet Get_Breed_attribute_summary(int company_id,int breed_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_BREED_ATTRIBUTES_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters.AddWithValue("@BREED_ID", breed_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public string Insert_item_Attribute(item_Attribute ba)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("INSERT_ITEM_ATTRIBUTES", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@attribute_form", ba.attribute_form);
                    cmd.Parameters.AddWithValue("@item_id", ba.item_id);
                    cmd.Parameters.AddWithValue("@attribute_id", ba.attribute_id);
                    cmd.Parameters.AddWithValue("@attribute_item_id", ba.attribute_item_id);
                    cmd.Parameters.AddWithValue("@value", ba.value);
                    cmd.Parameters.AddWithValue("@name", ba.name);
                    cmd.Parameters.AddWithValue("@text", ba.text);
                    cmd.Parameters.AddWithValue("@uom", ba.uom);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", ba.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", ba.created_by);
                    cmd.Parameters.AddWithValue("@STATUS", ba.status);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataSet Get_Item_attribute_summary(int company_id, int item_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_ITEM_ATTRIBUTES_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    cmd.Parameters.AddWithValue("@ITEM_ID", item_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public string Insert_resourcecard(resource_cardModel rs)
        {
            string res = "";
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("INSERT_RESOURCE_CARD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RESOURCE_ID", rs.resource_id);
                    cmd.Parameters.AddWithValue("@RESOURCE_NAME",rs.resource_name);
                    cmd.Parameters.AddWithValue("@UOM", rs.uom);
                    cmd.Parameters.AddWithValue("@DIRECT_UNIT_COST", rs.direct_unit_cost);
                    cmd.Parameters.AddWithValue("@IN_DIRECT_UNIT_COST_PER", rs.in_direct_unit_cost_per);
                    cmd.Parameters.AddWithValue("@UNIT_COST", rs.unit_cost);
                    cmd.Parameters.AddWithValue("@UNIT", rs.unit);
                    cmd.Parameters.AddWithValue("@CAPACITY", rs.capacity);
                    cmd.Parameters.AddWithValue("@STATUS", rs.status);
                    cmd.Parameters.AddWithValue("@COMPANY_ID", rs.company_id);
                    cmd.Parameters.AddWithValue("@CREATED_BY", rs.created_by);
                    cmd.Parameters.AddWithValue("@TYPE", rs.type);
                    cmd.Parameters.AddWithValue("@MAINTANANCE_FREQUENCY", rs.maintanance_frequency);
                    cmd.Parameters.AddWithValue("@LAST_MAINTANANCE_DATE", rs.last_maintanance_dt);
                    cmd.Parameters.AddWithValue("@NEXT_DUE_DATE", rs.next_due_date);
                    cmd.Parameters.AddWithValue("@MAINTANANCE_START_DATE", rs.maintanance_start_dt);
                    cmd.Parameters.AddWithValue("@UNDER_MAINTENANCE", rs.under_maintenance);

                    SqlParameter outData = new SqlParameter();
                    outData.ParameterName = "@MESSAGE";
                    outData.SqlDbType = SqlDbType.NVarChar;
                    outData.Size = 200;
                    outData.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outData);
                    con.Open();
                    res = cmd.ExecuteNonQuery().ToString();
                    res = Convert.ToString(outData.Value);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }
        public DataSet Get_Resource_Card_summary(int company_id)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("GET_RESOURCE_CARD_SUMMARY", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@COMPANY_ID", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }

        public DataSet Get_Resource_Card_Load(int resourcecard_id ,string viewby)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_Get_ResourceCardLoad", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@resourcecard_id", resourcecard_id);
                    cmd.Parameters.AddWithValue("@view_by", viewby);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }



        public DataTable Get_All_NOB_Location_List(int nature_id, int company_id)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("USP_GET_ALL_NOB_LOCATION", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", nature_id);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataTable Get_All_Batch_ByLocation_List(int nature_id, int company_id, string location)
        {
            DataTable ds = new DataTable();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_BATCHES_BY_LocationList", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", nature_id);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@LOC_ID", location);
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return ds;
        }
        public DataSet Get_All_Item_ByLocation_And_Batch_List(int nature_id, int company_id, string location,string batch, string category, string transfer_type)
        {
            DataSet ds = new DataSet();
            try
            {
                connection();
                using (SqlCommand cmd = new SqlCommand("PRC_GET_ITEM_BY_LOCATION_AND_BATCH", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NOB_ID", nature_id);
                    cmd.Parameters.AddWithValue("@company_id", company_id);
                    cmd.Parameters.AddWithValue("@LOC_ID", location);
                    cmd.Parameters.AddWithValue("@BATCH_ID", batch);
                    cmd.Parameters.AddWithValue("@CATEGORY", category);
                    cmd.Parameters.AddWithValue("@TRANSFER_TYPE", transfer_type);
                    da.Fill(ds);
                }
            }
            catch (Exception ex){}
            return ds;
        }
    }
}
