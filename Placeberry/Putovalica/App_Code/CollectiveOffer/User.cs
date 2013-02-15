using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;

namespace Collective
{
    public class User
    {
        public int Id { get; private set; }
        public Guid UserId { get; private set; }
        public string Email { get; private set; }       // email je ujedno i login za asp_membership
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Phone { get; private set; }
        public string Country { get; private set; }
        public string City { get; private set; }
        public string ZipCode { get; private set; }
        public string Street { get; private set; }

        private User(int id, Guid userId, string email, string firstName, string lastName, string phone,
            string country, string city, string zipCode, string street)
        {
            this.Id = id;
            this.UserId = userId;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Phone = phone;
            this.Country = country;
            this.City = city;
            this.ZipCode = zipCode;
            this.Street = street;
        }


        public static User ActivateUser(Guid activationkey)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_ActivateUser"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@ActivationKey", SqlDbType.UniqueIdentifier, 16) { Value = activationkey });

                    SqlParameter parId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parId);
                    SqlParameter parUserId = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parUserId);
                    SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parEmail);
                    SqlParameter parFirstName = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parFirstName);
                    SqlParameter parLastName = new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parLastName);
                    SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parPhone);
                    SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCountry);
                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCity);
                    SqlParameter parZipCode = new SqlParameter("@ZipCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parZipCode);
                    SqlParameter parStreet = new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parStreet);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parUserId.Value == DBNull.Value)
                    {
                        return null;
                    }

                    return new User(
                        Convert.ToInt32(parId.Value),
                        new Guid(parUserId.Value.ToString()),
                        parEmail.Value.ToString(),
                        parFirstName.Value != DBNull.Value ? parFirstName.Value.ToString() : null,
                        parLastName.Value != DBNull.Value ? parLastName.Value.ToString() : null,
                        parPhone.Value != DBNull.Value ? parPhone.Value.ToString() : null,
                        parCountry.Value != DBNull.Value ? parCountry.Value.ToString() : null,
                        parCity.Value != DBNull.Value ? parCity.Value.ToString() : null,
                        parZipCode.Value != DBNull.Value ? parZipCode.Value.ToString() : null,
                        parStreet.Value != DBNull.Value ? parStreet.Value.ToString() : null);
                }
            }
        }

        public static User GetUser(MembershipUser user)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetUser"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16) { Value = user.ProviderUserKey });

                    SqlParameter parId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parId);
                    SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parEmail);
                    SqlParameter parFirstName = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parFirstName);
                    SqlParameter parLastName = new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parLastName);
                    SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parPhone);
                    SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCountry);
                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCity);
                    SqlParameter parZipCode = new SqlParameter("@ZipCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parZipCode);
                    SqlParameter parStreet = new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parStreet);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parId.Value == DBNull.Value)
                    {
                        return null;
                    }

                    return new User(
                        Convert.ToInt32(parId.Value),
                        (Guid)user.ProviderUserKey,
                        parEmail.Value.ToString(),
                        parFirstName.Value != DBNull.Value ? parFirstName.Value.ToString() : null,
                        parLastName.Value != DBNull.Value ? parLastName.Value.ToString() : null,
                        parPhone.Value != DBNull.Value ? parPhone.Value.ToString() : null,
                        parCountry.Value != DBNull.Value ? parCountry.Value.ToString() : null,
                        parCity.Value != DBNull.Value ? parCity.Value.ToString() : null,
                        parZipCode.Value != DBNull.Value ? parZipCode.Value.ToString() : null,
                        parStreet.Value != DBNull.Value ? parStreet.Value.ToString() : null);
                }
            }
        }

        public void UpdateUser(string firstName, string lastName, string phone, string country, string city, string zipCode, string street)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_UpdateUser"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = this.Id });

                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = firstName });
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = lastName });
                    cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Value = phone });
                    cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = country });
                    cmd.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = city });
                    cmd.Parameters.Add(new SqlParameter("@ZipCode", SqlDbType.VarChar, 50) { Value = zipCode });
                    cmd.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Value = street });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    this.FirstName = firstName;
                    this.LastName = lastName;
                    this.Phone = phone;
                    this.Country = country;
                    this.City = city;
                    this.ZipCode = zipCode;
                    this.Street = street;
                }
            }
        }

        public static User GetInactiveUserByEmail(string email)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetInactiveUserByEmail"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = email });

                    SqlParameter parId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parId);
                    SqlParameter parUserId = new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parUserId);
                    SqlParameter parFirstName = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parFirstName);
                    SqlParameter parLastName = new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parLastName);
                    SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parPhone);
                    SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCountry);
                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parCity);
                    SqlParameter parZipCode = new SqlParameter("@ZipCode", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parZipCode);
                    SqlParameter parStreet = new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parStreet);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parId.Value == DBNull.Value)
                    {
                        return null;
                    }

                    return new User(
                        Convert.ToInt32(parId.Value),
                        new Guid(parUserId.Value.ToString()),
                        email,
                        parFirstName.Value != DBNull.Value ? parFirstName.Value.ToString() : null,
                        parLastName.Value != DBNull.Value ? parLastName.Value.ToString() : null,
                        parPhone.Value != DBNull.Value ? parPhone.Value.ToString() : null,
                        parCountry.Value != DBNull.Value ? parCountry.Value.ToString() : null,
                        parCity.Value != DBNull.Value ? parCity.Value.ToString() : null,
                        parZipCode.Value != DBNull.Value ? parZipCode.Value.ToString() : null,
                        parStreet.Value != DBNull.Value ? parStreet.Value.ToString() : null);
                }
            }
        }

        public static User CreateCollectiveUser(MembershipUser user, string activationKey, string firstName, string lastName, string phone, string country, string city, string zipCode, string street)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_CreateUser"
                })
                {
                    SqlParameter parId = new SqlParameter("@Id", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parId);

                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16) { Value = (Guid)user.ProviderUserKey });
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = user.UserName });    // username je na kolektivkama email

                    if (!String.IsNullOrEmpty(activationKey))
                        cmd.Parameters.Add(new SqlParameter("@ActivationKey", SqlDbType.UniqueIdentifier, 16) { Value = new Guid(activationKey) });
                    if (!String.IsNullOrEmpty(firstName))
                        cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = firstName });
                    if (!String.IsNullOrEmpty(lastName))
                        cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = lastName });
                    if (!String.IsNullOrEmpty(phone))
                        cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Value = phone });
                    if (!String.IsNullOrEmpty(country))
                        cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = country });
                    if (!String.IsNullOrEmpty(city))
                        cmd.Parameters.Add(new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = city });
                    if (!String.IsNullOrEmpty(zipCode))
                        cmd.Parameters.Add(new SqlParameter("@ZipCode", SqlDbType.VarChar, 50) { Value = zipCode });
                    if (!String.IsNullOrEmpty(street))
                        cmd.Parameters.Add(new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Value = street });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    if (parId.Value == DBNull.Value)
                    {
                        return null;
                    }

                    return new User(
                        Convert.ToInt32(parId.Value),
                        (Guid)user.ProviderUserKey,
                        user.UserName,
                        firstName,
                        lastName,
                        phone,
                        country,
                        city,
                        zipCode,
                        street);
                }
            }
        }

        public void Delete()
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_DeleteUser"
                })
                {

                    cmd.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int, 4) { Value = this.Id });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            } 
        }

        public static string GetNewActivationKey(Guid userId)
        {
            using (SqlConnection con = new SqlConnection(PutovalicaUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "Collective_GetNewActivationKey"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@UserId", SqlDbType.UniqueIdentifier, 16) { Value = userId });
                    Guid newActKey = Guid.NewGuid();
                    cmd.Parameters.Add(new SqlParameter("@NewActKey", SqlDbType.UniqueIdentifier, 16) { Value = newActKey });

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return newActKey.ToString();
                }
            } 
        }
    }
}