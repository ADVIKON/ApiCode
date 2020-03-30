using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EuforyServices
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {  
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["Demo"].ConnectionString);
            try
            {

            SqlCommand cmd = new SqlCommand("select * from AMPlayerTokens where tokenid= 101", con);
            cmd.CommandType = System.Data.CommandType.Text;
            con.Open();
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            ad.Fill(ds);
            Label1.Text = ds.Rows[0]["location"].ToString();
            con.Close();
            }
            catch (Exception ex)
            {
                con.Close();
                Label1.Text = ex.Message + Environment.NewLine + ex.InnerException;
            }
        }
    }
}