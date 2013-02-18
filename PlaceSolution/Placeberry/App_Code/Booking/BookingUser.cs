using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


public class BookingUser
{
    private int _bookingUserId;
    private bool IsBookingUserIdSet = false;
    private bool IsDataSet = false;

    private string _firstName;
    private string _lastName;
    private string _email;
    private string _phone;
    private string _country;
    private string _city;
    private string _street;

    public string FirstName
    {
        get
        {
            CheckDataSet();
            return _firstName;
        }
        private set
        {
            _firstName = value;
        }
    }

    public string LastName
    {
        get
        {
            CheckDataSet();
            return _lastName;
        }
        private set
        {
            _lastName = value;
        }
    }

    public string Email
    {
        get
        {
            CheckDataSet();
            return _email;
        }
        private set
        {
            _email = value;
        }
    }

    public string Phone
    {
        get
        {
            CheckDataSet();
            return _phone;
        }
        private set
        {
            _phone = value;
        }
    }

    public string Country
    {
        get
        {
            CheckDataSet();
            return _country;
        }
        private set
        {
            _country = value;
        }
    }

    public string City
    {
        get
        {
            CheckDataSet();
            return _city;
        }
        private set
        {
            _city = value;
        }
    }

    public string Street
    {
        get
        {
            CheckDataSet();
            return _street;
        }
        private set
        {
            _street = value;
        }
    }


    public bool IsValid { get { return !String.IsNullOrEmpty(this._firstName) && !String.IsNullOrEmpty(this._lastName) && !String.IsNullOrEmpty(this._email); } }


    public BookingUser(int bookingUserId)
    {
        this._bookingUserId = bookingUserId;
        this.IsBookingUserIdSet = true;
        this.IsDataSet = false;
    }

    public BookingUser(string firstName, string lastName, string email, string phone, string country, string city, string street)
	{       
        this._firstName = firstName;
        this._lastName = lastName;
        this._email = email;
        this._phone = phone;
        this._country = country;
        this._city = city;
        this._street = street;

        this.IsBookingUserIdSet = false;
        this.IsDataSet = true;
	}

    // checkMembership provjerava dali je neko vec registriran u bazi sa unesenim emailom. ta bi se provjera trebala koristiti samo na bookingu direktno na placeberryu
    public int GetBookingUserId(bool checkMembership)
    {
        if (!IsBookingUserIdSet)
        {
            using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = con,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetBookingUserId"
                })
                {
                    cmd.Parameters.Add(new SqlParameter("@CheckMembership", SqlDbType.Bit, 1) { Value = checkMembership });
                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Value = this._firstName });
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Value = this._lastName });
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50) { Value = this._email });

                    SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Value = this._phone, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parPhone);
                    SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Value = this._country, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parCountry);
                    SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 50) { Value = this._city, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parCity);
                    SqlParameter parStreet = new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Value = this._street, Direction = ParameterDirection.InputOutput };
                    cmd.Parameters.Add(parStreet);

                    SqlParameter parBookingUserId = new SqlParameter("@BookingUserId", SqlDbType.Int, 4) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(parBookingUserId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // ukoliko email postoji u bazi (u sustavu registriranih korisnika) vratiti ce -1

                    _bookingUserId = Convert.ToInt32(parBookingUserId.Value);

                    // ove ce parametre usera postaviti ukoliko je user u nekoj prosloj rezervaciji unio neobavezna podatke, al ne i u ovoj
                    _phone = parPhone.Value != DBNull.Value ? parPhone.Value.ToString() : string.Empty;
                    _country = parCountry.Value != DBNull.Value ? parCountry.Value.ToString() : string.Empty;
                    _city = parCity.Value != DBNull.Value ? parCity.Value.ToString() : string.Empty;
                    _street = parStreet.Value != DBNull.Value ? parStreet.Value.ToString() : string.Empty;

                    IsBookingUserIdSet = true;
                }
            }
        }

        return _bookingUserId;
    }

    private void CheckDataSet()
    {
        if (!this.IsDataSet)
        {
            SetData();            
        }
    }

    private void SetData()
    {
        using (SqlConnection con = new SqlConnection(PlaceberryUtil.GetConnectionString()))
        {
            using (SqlCommand cmd = new SqlCommand()
            {
                Connection = con,
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetBookingUser"
            })
            {
                cmd.Parameters.Add(new SqlParameter("@BookingUserId", SqlDbType.Int, 4) { Value = this._bookingUserId });

                SqlParameter parFirstName = new SqlParameter("@FirstName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parFirstName);
                SqlParameter parLastName = new SqlParameter("@LastName", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parLastName);
                SqlParameter parEmail = new SqlParameter("@Email", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parEmail);
                SqlParameter parPhone = new SqlParameter("@Phone", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parPhone);
                SqlParameter parCountry = new SqlParameter("@Country", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parCountry);
                SqlParameter parCity = new SqlParameter("@City", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parCity);
                SqlParameter parStreet = new SqlParameter("@Street", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(parStreet);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                this._firstName = parFirstName.Value.ToString();
                this._lastName = parLastName.Value.ToString();
                this._email = parEmail.Value.ToString();

                this._phone = parPhone.Value != DBNull.Value ? parPhone.Value.ToString() : null;
                this._country = parCountry.Value != DBNull.Value ? parCountry.Value.ToString() : null;
                this._city = parCity.Value != DBNull.Value ? parCity.Value.ToString() : null;
                this._street = parStreet.Value != DBNull.Value ? parStreet.Value.ToString() : null;

                this.IsDataSet = true;
            }
        }
    }
}