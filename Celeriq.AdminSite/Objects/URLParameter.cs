namespace Celeriq.AdminSite.Objects
{
    /// <summary>
    /// A query parameter for the URL object
    /// </summary>
    public class URLParameter
    {
        #region Class Members

        protected string _name = "";
        protected string _value = "";

        #endregion

        #region Constructors

        public URLParameter(string name, string value)
        {
            _name = name;
            _value = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The name of the parameter
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The value of the parameter
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion

    }
}