using System.Data.SqlClient;

namespace ProductCrud.Models
{
    public class DAL
    {
        private readonly string _ConnectionString;
        public DAL(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("DBCS");
        }
        public int InsertProducts(Products product)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString)) 
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertProduct", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Address", product.Address);
                    cmd.Parameters.AddWithValue("@Country", product.Country);
                    cmd.Parameters.AddWithValue("@State", product.State);
                    cmd.Parameters.AddWithValue("@City", product.City);
                    cmd.Parameters.AddWithValue("@ProductionDocument", product.ProductionDocuments);

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
        public Products GetProducts(int id)
        {
            Products product = null;
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetProductById", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Products
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductName = reader["ProductName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                City = reader["City"].ToString(),
                                ProductionDocuments = reader["ProductionDocument"] as string,
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                            };
                        }
                    }
                }
            }
            return product;
        }
         public List<Products> GetAllProducts()
        {
            List<Products> products = new List<Products>();
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAllProducts", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Products product = new Products
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductName = reader["ProductName"].ToString(),
                                Address = reader["Address"].ToString(),
                                Country = reader["Country"].ToString(),
                                State = reader["State"].ToString(),
                                City = reader["City"].ToString(),
                                ProductionDocuments = reader["ProductionDocument"] as string,
                                CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }
        public int UpdateProduct(Products product)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_UpdateProduct", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                    cmd.Parameters.AddWithValue("@Address", product.Address);
                    cmd.Parameters.AddWithValue("@Country", product.Country);
                    cmd.Parameters.AddWithValue("@State", product.State);
                    cmd.Parameters.AddWithValue("@City", product.City);
                    cmd.Parameters.AddWithValue("@ProductionDocument", product.ProductionDocuments);
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public int DeleteProducts(int id)
        {
            using (SqlConnection con = new SqlConnection(_ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_DeleteProduct", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
